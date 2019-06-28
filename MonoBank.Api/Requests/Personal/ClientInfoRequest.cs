using MonoBank.Api.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MonoBank.Api.Requests.Personal
{
    public class ClientInfoRequest : RequestBase<UserInfo>
    {
        public ClientInfoRequest(): base("/personal/client-info", HttpMethod.Get, true)
        {

        }
    }
}
