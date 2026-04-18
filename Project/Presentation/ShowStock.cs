public static class ShowStock
{
    static private ProductLogic productLogic = new();
    static public void Start()
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
        var list = productLogic.GetProducts();
        MenuHelpers.Announce("--- ALL PRODUCTS ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.ProductID}| Name: {item.Name}| Stock: {(item.Stock > 0 ? item.Stock.ToString() : "OUT OF STOCK")}");
        }
    }
}