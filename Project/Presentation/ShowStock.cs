public static class ShowStock
{
    static private ProductLogic productLogic = new();
    static public void Start()
    {
        ShowAll();
        MenuHelpers.Confirm("Enter 1 to return to Admin Menu\nEnter 2 to order products");
        string? input = MenuHelpers.Prompt("");
        if (input == "1")
        {
            AdminMenu.Start();
            return;
        }
        else if (input == "2")
        {
            OrderProducts();
            AdminMenu.Start();
        }
        else
        {
            MenuHelpers.Warn("Invalid input");
            Start();
        }
    }

    public static void OrderProducts()
    {
        string? orderInput = MenuHelpers.Prompt("Please enter the Name or Id of the product you'd like to order");
        if (string.IsNullOrWhiteSpace(orderInput))
        {
            MenuHelpers.Warn("No product given");
            return;
        }

        int inputAmount = MenuHelpers.PromptInt("Please enter the amount you'd like to order");

        int orderInputId;
        bool inputIsId = int.TryParse(orderInput, out orderInputId);

        if (inputIsId)
        {
            productLogic.OrderProductByID(orderInputId, inputAmount);
            MenuHelpers.Announce("Your order has been placed!");
            return;
        }

        bool ordered = productLogic.OrderProductByName(orderInput, inputAmount);
        if (ordered)
        {
            MenuHelpers.Announce("Your order has been placed!");
        }
        else
        {
            MenuHelpers.Warn("Product not found, no order placed");
        }
    }

    public static void ShowAll()
    {
        Display.List(
            productLogic.GetProducts(),
            item => $"ID: {item.ProductID}| Name: {item.Name}| Stock: {(item.Stock > 0 ? item.Stock.ToString() : "OUT OF STOCK")}",
            "--- ALL PRODUCTS ---");
    }
}
