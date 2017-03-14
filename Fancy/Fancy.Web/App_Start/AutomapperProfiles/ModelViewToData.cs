using AutoMapper;
using Fancy.Data.Models.Models;
using Fancy.Web.Areas.Admin.Models;

namespace Fancy.Web.App_Start.AutomapperProfiles
{
    public class ModelViewToData : Profile
    {
        public ModelViewToData()
        {
            this.CreateMap<AddItemViewModel, Item>()
                .ForMember(x => x.Orders, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}