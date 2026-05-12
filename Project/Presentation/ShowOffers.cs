public static class ShowOffers
{
    static private OfferLogic offerLogic = new();
    static public void Start()
    {
        ShowAll();
        MenuHelpers.PromptReturnToMenu("Enter 1 to return to Admin Menu", AdminMenu.Start);
    }

    static public void ShowAll()
    {
        var list = offerLogic.GetOffers();
        MenuHelpers.Announce("--- ALL OFFERS ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.OfferID}| Description: {item.Description}| Begin-Date: {item.StartDate}| End-Date: {item.EndDate}| Price: {item.RegularPrice} EUR |Discount: {item.DiscountPercentage}%| Discount-price: {item.DiscountPrice} EUR ");
        }
    }
}