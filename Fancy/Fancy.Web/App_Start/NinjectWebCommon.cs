[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Fancy.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Fancy.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Fancy.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using NinjectModules;
    using WebUtils.Contracts;
    using WebUtils;
    using Controllers;
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                kernel.Bind<IImageProvider>().To<ImageProvider>();
                kernel.Bind<IIdentityProvider>().To<IdentityProvider>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new AuthomapperNinjectModule());
            kernel.Load(new DataNinjectModule());
            kernel.Load(new ServicesNinjectModule());
            kernel.Load(new CommonNinjectModule());
        }        
    }
}
