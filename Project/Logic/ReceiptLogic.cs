public class ReceiptLogic
{
    private readonly ReceiptAccess _dataAccess = new();
    public List<ReceiptModel> GetPurchases()
    {
        var allPurchases = _dataAccess.GetAll();
        return allPurchases;
    }

    public List<ReceiptModel> GetPurchasesByAccountID(int accountID)
    {
        var Purchases = _dataAccess.GetPurchasesByAccountID(accountID);
        return Purchases;
    }
}