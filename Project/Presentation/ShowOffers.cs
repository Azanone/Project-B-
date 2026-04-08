public static class ShowOffers
{
    static private OfferLogic offerLogic = new();
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

    static public void ShowAll()
    {
        var list = offerLogic.GetOffers();
        Console.WriteLine("--- ALL OFFERS ---");
        foreach (var item in list)
        {
            Console.WriteLine($"ID: {item.OfferID}| Description: {item.Description}| Begin-Date: {item.StartDate}| End-Date: {item.EndDate}| Price: {item.RegularPrice} EUR |Discount: {item.DiscountPercentage}%| Discount-price: {item.DiscountPrice} EUR ");
        }
    }
}