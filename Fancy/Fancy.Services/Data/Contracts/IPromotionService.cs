namespace Fancy.Services.Data.Contracts
{
    public interface IPromotionService
    {
        void AddPromotion(int itemId, decimal discount);

        void RemovePromotion(int itemId);
    }
}
