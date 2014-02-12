using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI
{
    public interface ICommand
    {

        string Error
        {
            get;
            set;
        }

        object Execute();
    }
    public class CommandBase : ICommand
    {
        public const string REGEX_BOOL = "^(True)|(true)|(False)|(false)$";
        public string Error
        {
            get;
            set;
        }

        public virtual object  Execute()
        {
            return this;
        }
    }
}
