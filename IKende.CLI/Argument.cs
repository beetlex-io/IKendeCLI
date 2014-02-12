using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute:Attribute
    {
        public ArgumentAttribute(string name, string description)
        {
            Name = name;
            Description = description;
            Required = true;
        }


        public string Description
        {
            get;
            set;
        }

        public string Regex
        { get; set; }

        public string Name
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public string DefaultValue
        {
            get;
            set;
        }

    }
}
