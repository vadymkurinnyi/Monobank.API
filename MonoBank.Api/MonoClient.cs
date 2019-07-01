using MonoBank.Api.Args;
using MonoBank.Api.Requests.Abstractions;
using MonoBank.Api.Requests.Bank;
using MonoBank.Api.Requests.Personal;
using MonoBank.Api.Types;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MonoBank.Api
{
    /// <summary>
    /// A client to use the Monobank API
    /// </summary>
    public class MonoClient : IMonoClient
    {
        private const string BASE_URL = "https://api.monobank.ua";
        private readonly string _token;
        private readonly HttpClient _httpClient;
        SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 60);
        /// <summary>
        /// Indicates if receiving statement
        /// </summary>
        public bool IsReceiving { get; set; }

        private CancellationTokenSource _receivingCancellationTokenSource;
        /// <summary>
        /// Create a new <see cref="MonoClient"/> instance.
        /// </summary>
        /// <param name="token">API X-Token</param>
        /// <param name="httpClient">A custom <see cref="HttpClient"/></param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="token"/> format is invalid</exception>
        public MonoClient(string token, HttpClient httpClient = null)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _httpClient = httpClient ?? new HttpClient();
        }
        #region Events
        /// <summary>
        /// Occurs when a <see cref="StatementItem"/> is received.
        /// </summary>
        public event EventHandler<StatementEventArgs> OnStatement;
        /// <summary>
        /// Occurs when an error occurs during the background statement pooling.
        /// </summary>
        public event EventHandler<ErrorEventArgs> OnError;
        #endregion Events
        #region API methods
        /// <summary>
        /// Get a basic list of monobank currency rates. Information is cached and updated no more than once every 5 minutes.
        /// </summary>
        /// <returns>An arrey of <see cref="CurrencyInfo"/></returns>
        public Task<CurrencyInfo[]> GetCurrency()
        => MakeRequestAsync(new CurrencyRequest());
        /// <summary>
        /// Obtaining information about the client and the list of his accounts. Limit on the use of the function no more than 1 time in 60 seconds.
        /// </summary>
        /// <returns>Information about the client and the list of his accounts.</returns>
        public Task<UserInfo> GetClientInfo()
        => MakeRequestAsync(new ClientInfoRequest());
        /// <summary>
        /// Receive an extract for the time from {to} to {to} time in seconds Unix time format. The maximum time for which it is possible to extract an extract is 31 days (2678400 seconds) Limit on the use of the function no more than 1 time in 60 seconds.
        /// </summary>
        /// <param name="account">Account ID from the list Statement list or 0 - default account.</param>
        /// <param name="from">Start time statement</param>
        /// <param name="to">Ending time (if not, the current time will be used)</param>
        /// <returns>An arrey of <see cref="StatementItem"/></returns>
        public Task<StatementItem[]> GetStatements(string account, long from, long? to = null)
        => MakeRequestAsync(new StatementRequest(account, from, to));
        #endregion API methods
        public void StartReceiving(string account, long? from = null, CancellationToken cancellationToken = default)
        {
            if(!from.HasValue)
                from = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            _receivingCancellationTokenSource = new CancellationTokenSource();

            cancellationToken.Register(() => _receivingCancellationTokenSource.Cancel());

            ReceiveAsync(account, from.Value, _receivingCancellationTokenSource.Token);

        }
        private async Task ReceiveAsync(string account, long from, CancellationToken cancellationToken = default)
        {
            IsReceiving = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await semaphore.WaitAsync(cancellationToken).ContinueWith(tsk => { });
                    var tmpFrom = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var result = await MakeRequestAsync(new StatementRequest(account, from)).ConfigureAwait(false);
                    from = tmpFrom;
                    if (result != null)
                    {
                        foreach (var statementItem in result)
                        {
                            OnStatement?.Invoke(this, new StatementEventArgs(account, statementItem));
                        }
                    }
                    await Task.Delay(Convert.ToInt32(Timeout.TotalMilliseconds));
                }
                catch (Exception)
                {

                }
                finally
                {
                    semaphore.Release();
                }

            }
        }
        public void StopReceiving()
        {
            try
            {
                _receivingCancellationTokenSource.Cancel();
            }
            catch (WebException)
            {
            }
            catch (TaskCanceledException)
            {
            }
        }
        public async Task<TResponse> MakeRequestAsync<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            string url = BASE_URL + request.MethodName;

            var httpRequest = new HttpRequestMessage(request.Method, url)
            {
                Content = request.ToHttpContent()
            };
            if (request.UseAuthorization)
                httpRequest.Headers.Add("X-Token", _token);

            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException e)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw;

                throw new Exception("Request timed out");
            }
            var actualResponseStatusCode = httpResponse.StatusCode;
            string responseJson = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
             
            try
            {
                return JsonConvert.DeserializeObject<TResponse>(responseJson);
            }
            catch (Exception ex)
            {
                if (responseJson?.Contains("errorDescription") ?? false)
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseJson);
                    OnError?.Invoke(this, new ErrorEventArgs(errorResponse?.ErrorDescription??$"Code:{actualResponseStatusCode}."));

                }
            }

            return default(TResponse);
        }

    }
}
