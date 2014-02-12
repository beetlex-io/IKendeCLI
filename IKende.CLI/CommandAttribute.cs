using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute:Attribute
    {
        public CommandAttribute(string name)
        {
            Name = name;
            
            
        }

        public string Name
        {
            get;
            set;
        }

       

        public string Description
        {
            get;
            set;
        }
    }
}
