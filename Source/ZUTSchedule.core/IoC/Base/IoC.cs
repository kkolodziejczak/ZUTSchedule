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
        public static IContainer container;

        /// <summary>
        /// Builder for IoC container
        /// </summary>
        public static ContainerBuilder Builder { get; private set; } = new ContainerBuilder();

        /// <summary>
        /// A shortcut to access the <see cref="INavigationService"/>
        /// </summary>
        public static INavigationService Navigation => Get<INavigationService>();

        /// <summary>
        /// A shortcut to access the <see cref="INavigationService"/>
        /// </summary>
        public static ICredentialManager CredentialManager => Get<ICredentialManager>();

        /// <summary>
        /// A shortcut to access the <see cref="CommunicationService"/>
        /// </summary>
        public static CommunicationService CommunicationService => Get<CommunicationService>();

        /// <summary>
        /// A shortcut to access the <see cref="Storage"/>
        /// </summary>
        public static Storage Settings => Get<Storage>();

        /// <summary>
        /// Fire after <see cref="Builder"/> is set
        /// </summary>
        public static void Setup()
        {
            Builder.RegisterInstance(new CommunicationService());
            container = Builder.Build();
        }

        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return container.Resolve<T>();
        }

    }
}
