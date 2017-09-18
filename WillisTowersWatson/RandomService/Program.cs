
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
            ioc.Register<IConsoleLogger, ConsoleLogger>(LifeCycle.Singleton);

            //resolve
            var consoleLogger = ioc.Resolve<IConsoleLogger>();
            consoleLogger.SomeValue = "Test";
            consoleLogger.WriteToConsole("Here is a logger we just resolved using IOC !");

            var consoleLogger2 = ioc.Resolve<IConsoleLogger>();

            // this will come back as "Test" if the lifecycle is singleton and as null if the lifecycle is transient.
            Console.WriteLine(consoleLogger2.SomeValue);
            Console.ReadLine();
        }
    }
}