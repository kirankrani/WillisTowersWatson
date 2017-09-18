using System;
using System.Collections.Generic;
using System.Linq;

namespace IocContainer
{
    public class Container : IContainer
    {
        private readonly IList<RegisteredObject> _registeredObjects = new List<RegisteredObject>();
        public void Register<TTypeToResolve, TConcreteType>()
        {
            Register(typeof(TTypeToResolve), typeof(TConcreteType));
        }
        public void Register(Type typeToResolve, Type concreteType)
        {
            Register(typeToResolve, concreteType, LifeCycle.Transient);
        }

        public void Register<TTypeToResolve, TConcrete>(LifeCycle lifefCycle)
        {
            Register(typeof(TTypeToResolve), typeof(TConcrete), lifefCycle);
        }
        public void Register(Type typeToResolve, Type concreteType, LifeCycle lifeCycle)
        {
            _registeredObjects.Add(new RegisteredObject(typeToResolve, concreteType, lifeCycle));
        }

        public TTypeToResolve Resolve<TTypeToResolve>()
        {
            return (TTypeToResolve)Resolve(typeof(TTypeToResolve));
        }
        public object Resolve(Type typeToResolve)
        {

            var registeredObject = _registeredObjects.FirstOrDefault(o => o.TypeToResolve == typeToResolve);
            if (registeredObject != null)
                return GetInstance(registeredObject);

            else if (typeToResolve.IsGenericType)
            {
                registeredObject = _registeredObjects.FirstOrDefault(o => o.TypeToResolve == typeToResolve.GetGenericTypeDefinition());
                if (registeredObject != null)
                {
                    var closedDestination =
                        registeredObject.ConcreteType.MakeGenericType(typeToResolve.GenericTypeArguments);
                    return GetInstance(closedDestination);
                }
            }
            else if (!typeToResolve.IsAbstract)
            {
                return GetInstance(typeToResolve);
            }

            throw new InvalidOperationException($"Could not resolve : {typeToResolve.FullName}. Please make sure it is registered !");
        }

        private object GetInstance(RegisteredObject registeredObject)
        {
            if (registeredObject.Instance != null && registeredObject.LifeCycle != LifeCycle.Transient)
                return registeredObject.Instance;
            registeredObject.Instance = GetInstance(registeredObject.ConcreteType);
            return registeredObject.Instance;
        }

        private object GetInstance(Type typeToResolve)
        {
            var parameters = ResolveConstructorParameters(typeToResolve);
            return Activator.CreateInstance(typeToResolve, parameters);
        }

        private object[] ResolveConstructorParameters(Type type)
        {
            return type.GetConstructors()
              .OrderByDescending(c => c.GetParameters().Count())
              .First()
              .GetParameters()
              .Select(p => Resolve(p.ParameterType))
              .ToArray();
        }
    }
}
