using AutoMapper;
using Fancy.Data.Models.Models;
using Fancy.Web.Areas.Items.Models;
using Fancy.Web.Areas.Profile.Models;

namespace Fancy.Web.App_Start.AutomapperProfiles
{
    public class DataToModelView : Profile
    {
        public DataToModelView()
        {
            this.CreateMap<Item, SingleItemViewModel>();
               // .ForMember(x => x.ImageBase64String, opt => opt.Ignore());

            this.CreateMap<Order, OrderViewModel>();
        }
    }
}