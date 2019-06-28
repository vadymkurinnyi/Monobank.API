using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MonoBank.Api.Requests.Abstractions
{
    /// <summary>
    /// Represents a request to API
    /// </summary>
    /// <typeparam name="TResponse">Type of result expected in result</typeparam>
    public interface IRequest<TResponse>
    {
        bool UseAuthorization { get; }
        /// <summary>
        /// HTTP method of request
        /// </summary>
        HttpMethod Method { get; }

        /// <summary>
        /// API method name
        /// </summary>
        string MethodName { get; }

        /// <summary>
        /// Generate content of HTTP message
        /// </summary>
        /// <returns>Content of HTTP request</returns>
        HttpContent ToHttpContent();
    }
}
