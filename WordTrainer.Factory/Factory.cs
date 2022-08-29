using System;
using System.Collections.Generic;
using System.Windows;

namespace WordTrainer.Factory
{
    public class Factory
    {
        #region Singletone
        private static Lazy<Factory> instance = new();

        public static Factory Instance => instance.Value;
        #endregion

        private static Dictionary<Type, Type> types;
        
        public static void InitializeFactory()
        {
            types = new();
        }

        public void RegisterType(Type interface_, Type entity) 
        {
            if (interface_ != null && entity != null)
            {
                if (!types.ContainsKey(interface_))
                {
                    types.Add(interface_, entity);
                }
                else throw new Exception("This type is already bounded");
            }
        }

        public E CreateFrameworkElement<E>(Type interface_) where E : FrameworkElement
        {
            E frameworkElement = null;
            if (interface_ != null)
            {
                if (types.ContainsKey(interface_))
                {
                    frameworkElement = (E)Activator.CreateInstance(types[interface_]);
                }
            }
            else
            {
                throw new Exception("This type cannot bound to real type.");
            }

            return frameworkElement;
        }

        public E CreateInstance<E>(Type interface_) where E : class
        {
            E newInstance = null;
            if (interface_ != null)
            {
                if (types.ContainsKey(interface_))
                {
                    newInstance = (E)Activator.CreateInstance(types[interface_]);
                }
            }
            else
            {
                throw new Exception("This type cannot bound to real type.");
            }

            return newInstance;
        }
    }
}
