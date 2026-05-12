public static class ShowProducts
{
    static private ProductLogic productLogic = new();
    static public void Start()
    {
        ShowAll();
        MenuHelpers.PromptReturnToMenu("Enter 1 to return to Admin Menu", AdminMenu.Start);
    }

    public static void ShowAll()
    {
        var list = productLogic.GetProducts();
        MenuHelpers.Announce("--- ALL PRODUCTS ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.ProductID}| Name: {item.Name}| Category: {item.Category}| Price: {item.Price} EUR");
        }
    }
}