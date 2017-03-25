using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Conventions.Syntax;
using Fancy.Data.Contexts;

namespace Fancy.Web.App_Start.NinjectModules
{
    public class DataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IFancyDbContext>().To<FancyDbContext>().InSingletonScope();
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