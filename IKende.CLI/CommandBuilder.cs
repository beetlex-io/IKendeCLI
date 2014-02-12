using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI
{
    public class CommandBuilder:IComparer<CommandBuilder>
    {
        public CommandBuilder(Type type)
        {
            mCommandType = type;
            LoadInfo();
        }

        private List<ArgumentBuilder> mArgumentBuilders = new List<ArgumentBuilder>();

        private Type mCommandType;

        private void LoadInfo()
        {
            CommandAttribute[] ca = (CommandAttribute[])mCommandType.GetCustomAttributes(typeof(CommandAttribute), false);
            if (ca.Length > 0)
            {
                Command = ca[0];
                LoadArgumentsInfo();
            }

        }

        private void LoadArgumentsInfo()
        {
            foreach (System.Reflection.PropertyInfo p in mCommandType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (p.CanWrite)
                {
                    ArgumentAttribute[] aa = (ArgumentAttribute[])p.GetCustomAttributes(typeof(ArgumentAttribute), false);
                    if (aa.Length > 0)
                    {
                        ArgumentBuilder ab = new ArgumentBuilder();
                        ab.Property = p;
                        ab.Argument = aa[0];
                        mArgumentBuilders.Add(ab);
                    }
                }
            }
        }

        public Type CommandType
        {
            get
            {
                return mCommandType;
            }
        }
        public string GetCommand()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Command.Name).Append(" ");
            foreach (ArgumentBuilder ab in mArgumentBuilders)
            {
                sb.Append(ab.Argument.Required ? "<" : "[");
                sb.Append(ab.Argument.Description);
                sb.Append(ab.Argument.Required ? ">" : "]").Append(" ");
            }
            return sb.ToString();
        }
        public ParseResult CreateObject(ILineAnalyzer la)
        {
            ParseResult result = new ParseResult();
            ICommand cmd = (ICommand)Activator.CreateInstance(mCommandType);
            result.Command = cmd;
            foreach (ArgumentBuilder ab in mArgumentBuilders)
            {
                string value = la[ab.Argument.Name];
                if (string.IsNullOrEmpty(value))
                    value = ab.Argument.DefaultValue;
                if (string.IsNullOrEmpty(value) && !ab.Argument.Required)
                    continue;
                if (string.IsNullOrEmpty(value) && ab.Argument.Required)
                {
                    result.Error = CONST_MESSAGE.WRONG_ARGUMENTS;
                    result.Error += "\r\n " + GetCommand();
                    return result;
                }
                object data = ab.GetValue(value);
                if (data == null)
                {
                    result.Error = string.Format(CONST_MESSAGE.ARGUMENT_VALUE_NOT_AVAILABLE, ab.Argument.Name,ab.Argument.Description);
                    return result;
                }
                ab.SetValue(data, cmd);

            }
            try
            {
                result.Command = cmd.Execute();
                result.Error = cmd.Error;
            }
            catch (Exception e_)
            {
                result.Error = e_.Message;
            }
            return result;
        }

        public CommandAttribute Command
        {
            get;
            private set;
        }

        public int Compare(CommandBuilder x, CommandBuilder y)
        {
            return x.Command.Name.CompareTo(y.Command.Name);
        }
    }
}
