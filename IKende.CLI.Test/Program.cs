using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI.Test
{
    class Program
    {
        static Parse<LineAnalyzer> mParse = new Parse<LineAnalyzer>();
        static void Main(string[] args)
        {
            mParse.LoadAssembly(typeof(Program).Assembly);
            string line;
            while (true)
            {
                Console.Write(" > ");
                line = Console.ReadLine();
                if (line.IndexOf("?") == 0)
                {
                    DisplayCommands();
                }
                else
                {
                    ParseResult presult = mParse.Execute(line);
                    if (!string.IsNullOrEmpty(presult.Error))
                    {
                       Console.WriteLine( " " + presult.Error);
                    }
                    else{
                        Console.WriteLine(" OK");
                    }
                   
                }
            }
        }
        static void DisplayCommands()
        {
            Console.WriteLine(" Commands:");
            string[] commands = mParse.GetCommands();
            for (int i = 0; i < commands.Length; i++)
            {

                Console.WriteLine("     {0}) {1}", i + 1, commands[i]);
            }
        }
    }
    [Command("login")]
    public class Login:CommandBase
    {
        [Argument("h","-h Host",Regex=@"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)+([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$")]
        public string Host
        {
            get;
            set;
        }
        [Argument("p","-p Port",DefaultValue="8088",Regex=@"^\d{4,5}$")]
        public int Port
        {
            get;
            set;
        }
        [Argument("u", "-u userName")]
        public string UserName
        {
            get;
            set;
        }
        [Argument("w", "-w passWord")]
        public string Pwd
        {
            get;
            set;
        }
    }
    [Command("create app")]
    public class CreateApp : CommandBase
    {
        [Argument("p", "-p processName")]
        public string ProcessName
        {
            get;
            set;
        }
        [Argument("a", "-a appName")]
        public string AppName
        {
            get;
            set;
        }

        [Argument("d", "-d appPath")]
        public string Path
        {
            get;
            set;
        }

        [Argument("w", "-w true|false", Required = false, Regex = REGEX_BOOL)]
        public bool Watch
        {
            get;
            set;
        }
        [Argument("f", "-f *.cs|*.config", Required = false)]
        public string Filter
        {
            get;
            set;
        }

        [Argument("c", "-c true|false", Required = false, Regex = REGEX_BOOL)]
        public bool Compiler { get; set; }

    }


    [Command("create process")]
    public class CreateProcess : CommandBase
    {
        [Argument("p", "-p processName")]
        public string Name
        {
            get;
            set;
        }
        [Argument("r", "-r Remark")]
        public string Remark
        {
            get;
            set;
        }
    }
}
