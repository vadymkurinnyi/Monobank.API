using MonoBank.Api.Types;
using System.Net.Http;

namespace MonoBank.Api.Requests.Personal
{
    public class StatementRequest : RequestBase<StatementItem[]>
    {
        public StatementRequest(string account, long from, long? to = null):base($"/personal/statement/{account}/{from}/{to}", HttpMethod.Get, true)
        {
        }
    }
}
