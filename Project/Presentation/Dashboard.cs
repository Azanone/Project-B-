using Project.Logic;
using Project.Models;

static class Dashboard
{
    private static readonly ProductLogic ProductLogic = new();
    private static readonly OfferLogic OfferLogic = new();
    private static readonly AccountsLogic AccountsLogic = new();
    public static readonly ShoppingCartLogic ShoppingCart = new();
    private static readonly ShoppingCartLogic Wishlist = new();
    private static readonly ReceiptLogic ReceiptLogic = new();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            AccountModel? account = AccountsLogic.CurrentAccount;

            if (account == null)
            {
                MenuHelpers.Announce("Welcome guest");
            }
            else
            {
                MenuHelpers.Announce("Welcome back " + account.FullName);
            }
            
            MenuHelpers.Confirm("Enter 1 to see all products");
            MenuHelpers.Confirm("Enter 2 to see all offers");
            MenuHelpers.Confirm("Enter 3 to see store layout");
            MenuHelpers.Confirm("Enter 4 to add a product to shopping list");
            MenuHelpers.Confirm("Enter 5 to view shopping list and total");
            MenuHelpers.Confirm("Enter 6 to clear shopping cart");
            MenuHelpers.Confirm("Enter 7 to Wishlist");
            MenuHelpers.Confirm("Enter 8 to show purchase history");
            MenuHelpers.Confirm("Enter 9 to complete puchase");
            MenuHelpers.Confirm("Enter 10 to logout");

            string input = MenuHelpers.Prompt("Choose an option") ?? string.Empty;
            if (input == "1")
            {
                ShowProducts();
            }
            else if (input == "2")
            {
                ShowOffers();
            }
            else if (input == "3")
            {
                ShowLayout();
            }
            else if (input == "4")
            {
                AddProductToShoppingCart();
            }
            else if (input == "5")
            {
                ShowShoppingCart();
                WaitForContinue();
            }
            else if (input == "6")
            {
                if (account == null)
                {
                    MenuHelpers.Warn("You must be logged in.");
                    WaitForContinue();
                    continue;
                }

                ShoppingCart.ClearCurrentCart();
                MenuHelpers.Confirm("Shopping list cleared");
                WaitForContinue();
            }
            else if (input == "7")
            {
                ShowWishlist.Start();
            }
            else if (input == "8")
            {
                ShowPurchaseHistory();
            }
            else if (input == "9")
            {
                PurchaseShoppingCart.PurchaseChoice();
                WaitForContinue();
            }
            else if (input == "10")
            {
                AccountsLogic.Logout();
                Menu.Start();
                return;
            }
            else
            {
                MenuHelpers.Warn("Invalid input");
                WaitForContinue();
            }
        }
    }

    private static void ShowProducts()
    {
        Console.Clear();
        var list = ProductLogic.GetProducts();
        AccountModel? account = AccountsLogic.CurrentAccount;
        MenuHelpers.Announce("--- ALL PRODUCTS ---");
        foreach (var item in list)
        {
            string ageLabel = item.MinAge > 0 ? $" | Age: {item.MinAge}+" : "";
            MenuHelpers.Confirm($"ID: {item.ProductID} | Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR{ageLabel}");
        }
        WaitForContinue();
    }

    private static void ShowOffers()
    {
        Console.Clear();
        var list = OfferLogic.GetOffers();
        MenuHelpers.Announce("--- ALL OFFERS ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.OfferID} | Description: {item.Description} | Begin: {item.StartDate} | End: {item.EndDate} | Price: {item.RegularPrice} EUR | Discount: {item.DiscountPercentage}% | Discount-price: {item.DiscountPrice} EUR");
        }
        WaitForContinue();
    }

    private static void AddProductToShoppingCart()
    {
        Console.Clear();

        var account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            MenuHelpers.Warn("You must be logged in.");
            WaitForContinue();
            return;
        }

        var products = ProductLogic.GetProducts();
        MenuHelpers.Announce("--- ADD PRODUCT TO SHOPPING LIST ---");

        foreach (var item in products)
        {
            string ageLabel = item.MinAge > 0 ? $" | Age: {item.MinAge}+" : "";
            MenuHelpers.Confirm($"ID: {item.ProductID} | Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR{ageLabel}");
        }

        string rawId = MenuHelpers.Prompt("Enter product ID") ?? string.Empty;

        if (!int.TryParse(rawId, out int productId))
        {
            MenuHelpers.Warn("Invalid product ID");
            WaitForContinue();
            return;
        }

        ProductModel? selectedProduct =
            products.FirstOrDefault(p => p.ProductID == productId);

        if (selectedProduct == null)
        {
            MenuHelpers.Warn("Product not found");
            WaitForContinue();
            return;
        }

        if (selectedProduct.MinAge > 0 && account != null && !ProductLogic.IsOldEnoughForProduct(selectedProduct, account.Age))
        {
            MenuHelpers.Warn($"You must be {selectedProduct.MinAge}+ to purchase {selectedProduct.Name}");
            WaitForContinue();
            return;
        }

        if (selectedProduct.Stock <= 0)
        {
            MenuHelpers.Warn("Selected product is out of stock");
            WaitForContinue();
            return;
        }

        var cartItem = new ShoppingCartItem(selectedProduct, 1);
        ShoppingCart.AddItem(cartItem);

        MenuHelpers.Confirm($"Added {selectedProduct.Name} to shopping list");

        WaitForContinue();
    }
    
    private static void RemoveItemFromCart()
    {
        Console.Clear();

        var account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            MenuHelpers.Warn("Account not found");
            WaitForContinue();
            return;
        }

        var cartItems = ShoppingCart.GetAllItems();

        if (!cartItems.Any())
        {
            MenuHelpers.Warn("Cart is empty");
            WaitForContinue();
            return;
        }

        MenuHelpers.Announce("--- REMOVE PRODUCT FROM SHOPPING LIST ---");

        foreach (var item in cartItems)
        {
            MenuHelpers.Confirm(
                $"ID: {item.CartItemId} | {item.Product.Name} | Qty: {item.Quantity}"
            );
        }

        string rawId = MenuHelpers.Prompt("Enter item ID") ?? "";

        if (!int.TryParse(rawId, out int cartItemId))
        {
            MenuHelpers.Warn("Invalid ID");
            WaitForContinue();
            return;
        }

        var itemToRemove = cartItems.FirstOrDefault(item => item.CartItemId == cartItemId);
        if (itemToRemove == null)
        {
            MenuHelpers.Warn("Item not found");
            WaitForContinue();
            return;
        }

        ShoppingCart.RemoveItem(itemToRemove);

        MenuHelpers.Confirm("Item removed");
        WaitForContinue();
    }

    private static void ShowShoppingCart()
    {
        Console.Clear();
        var account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            MenuHelpers.Warn("You must be logged in.");
            //WaitForContinue();
            return;
        }

        MenuHelpers.Announce("--- YOUR SHOPPING LIST ---");

        var items = ShoppingCart.GetAllItems();

        if (items.Count == 0)
        {
            MenuHelpers.Warn("Shopping list is empty");
           // WaitForContinue();
            return;
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
        //WaitForContinue();
    }

    private static void ShowLayout()
    {
        Console.Clear();
        MenuHelpers.Confirm(@"╔══════════════╦══════════════════╦═══════════════════╗
║              ║                  ║                   ║
║   BAKERY     ║     DAIRY        ║     FROZEN        ║
║              ║                  ║                   ║
╠══════════════╩══════════════════╣                   ║
║                                 ╚═══════════════════╣
║  ┌───────┐ ┌──────────┐ ┌─────┐ ┌─────────┐        ║
║  │       │ │ Canned & │ │Beve-│ │ Snacks  │        ║
║  │ Deli  │ │ Dry Food │ │rage │ │  And    │        ║
║  │       │ │          │ │     │ │  Goods  │        ║
║  └───────┘ └──────────┘ └─────┘ └─────────┘        ║
╠════════════════════╦════════════════════════════════╣
║                    ║                                ║
║   FRESH PRODUCE    ║    CASHOUT /                   ║
║                    ║    CUSTOMER SERVICE            ║
║                    ║                                ║
╚════════════════════╝        ↑           ╚═══════════╝
                        ENTRANCE / EXIT");
        WaitForContinue();
    }

    public static void WaitForContinue()
    {
        MenuHelpers.Prompt("Press Enter to continue");
    }
    
    private static void ShowPurchaseHistory()
    {
        Console.Clear();
        AccountModel? account = AccountsLogic.CurrentAccount;
        if (account == null)
        {
            MenuHelpers.Warn("You must be logged in to view purchase history");
            WaitForContinue();
            return;
        }

        var receipts = ReceiptLogic.GetPurchasesByAccountID(account.UserId);
        MenuHelpers.Announce("--- YOUR PURCHASE HISTORY ---");

        if (receipts.Count == 0)
        {
            MenuHelpers.Warn("No purchases found");
            WaitForContinue();
            return;
        }

        var grouped = receipts.GroupBy(r => r.PurchaseID);
        foreach (var group in grouped)
        {
            var first = group.First();
            MenuHelpers.Confirm("----------------------------------------");
            MenuHelpers.Announce($"  Purchase #{group.Key}  |  {first.CreatedAt:dd-MM-yyyy}");
            MenuHelpers.Confirm("----------------------------------------");
            foreach (var item in group)
            {
                string qtyLabel = item.Quantity > 1 ? $"x{item.Quantity} " : "";
                decimal lineTotal = item.ProductPrice * item.Quantity;
                MenuHelpers.Confirm($"  {item.ProductName,-22} {qtyLabel}{lineTotal:F2}");
            }
            MenuHelpers.Confirm("----------------------------------------");
            MenuHelpers.Confirm($"  Total:                    {first.TotalPrice:F2}");
            Console.WriteLine();
        }

        WaitForContinue();
    }
}