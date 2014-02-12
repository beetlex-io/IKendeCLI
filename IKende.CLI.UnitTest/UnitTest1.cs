using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKende.CLI.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private Parse<LineAnalyzer> mParse = new Parse<LineAnalyzer>();

        public UnitTest1()
        {
            mParse.LoadAssembly(typeof(UnitTest1).Assembly);
        }

        [TestMethod]
        public void TestLineAnalyer()
        {
            LineAnalyzer la = new LineAnalyzer();
            la.Execute("execute");
            Assert.AreEqual(la.Command, "execute");
        }
        [TestMethod]
        public void TestLineAnalyerArgument()
        {
            LineAnalyzer la = new LineAnalyzer();
            la.Execute("execute -h 192.168.0.1:80 -u henry -p 123");
            Assert.AreEqual(la.Command, "execute");
            Assert.AreEqual(la["h"], "192.168.0.1:80");
            Assert.AreEqual(la["u"], "henry");
            Assert.AreEqual(la["p"], "123");

            la = new LineAnalyzer();
            la.Execute(@"update -h ftp://192.168.0.0.1/ -l C:\Program Files\IIS Express");
            Assert.AreEqual(la.Command, "update");
            Assert.AreEqual(la["h"], @"ftp://192.168.0.0.1/");
            Assert.AreEqual(la["l"], @"C:\Program Files\IIS Express");
        }

        [TestMethod]
        public void TestCommand()
        {
            ParseResult result = mParse.Execute("execute -h 192.168.0.1:80 -u henry -p 123");
            if (!string.IsNullOrEmpty(result.Error))
            {
                Assert.Fail(result.Error);
            }
            Execute exe = (Execute)result.Command;
            Assert.AreEqual(exe.Host, "192.168.0.1:80");


        }

        [TestMethod]
        public void TestArgumentDefault()
        {
            ParseResult result = mParse.Execute("execute -u henry -p 123");
            if (!string.IsNullOrEmpty(result.Error))
            {
                Assert.Fail(result.Error);
            }
            Execute exe = (Execute)result.Command;
            Assert.AreEqual(exe.Host, "127.0.0.1:80");
        }

        [TestMethod]
        public void TestCreateProcess()
        {
            ParseResult result = mParse.Execute("create process -p ikende -r www.ikende.com");
            if (!string.IsNullOrEmpty(result.Error))
            {
                Assert.Fail(result.Error);
            }
            CreateProcess cmd = (CreateProcess)result.Command;
            Assert.AreEqual(cmd.Name, "ikende");
            Assert.AreEqual(cmd.Remark, "www.ikende.com");
        }
        [TestMethod]
        public void TestCreateApp()
        {
            ParseResult result = mParse.Execute(@"Create App -p ikende -a socketserver -d c:\socket -w true -f *.cs|*.config -c true");
            if (!string.IsNullOrEmpty(result.Error))
            {
                Assert.Fail(result.Error);
            }
            CreateApp cmd = (CreateApp)result.Command;
            Assert.AreEqual(cmd.ProcessName, "ikende");
            Assert.AreEqual(cmd.AppName, "socketserver");
            Assert.AreEqual(cmd.Watch, true);
            Assert.AreEqual(cmd.Filter, "*.cs|*.config");
            Assert.AreEqual(cmd.Path, @"c:\socket");
         
            Assert.AreEqual(cmd.Compiler,true);
        }
        [TestMethod]
        public void TestGetCommands()
        {
            foreach (string cmd in mParse.GetCommands())
            {
                Console.WriteLine(cmd);
            }
        }
    }
    [Command("login")]
    public class Login : CommandBase
    {
        [Argument("h", "-h Host", Regex = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)+([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$")]
        public string Host
        {
            get;
            set;
        }
        [Argument("p", "-p Port", DefaultValue = "8088", Regex = @"^\d{4,5}$")]
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
    public class CreateApp :CommandBase
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

        [Argument("w", "-w true|false", Required=false, Regex = REGEX_BOOL)]
        public bool Watch
        {
            get;
            set;
        }
        [Argument("f", "-f *.cs|*.config",Required=false)]
        public string Filter
        {
            get;
            set;
        }
    
        [Argument("c", "-c true|false", Required = false, Regex = REGEX_BOOL)]
        public bool Compiler { get; set; }
       
    }

    [Command("execute")]
    public class Execute : CommandBase
    {
        [Argument("h", "-h host", Required = true, DefaultValue = "127.0.0.1:80")]
        public string Host
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
        [Argument("p", "-p passWord")]
        public string Pwd
        {
            get;
            set;
        }

       
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
