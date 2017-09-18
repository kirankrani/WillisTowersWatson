using System;

namespace IocContainer
{
    public interface IContainer
    {
        void Register<TTypeToResolve, TConcreteType>();
        void Register(Type typeToResolve, Type concreteType);
        void Register<TTypeToResolve, TConcrete>(LifeCycle lifefCycle);
        void Register(Type typeToResolve, Type concreteType, LifeCycle lifeCycle);
        TTypeToResolve Resolve<TTypeToResolve>();
        object Resolve(Type typeToResolve);
    }
}
