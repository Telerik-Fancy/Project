using AutoMapper;
using Fancy.Common.Validator;
using Fancy.Services.Common.Contracts;
using System;

namespace Fancy.Services.Common
{
    public class MappingService : IMappingService
    {
        private readonly IMapper mapper;

        public MappingService(IMapper mapper)
        {
            Validator.ValidateNullArgument(mapper, "mapper");

            this.mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            var mappedEntity = this.mapper.Map<TDestination>(source);
            return mappedEntity;
        }
    }
}
