using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI
{
   
    public class LineAnalyzer:ILineAnalyzer
    {
        private System.Collections.Specialized.NameValueCollection mProperties = new System.Collections.Specialized.NameValueCollection();

        private const string mCommandRegex = @"-(\w{1})\s+([^-\n]*)";

        public string this[string key]
        {
            get
            {
                return (string)mProperties[key];
            }
            set
            {
                mProperties[key] = value;
            }
        }
        public string Command
        {
            get;
            set;
        }
        public void Execute(string value)
        {

            int index = value.IndexOf("-");
            if (index > 0)
            {
                Command = value.Substring(0, index-1);
                value = value.Substring(index-1, value.Length - index+1);
            }
            else
            {
                Command = value;
                return;
            }
            foreach (System.Text.RegularExpressions.Match item in System.Text.RegularExpressions.Regex.Matches(value, mCommandRegex))
            {
                mProperties[item.Groups[1].Value] = item.Groups[2].Value.Trim();
            }
        }
    }
}
