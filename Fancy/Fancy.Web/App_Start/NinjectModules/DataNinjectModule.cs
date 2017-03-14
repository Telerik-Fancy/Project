using Fancy.Data.Contexts;
using Fancy.Data.Models.Models;
using Fancy.Data.Repositories;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Conventions.Syntax;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Fancy.Web.App_Start.NinjectModules
{
    public class DataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IFancyDbContext>().To<FancyDbContext>().InRequestScope();
            this.Bind(this.BindAllClassesByConvention);
        }

        private void BindAllClassesByConvention(IFromSyntax bind)
        {
            bind
                .FromAssembliesMatching("*.Data.*")
                .SelectAllClasses()
                .Excluding<FancyDbContext>()
                .BindDefaultInterface();
        }
    }
}