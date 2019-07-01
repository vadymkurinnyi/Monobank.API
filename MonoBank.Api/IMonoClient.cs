using MonoBank.Api.Args;
using MonoBank.Api.Requests.Abstractions;
using MonoBank.Api.Types;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonoBank.Api
{
    /// <summary>
    /// A client interface to use the MonoBank API
    /// </summary>
    public interface IMonoClient
    {
        /// <summary>
        /// Timeout for requests
        /// </summary>
        TimeSpan Timeout { get; set; }
        /// <summary>
        /// Indicates if receiving statments
        /// </summary>
        bool IsReceiving { get; }
        #region Events
        /// <summary>
        /// Occurs when a <see cref="StatementItem"/> is received.
        /// </summary>
        event EventHandler<StatementEventArgs> OnStatement;
        /// <summary>
        /// Occurs when an error occurs during the background statement pooling.
        /// </summary>
        event EventHandler<ErrorEventArgs> OnError;
        #endregion
        #region API methods
        /// <summary>
        /// Get a basic list of monobank currency rates. Information is cached and updated no more than once every 5 minutes.
        /// </summary>
        /// <returns>An arrey of <see cref="CurrencyInfo"/></returns>
        Task<CurrencyInfo[]> GetCurrency();
        /// <summary>
        /// Obtaining information about the client and the list of his accounts. Limit on the use of the function no more than 1 time in 60 seconds.
        /// </summary>
        /// <returns>Information about the client and the list of his accounts.</returns>
        Task<UserInfo> GetClientInfo();
        /// <summary>
        /// Receive an extract for the time from {to} to {to} time in seconds Unix time format. The maximum time for which it is possible to extract an extract is 31 days (2678400 seconds) Limit on the use of the function no more than 1 time in 60 seconds.
        /// </summary>
        /// <param name="account">Account ID from the list Statement list or 0 - default account.</param>
        /// <param name="from">Start time statement</param>
        /// <param name="to">Ending time (if not, the current time will be used)</param>
        /// <returns>An arrey of <see cref="StatementItem"/></returns>
        Task<StatementItem[]> GetStatements(string account, long from, long? to = null);
        #endregion
        /// <summary>
        /// Start statement receiving
        /// </summary>
        /// <param name="account">Account ID from the list Statement list or 0 - default account.</param>
        /// <param name="from">Start time statement</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        void StartReceiving(string account, long? from = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// Stop statement receiving
        /// </summary>
        void StopReceiving();
        /// <summary>
        /// Send a request to Monobank API
        /// </summary>
        /// <typeparam name="TResponse">Type of expected result in the response object</typeparam>
        /// <param name="request">API request object</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result of the API request</returns>
        Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
