public static class ShowProducts
{
    static private ProductLogic productLogic = new();
    static public void Start()
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
        var list = productLogic.GetProducts();
        Console.WriteLine("--- ALL PRODUCTS ---");
        foreach (var item in list)
        {
            Console.WriteLine($"ID: {item.ProductID}| Name: {item.Name}| Category: {item.Category}| Price: €{item.Price}");
        }
    }
}