namespace Fancy.Services.Common.Contracts
{
    public interface IMappingService
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
