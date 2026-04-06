public class PurchaseLogic
{
    private readonly PurchaseAcces _dataAccess = new();
    public List<PurchaseModel> GetPurchases()
    {
        var allPurchases = _dataAccess.GetAll();
        return allPurchases;
    }
}