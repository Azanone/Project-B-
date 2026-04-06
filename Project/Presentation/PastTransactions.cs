public static class PastTransactions
{
    static private PurchaseLogic accountsLogic = new();
    public static void Start()
    {
        ShowAll();
        Console.WriteLine("Enter 1 to return to Admin Menu");
        string input = Console.ReadLine();

        if (input == "1")
        {
            AdminMenu.Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }

    public static void ShowAll()
    {
        var list = accountsLogic.GetPurchases();
        Console.WriteLine("--- TRANSACTION HISTORY ---");
        foreach (var item in list)
        {
            Console.WriteLine($"ID: {item.PurchaseID}| Name: {item.UserName}| Date:-{item.PurchaseDate}| Amount:{item.TotalAmount}");
        }
    }
}