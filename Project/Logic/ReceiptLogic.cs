public class ReceiptLogic
{
    private readonly ReceiptAccess _dataAccess = new();


    public static int GetCurrentUserId()
    {
        AccountModel? account = AccountsLogic.CurrentAccount;
        if (account == null)
        {
            throw new InvalidOperationException("User must be logged in.");
        }

        return account.UserId;
    }

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