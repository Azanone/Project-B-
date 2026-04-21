public static class ShowStock
{
    static private ProductLogic productLogic = new();
    static public void Start()
    {
        ShowAll();
        Console.WriteLine("Enter 1 to return to Admin Menu\nEnter 2 to order products");
        string input = Console.ReadLine();
        if (input == "1")
        {
            AdminMenu.Start();
        }
        if (input == "2")
        {
            int orderinputId;
            Console.WriteLine("Please enter the Name OR Id or the product you'd like to order");//handelt nog geen invalid inputs
            string orderinputName = Console.ReadLine();
            if (int.TryParse(orderinputName,out orderinputId))
            {
            Console.WriteLine("Please enter the the amount you'd like to order");
            int inputAmount = Convert.ToInt16(Console.ReadLine());
            OrderProductByID(orderinputId,inputAmount);
            Console.WriteLine("Your order has been placed!");
            }
            else
            {
            Console.WriteLine("Please enter the the amount you'd like to order");//Handelt nog geen invalid inputs. Maybe current stock laten zien bij deze stap maar dan moet je eerstgetproducts() weer roepen volgensmij
            int inputAmount = Convert.ToInt16(Console.ReadLine());
            OrderProductByName(orderinputName,inputAmount);
            Console.WriteLine("Your order has been placed!");
            }
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
            Console.WriteLine($"ID: {item.ProductID}| Name: {item.Name}| Stock: {(item.Stock > 0 ? item.Stock.ToString() : "OUT OF STOCK")}");
        }
    }

    public static void OrderProductByID(int productID, int orderAmount)
    {
        productLogic.OrderProductByID(productID,orderAmount);
    }
    public static void OrderProductByName(string productName, int orderAmount)
    {
        productLogic.OrderProductByName(productName,orderAmount); //Deze code is niet geschreven om foute input te catchen/handelen, also misschien als het aantal dat bestelt word te hoog is vragen of de user zeker is?
    }
}