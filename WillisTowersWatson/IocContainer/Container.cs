using System;
using System.Collections.Generic;
using System.Linq;

namespace IocContainer
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, RegisteredObject> _registeredObjects = new Dictionary<Type, RegisteredObject>();
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
            _registeredObjects.Add(typeToResolve, new RegisteredObject(typeToResolve, concreteType, lifeCycle));
        }

        public TTypeToResolve Resolve<TTypeToResolve>()
        {
            return (TTypeToResolve)Resolve(typeof(TTypeToResolve));
        }
        public object Resolve(Type typeToResolve)
        {
            if (_registeredObjects.ContainsKey(typeToResolve))
                return GetInstance(_registeredObjects[typeToResolve]);

            else if (typeToResolve.IsGenericType)
            {
                var registeredObject = _registeredObjects[typeToResolve.GetGenericTypeDefinition()];
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
            if (registeredObject.LifeCycle == LifeCycle.Singleton)
                return registeredObject.Instance ?? (registeredObject.Instance = GetInstance(registeredObject.ConcreteType));
            return GetInstance(registeredObject.ConcreteType);
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
