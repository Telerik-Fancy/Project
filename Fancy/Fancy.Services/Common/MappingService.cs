using AutoMapper;
using Bytes2you.Validation;
using Fancy.Services.Common.Contracts;

namespace Fancy.Services.Common
{
    public class MappingService : IMappingService
    {
        private readonly IMapper mapper;

        public MappingService(IMapper mapper)
        {
            Guard.WhenArgument(mapper, nameof(mapper)).IsNull().Throw();

            this.mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            var mappedEntity = this.mapper.Map<TDestination>(source);
            return mappedEntity;
        }
    }
}
