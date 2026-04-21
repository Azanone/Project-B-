public class OfferLogic
{
    private readonly OfferAccess _dataAccess = new();
    public List<OfferModel> GetOffers()
    {
        var allOffers = _dataAccess.GetAll();
        return allOffers;
    }
}