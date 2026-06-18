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
            decimal unitPrice = shoppingCartLogic.GetEffectivePrice(item.Product);
            total += unitPrice * item.Quantity;

            MenuHelpers.Line(
                $"{i + 1}. {item.Product.Name} | " +
                $"Category: {item.Product.Category} | " +
                $"Brand: {item.Product.Brand} | " +
                $"Qty: {item.Quantity} | " +
                $"Price: {unitPrice:0.00} EUR"
            );
        }

        MenuHelpers.Announce($"Total: {total:0.00} EUR");
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
        
        decimal total = cartItems.Sum(i => shoppingCartLogic.GetEffectivePrice(i.Product) * i.Quantity);

        string? paymentMethod = PaymentCheckout.Start(total);
        if (paymentMethod == null)
        {
            MenuHelpers.Warn("Checkout cancelled, no payment made.");
            return;
        }

        int userId = account != null ? (int)account.Id : -1;

        bool purchaseSuccess = shoppingCartLogic.CompletePurchase(userId);
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
        MenuHelpers.Line(@"  ___________________________________________");
        MenuHelpers.Line(@" /                                           \");
        MenuHelpers.Line(@"|   *************************************** |");
        MenuHelpers.Line(@"\_  * BabylonMarkt                        * |");
        MenuHelpers.Line(@"  | *************************************** |");
        MenuHelpers.Line($"  |  Date: {DateTime.Now:dd-MM-yyyy}                       |");
        MenuHelpers.Line($"  |  Paid with: {paymentMethod,-25}   |");
        MenuHelpers.Line(@"  | --------------------------------------- |");
        MenuHelpers.Line($"  |  {"ITEM",-27}{"PRICE",-11} |");
        MenuHelpers.Line(@"  | ---------------------------------------  |");

        foreach (ShoppingCartItem item in items)
        {
            string name = item.Product.Name.Length > 25 ? item.Product.Name.Substring(0, 25) : item.Product.Name;
            string qtyLabel = item.Quantity > 1 ? $"x{item.Quantity} " : "";
            decimal lineTotal = shoppingCartLogic.GetEffectivePrice(item.Product) * item.Quantity;
            MenuHelpers.Line($"  |  {name,-27}{qtyLabel + lineTotal.ToString("F2") + " EUR",-11}  |");
        }

        MenuHelpers.Line(@"  | ---------------------------------------  |");
        MenuHelpers.Line($"  |  {"SUBTOTAL:",-27}{subtotal.ToString("F2") + " EUR",-11}  |");
        MenuHelpers.Line($"  |  {"TAX (VAT):",-27}{vat.ToString("F2") + " EUR",-11}  |");
        MenuHelpers.Line(@"  | ---------------------------------------  |");
        MenuHelpers.Line($"  |  {"TOTAL:",-27}{(subtotal + vat).ToString("F2") + " EUR",-11}  |");
        MenuHelpers.Line(@"  | ---------------------------------------  |");
        MenuHelpers.Line(@"  |         THANK YOU FOR VISITING!           |");
        MenuHelpers.Line(@"  | __________________________________________|___");
        MenuHelpers.Line(@"  | /                                            /");
        MenuHelpers.Line(@"  \_/___________________________________________/");

        MenuHelpers.Pause();
    }
}