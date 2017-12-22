using Ninject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZUTSchedule.core
{
    public static class IoC
    {
        /// <summary>
        /// The kernel for our IoC container
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// A shortcut to access the <see cref="INavigationService"/>
        /// </summary>
        public static INavigationService Navigation => IoC.Get<INavigationService>();

        /// <summary>
        /// Setup IoC container
        /// </summary>
        public static void Setup()
        {
            BindViewModels();
        }

        /// <summary>
        /// Binds all singleton view models
        /// </summary>
        private static void BindViewModels()
        {
            // Bind to a single instance of Application view model
            Kernel.Bind<MainWindowViewModel>().ToConstant(new MainWindowViewModel());
        }

        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }

    }
}
