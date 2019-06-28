using MonoBank.Api.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MonoBank.Api.Requests.Bank
{
    public class CurrencyRequest : RequestBase<CurrencyInfo[]>
    {
        public CurrencyRequest()
            : base("/bank/currency", HttpMethod.Get)
        {
        }
    }
}
