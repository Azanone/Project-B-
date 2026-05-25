using Project.Logic;
using Project.Models;

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

                int userId = ReceiptLogic.GetCurrentUserId();

                var ageCheck = shoppingCartLogic.CheckAgeRestriction();
                if (!ageCheck.Allowed)
                {
                    MenuHelpers.Warn($"Age restriction: you must be at least {ageCheck.RequiredAge} years old to buy '{ageCheck.ProductName}'. Purchase blocked.");
                    MenuHelpers.Pause();
                    return;
                }

                var cartItems = shoppingCartLogic.GetAllItems();
                if (cartItems.Count == 0)
                {
                    MenuHelpers.Warn("Purchase failed, your shopping cart is empty.");
                    return;
                }
                decimal total = cartItems.Sum(i => i.Product.Price * i.Quantity);

                string? paymentMethod = PaymentCheckout.Start(total);
                if (paymentMethod == null)
                {
                    MenuHelpers.Warn("Checkout cancelled, no payment made.");
                    return;
                }

                bool purchaseSucces = shoppingCartLogic.CompletePurchase(userId, paymentMethod);
                if (purchaseSucces)
                    PrintReceipt(cartItems, total, paymentMethod);
                else
                    MenuHelpers.Warn("Purchase failed, your shopping cart is empty.");
            }
            else
            {
                MenuHelpers.Warn("Invalid Input");
            }
        }
    }

    private static void PrintReceipt(List<ShoppingCartItem> items, decimal subtotal, string paymentMethod)
    {
        decimal vat = subtotal * 0.21m;

        Console.Clear();
        Console.WriteLine(@"  ___________________________________________");
        Console.WriteLine(@" /                                           \");
        Console.WriteLine(@"|   *************************************** |");
        Console.WriteLine(@"\_  * BabylonMarkt                        * |");
        Console.WriteLine(@"  | *************************************** |");
        Console.WriteLine($"  |  Date: {DateTime.Now:dd-MM-yyyy}                       |");
        Console.WriteLine($"  |  Paid with: {paymentMethod,-25}   |");
        Console.WriteLine(@"  | --------------------------------------- |");
        Console.WriteLine($"  |  {"ITEM",-27}{"PRICE",-11} |");
        Console.WriteLine(@"  | ---------------------------------------  |");

        foreach (ShoppingCartItem item in items)
        {
            string name = item.Product.Name.Length > 25 ? item.Product.Name.Substring(0, 25) : item.Product.Name;
            string qtyLabel = item.Quantity > 1 ? $"x{item.Quantity} " : "";
            decimal lineTotal = item.Product.Price * item.Quantity;
            Console.WriteLine($"  |  {name,-27}{qtyLabel + lineTotal.ToString("F2") + " EUR",-11}  |");
        }

        Console.WriteLine(@"  | ---------------------------------------  |");
        Console.WriteLine($"  |  {"SUBTOTAL:",-27}{subtotal.ToString("F2") + " EUR",-11}  |");
        Console.WriteLine($"  |  {"TAX (VAT):",-27}{vat.ToString("F2") + " EUR",-11}  |");
        Console.WriteLine(@"  | ---------------------------------------  |");
        Console.WriteLine($"  |  {"TOTAL:",-27}{(subtotal + vat).ToString("F2") + " EUR",-11}  |");
        Console.WriteLine(@"  | ---------------------------------------  |");
        Console.WriteLine(@"  |         THANK YOU FOR VISITING!           |");
        Console.WriteLine(@"  | __________________________________________|___");
        Console.WriteLine(@"  | /                                            /");
        Console.WriteLine(@"  \_/___________________________________________/");

        MenuHelpers.Pause();
    }
}