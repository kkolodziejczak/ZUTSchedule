using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZUTSchedule.core
{
    public static class IoC
    {
        /// <summary>
        /// The IoC container
        /// </summary>
        private static IContainer _container;

        /// <summary>
        /// Builder for IoC container
        /// </summary>
        public static ContainerBuilder Builder { get; private set; } = new ContainerBuilder();

        /// <summary>
        /// A shortcut to access the <see cref="INavigationService"/>
        /// </summary>
        public static INavigationService Navigation => IoC.Get<INavigationService>();

        /// <summary>
        /// A shortcut to access the <see cref="Storage"/>
        /// </summary>
        public static Storage Settings => IoC.Get<Storage>();

        /// <summary>
        /// A shortcut to access the <see cref="EDziekanatService"/>
        /// </summary>
        public static EDziekanatService EDziekanatService => IoC.Get<EDziekanatService>();

        /// <summary>
        /// Fire after <see cref="Builder"/> is set
        /// </summary>
        public static void Setup()
        {
            Builder.RegisterInstance(new Storage());
            Builder.RegisterInstance(new EDziekanatService());
            Builder.RegisterInstance(new MainWindowViewModel());
            _container = Builder.Build();
        }

        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return _container.Resolve<T>();
        }

    }
}
