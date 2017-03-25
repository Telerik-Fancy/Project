using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Conventions.Syntax;

namespace Fancy.Web.App_Start.NinjectModules
{
    public class ServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(this.BindAllClassesByConvention);
        }

        private void BindAllClassesByConvention(IFromSyntax bind)
        {
            bind
                .FromAssembliesMatching("*.Services.*")
                .SelectAllClasses()
                .BindDefaultInterface();
        }
    }
}