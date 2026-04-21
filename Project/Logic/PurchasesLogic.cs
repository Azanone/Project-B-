public class PurchaseLogic
{
    private readonly PurchaseAccess _dataAccess = new();
    public List<PurchaseModel> GetPurchases()
    {
        var allPurchases = _dataAccess.GetAll();
        return allPurchases;
    }
}