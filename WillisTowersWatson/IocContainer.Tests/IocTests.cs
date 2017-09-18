using RandomService;
using Xunit;

namespace IocContainer.Tests
{
    public class IocTests
    {
        [Fact]
        public void Can_Resolve_Types()
        {
            var ioc = new Container();
            ioc.Register<ILogger, SqlServerLogger>();

            var logger = ioc.Resolve<ILogger>();

            Assert.Equal(typeof(SqlServerLogger), logger.GetType());

        }

        [Fact]
        public void Can_Resolve_Types_With_Nested_Dependencies()
        {
            var ioc = new Container();
            ioc.Register<ILogger, SqlServerLogger>();
            ioc.Register<IRepositoy<Employee>, SqlRepository<Employee>>();

            var repository = ioc.Resolve<IRepositoy<Employee>>();

            Assert.Equal(typeof(SqlRepository<Employee>), repository.GetType());
        }

        [Fact]
        public void Can_Work_With_UnboudTypes()
        {
            var ioc = new Container();
            ioc.Register<ILogger, SqlServerLogger>();
            ioc.Register(typeof(IRepositoy<>), typeof(SqlRepository<>));

            var service = ioc.Resolve<InvoiceService>();

            Assert.NotNull(service);
        }

        [Fact]
        public void Can_Manage_Lifecycle_Singleton()
        {
            var ioc = new Container();
            ioc.Register<IConsoleLogger, ConsoleLogger>(LifeCycle.Singleton);
            var consoleLogger = ioc.Resolve<IConsoleLogger>();
            var consoleLogger2 = ioc.Resolve<IConsoleLogger>();

            Assert.Same(consoleLogger, consoleLogger2);
        }

        [Fact]
        public void Can_Manage_Lifecycle_Transient()
        {
            var ioc = new Container();
            ioc.Register<IConsoleLogger, ConsoleLogger>(LifeCycle.Transient);
            var consoleLogger = ioc.Resolve<IConsoleLogger>();
            var consoleLogger2 = ioc.Resolve<IConsoleLogger>();

            Assert.NotSame(consoleLogger, consoleLogger2);
        }
    }
}
