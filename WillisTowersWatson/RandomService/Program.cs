
using IocContainer;
using System;

namespace RandomService
{
    class Program
    {
        static void Main(string[] args)
        {
            var ioc = new Container();

            //register
            ioc.Register<IConsoleLogger, ConsoleLogger>(LifeCycle.Transient);

            //resolve
            var consoleLogger = ioc.Resolve<IConsoleLogger>();
            consoleLogger.SomeValue = "Test";
            consoleLogger.WriteToConsole("Here is a logger we just resolved using IOC !");

            var consoleLogger2 = ioc.Resolve<IConsoleLogger>();

            Console.WriteLine(consoleLogger2.SomeValue);
            Console.ReadLine();
        }
    }
}