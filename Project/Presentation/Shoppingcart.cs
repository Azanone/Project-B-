using Project.Logic;
 
public class PurchaseShoppingCart
{
 
 
    static private ShoppingCartLogic shoppingCartLogic = new();
   
    public static decimal ShowShoppingCart()
    {
        Console.Clear();
        var account = AccountsLogic.CurrentAccount;
 
        if (account == null)
        {
            MenuHelpers.Warn("You must be logged in.");
            MenuHelpers.Pause();
            return 0;
        }
 
        MenuHelpers.Announce("--- YOUR SHOPPING LIST ---");
 
        var items = shoppingCartLogic.GetAllItems();
 
        if (items.Count == 0)
        {
            MenuHelpers.Warn("Shopping list is empty");
            MenuHelpers.Pause();
            return 0;
        }
 
        decimal total = 0;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            total += item.Product.Price * item.Quantity;
 
            MenuHelpers.Confirm(
                $"{i + 1}. {item.Product.Name} | " +
                $"Category: {item.Product.Category} | " +
                $"Brand: {item.Product.Brand} | " +
                $"Qty: {item.Quantity} | " +
                $"Price: {item.Product.Price} EUR"
            );
        }
 
        MenuHelpers.Announce($"Total: {total} EUR");
        MenuHelpers.Pause();
        return total;
    }
 
    public static void PurchaseChoice()
    {
 
        bool validinput = false;
        while (!validinput)
        {
            ShowShoppingCart();
            MenuHelpers.Confirm($"\nEnter 1 to return\nEnter 2 to continue to checkout");
            string? userChoiceInput = Console.ReadLine();
            string userChoice = userChoiceInput ?? string.Empty;
            if(userChoice  == "1")
            {
                validinput = true;
            }
            else if(userChoice == "2")
            {
                validinput = true;
                bool purchaseSucces = shoppingCartLogic.CompletePurchase(ReceiptLogic.GetCurrentUserId());//Belasting word nog niet gerekend bij de transactie, het is niet te zien op de bon
                if (purchaseSucces)
                    MenuHelpers.Announce("Thank you for your purchase!");
                else
                    MenuHelpers.Warn("Purchase failed, your shopping cart is empty.");
            }
            else
            {
                MenuHelpers.Warn("Invalid Input");
            }
        }
    }
}