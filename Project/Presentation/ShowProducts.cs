using Project.Presentation;

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
        Display.List(
            productLogic.GetProducts(),
            item => $"ID: {item.ProductID}| Name: {item.Name}| Category: {item.Category}| Price: {item.Price} EUR",
            "--- ALL PRODUCTS ---");
            
        MenuHelpers.WriteColor("\nWant to leave a review? [Type 1]", ConsoleColor.Red);
        MenuHelpers.WriteColor("Want to continue Shopping? [Type 2]", ConsoleColor.Red);
        
        while (true)
        {
            string? input = MenuHelpers.Prompt("");
            if (input == "1")
            {
                ProductReviews.Start();
                return;
            }
        }
    }
}