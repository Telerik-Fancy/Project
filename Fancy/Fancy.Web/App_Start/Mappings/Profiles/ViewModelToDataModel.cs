using AutoMapper;
using Fancy.Data.Models.Models;
using Fancy.Web.Areas.Admin.Models;
using System;

namespace Fancy.Web.App_Start.Mappings.Profiles
{
    public class ViewModelToDataModel : Profile
    {
        [Obsolete]
        protected override void Configure()
        {
            this.CreateMap<AddItemViewModel, Item>()

        }
    }
}