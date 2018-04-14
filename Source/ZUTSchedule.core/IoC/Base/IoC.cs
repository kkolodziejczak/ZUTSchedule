using Autofac;

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
        public static INavigationService Navigation => Get<INavigationService>();

        /// <summary>
        /// A shortcut to access the <see cref="IMessageService"/>
        /// </summary>
        public static IMessageService MessageService => Get<IMessageService>();

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
        public static void Compile()
        {
            _container = Builder.Build();
        }

        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// <para>
        /// NOTE: When <typeparam name="T"/> will be not found default will be returned
        /// </para>
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch
            {
                return default(T);
            }
        }

    }
}
