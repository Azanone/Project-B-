public static class PastTransactions
{
    static private PurchaseLogic accountsLogic = new();
    public static void Start()
    {
        ShowAll();
        string? input = MenuHelpers.Prompt("Enter 1 to return to Admin Menu");
        if (input == "1")
        {
            AdminMenu.Start();
        }
        else
        {
            MenuHelpers.Warn("Invalid input");
            Start();
        }
    }

    public static void ShowAll()
    {
        var list = accountsLogic.GetPurchases();
        MenuHelpers.Announce("--- TRANSACTION HISTORY ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.PurchaseID}| Name: {item.UserName}| Date:-{item.PurchaseDate}| Amount:{item.TotalAmount}");
        }
    }
}