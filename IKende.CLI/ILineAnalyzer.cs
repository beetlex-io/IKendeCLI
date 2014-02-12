using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI
{
    public interface ILineAnalyzer
    {
        string this[string key]
        {
            get;
            set;
        }
        string Command
        {
            get;
            set;
        }
        void Execute(string value);
    }
}
