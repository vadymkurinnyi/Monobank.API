using MonoBank.Api.Requests.Abstractions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MonoBank.Api.Requests
{
    /// <summary>
    /// Represents a API request
    /// </summary>
    /// <typeparam name="TResponse">Type of result expected in result</typeparam>
    public abstract class RequestBase<TResponse> : IRequest<TResponse>
    {
        /// <inheritdoc />
        public HttpMethod Method { get; }

        /// <inheritdoc />
        public string MethodName { get; protected set; }

        public bool UseAuthorization { get; } = false;

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">API method</param>
        protected RequestBase(string methodName)
            : this(methodName, HttpMethod.Post)
        { }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">API method</param>
        /// <param name="method">HTTP method to use</param>
        protected RequestBase(string methodName, HttpMethod method)
        {
            MethodName = methodName;
            Method = method;
        }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">API method</param>
        /// <param name="method">HTTP method to use</param>
        protected RequestBase(string methodName, HttpMethod method, bool useAuthorization)
        {
            MethodName = methodName;
            Method = method;
            UseAuthorization = useAuthorization;
        }

        /// <summary>
        /// Generate content of HTTP message
        /// </summary>
        /// <returns>Content of HTTP request</returns>
        public virtual HttpContent ToHttpContent()
        {
            string payload = JsonConvert.SerializeObject(this);
            return new StringContent(payload, Encoding.UTF8, "application/json");
        }
    }
}
