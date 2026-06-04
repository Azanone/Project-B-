using System;
using System.Collections.Generic;
using System.Linq;
using Project.Logic;
using Project.Models;

public class PurchaseShoppingCart
{
    private static ShoppingCartLogic shoppingCartLogic = new();
   
    public static decimal ShowShoppingCart()
    {
        Console.Clear();
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
        ShowShoppingCart();
        var account = AccountsLogic.CurrentAccount;

        if (account != null)
        {
            var ageCheck = shoppingCartLogic.CheckAgeRestriction();
            if (!ageCheck.Allowed)
            {
                MenuHelpers.Warn($"Age restriction: you must be at least {ageCheck.RequiredAge} years old to buy '{ageCheck.ProductName}'. Purchase blocked.");
                MenuHelpers.Pause();
                return;
            }
        }
        else
        {
            var items = shoppingCartLogic.GetAllItems();
            bool hasAgeRestrictedItem = items.Any(i => i.Product.MinAge > 0);
            
            if (hasAgeRestrictedItem)
            {
                string input = MenuHelpers.Prompt("Your cart contains age-restricted items. Are you 18 or older? (yes/no):") ?? string.Empty;
                if (!input.Trim().Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    MenuHelpers.Warn("Purchase blocked due to age restriction policies.");
                    MenuHelpers.Pause();
                    return;
                }
            }
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

        int userId = account != null ? (int)account.Id : -1;

        bool purchaseSuccess = shoppingCartLogic.CompletePurchase(userId, paymentMethod);
        if (purchaseSuccess)
        {
            PrintReceipt(cartItems, total, paymentMethod);
        }
        else
        {
            MenuHelpers.Warn("Purchase failed, transaction processing error.");
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