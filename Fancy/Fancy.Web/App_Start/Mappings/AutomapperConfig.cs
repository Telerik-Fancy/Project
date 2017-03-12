using System.Reflection;
using AutoMapper;

namespace Fancy.Web.App_Start.Mappings
{
    public class AutomapperConfig
    {
        public static MapperConfiguration Mapper { get; private set; }

        public static void RegisterMappings()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfiles(Assembly.GetExecutingAssembly().FullName));
        }
    }
}