using MonoBank.Api.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBank.Api.Args
{
    public class StatementEventArgs : EventArgs
    {
        public string Account { get; private set; }
        public StatementItem Statement { get; private set; }
        internal StatementEventArgs(string account, StatementItem statement)
        {
            Account = account;
            Statement = statement;
        }
    }
}
