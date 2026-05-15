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
        }
        if (input == "2")
        {
            int orderinputId;
            string? orderinputName = MenuHelpers.Prompt("Please enter the Name OR Id or the product you'd like to order");
            if (int.TryParse(orderinputName, out orderinputId))
            {
                int inputAmount = MenuHelpers.PromptInt("Please enter the the amount you'd like to order");
                OrderProductByID(orderinputId, inputAmount);
                MenuHelpers.Announce("Your order has been placed!");
            }
            else
            {
                int inputAmount = MenuHelpers.PromptInt("Please enter the the amount you'd like to order");
                OrderProductByName(orderinputName, inputAmount);
                MenuHelpers.Announce("Your order has been placed!");
            }
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
        Display.List(
            productLogic.GetProducts(),
            item => $"ID: {item.ProductID}| Name: {item.Name}| Stock: {(item.Stock > 0 ? item.Stock.ToString() : "OUT OF STOCK")}",
            "--- ALL PRODUCTS ---");
    }

    public static void OrderProductByID(int productID, int orderAmount)
    {
        productLogic.OrderProductByID(productID, orderAmount);
    }
    public static void OrderProductByName(string productName, int orderAmount)
    {
        productLogic.OrderProductByName(productName, orderAmount);
    }
}
