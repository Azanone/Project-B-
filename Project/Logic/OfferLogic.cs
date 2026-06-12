public class OfferLogic
{
    private readonly OfferAccess _dataAccess = new();
    public List<OfferModel> GetOffers()
    {
        var allOffers = _dataAccess.GetAll();
        return allOffers;
    }

    public Dictionary<int, int> GetProductToOfferMapping()
    {
        return _dataAccess.GetProductToOfferMapping();
    }

    public OfferModel? GetActiveOfferForProduct(long productId, List<OfferModel>? offers = null, Dictionary<int, int>? productOfferMap = null)
    {
        offers ??= GetOffers();
        productOfferMap ??= GetProductToOfferMapping();

        if (!productOfferMap.TryGetValue((int)productId, out int offerId))
        {
            return null;
        }

        DateTime today = DateTime.Today;
        return offers.FirstOrDefault(o => o.OfferID == offerId &&
                                          today >= o.StartDate.Date &&
                                          today <= o.EndDate.Date);
    }

    public bool HasActiveOffer(long productId, List<OfferModel>? offers = null, Dictionary<int, int>? productOfferMap = null)
    {
        return GetActiveOfferForProduct(productId, offers, productOfferMap) != null;
    }
}