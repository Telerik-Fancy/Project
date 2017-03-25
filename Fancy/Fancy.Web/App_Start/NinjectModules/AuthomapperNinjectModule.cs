using AutoMapper;
using Ninject.Activation;
using Ninject.Modules;
using Fancy.Web.App_Start.AutomapperProfiles;

namespace Fancy.Web.App_Start.NinjectModules
{
    public class AuthomapperNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IMapper>().ToMethod(this.IMapperFactoryMethod).InSingletonScope();
        }

        private IMapper IMapperFactoryMethod(IContext context)
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile<ModelViewToData>();
                configuration.AddProfile<DataToModelView>();
            });

            mapperConfiguration.AssertConfigurationIsValid();

            return mapperConfiguration.CreateMapper();
        }
    }
}