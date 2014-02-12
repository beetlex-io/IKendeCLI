using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI
{
    public class Parse<T> where T :ILineAnalyzer,new()
    {
        public Parse()
        {
            
        }
        
        private List<CommandBuilder> mCommands = new List<CommandBuilder>();

        public void LoadAssembly(System.Reflection.Assembly assembly)
        { 
            foreach(Type type in assembly.GetTypes())
            {
                if(type.GetCustomAttributes(typeof(CommandAttribute),false).Length>0)
                {
                    Load(type);
                }
            }
        }

        public void Load<T>() where T : ICommand, new()
        {
            Load(typeof(T));
            
        }

        public void Load(Type type)
        {
            CommandBuilder cb = new CommandBuilder(type);
            mCommands.Add(cb);
        }

        public List<CommandBuilder> Commands
        {
            get{
                return mCommands;
            }
             
        }

        public ParseResult Execute(string command)
        {
            ParseResult result = null;
            ILineAnalyzer la = new T();
            la.Execute(command);
            foreach (CommandBuilder cb in mCommands)
            {
                if (la.Command.IndexOf(cb.Command.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    result = cb.CreateObject(la);
                }
            }

            if (result == null)
            {
                result = new ParseResult();
                result.Error = string.Format(CONST_MESSAGE.COMMAND_NOT_FOUND, la.Command);
            }
            return result;
        }

        public string[] GetCommands()
        {
            string[] cmds = new string[mCommands.Count];
            for (int i = 0; i < mCommands.Count; i++)
            {
                cmds[i] = mCommands[i].GetCommand();
            }
            Array.Sort<string>(cmds, new CommandSort());
            return cmds;
            
        }

        class CommandSort : IComparer<string>
        {

            public int Compare(string x, string y)
            {
                return x.CompareTo(y);
            }
        }
    }
}
