using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBank.Api.Args
{
    public class ErrorEventArgs: EventArgs
    {
        public string Description { get; set; }
        public ErrorEventArgs(string description)
        {
            Description = description;
        }
    }
}
