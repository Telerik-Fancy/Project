using AutoMapper;
using Fancy.Data.Models.Models;
using Fancy.Web.Areas.Items.Models;

namespace Fancy.Web.App_Start.AutomapperProfiles
{
    public class DataToModelView : Profile
    {
        public DataToModelView()
        {
            this.CreateMap<Item, ViewItem>()
                .ForMember(x => x.ImageBase64String, opt => opt.Ignore());
        }
    }
}