using System;

namespace RandomService
{
    public interface ILogger
    {

    }

    public interface IConsoleLogger
    {
        string SomeValue { get; set; }
        void WriteToConsole(string log);
    }
    public class SqlServerLogger : ILogger
    {

    }

    public class ConsoleLogger : IConsoleLogger
    {
        public string SomeValue { get; set; }
        public void WriteToConsole(string log)
        {
            Console.WriteLine(log);
        }
    }
    public interface IRepositoy<T>
    {

    }

    public class SqlRepository<T> : IRepositoy<T>
    {
        public SqlRepository(ILogger logger)
        {

        }
    }

    public class InvoiceService
    {
        public InvoiceService(IRepositoy<Employee> repositoy, ILogger logger)
        {

        }
    }

    public class Employee
    {
        public string Name { get; set; }
    }
}
