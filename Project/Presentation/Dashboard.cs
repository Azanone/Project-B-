using Project.Logic;
using Project.Models;

static class Dashboard
{
    private static readonly ProductLogic ProductLogic = new();
    private static readonly OfferLogic OfferLogic = new();
    private static readonly AccountsLogic AccountsLogic = new();
    private static readonly ShoppingListLogic ShoppingCart = new();
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
            MenuHelpers.Confirm("Enter 6 to clear shopping list");
            MenuHelpers.Confirm("Enter 7 to show purchase history");
            MenuHelpers.Confirm("Enter 8 to logout");

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
                AddProductToShoppingList();
            }
            else if (input == "5")
            {
                ShowShoppingList();
            }
            else if (input == "6")
            {
                ShoppingCart.GetAllItems().Clear();
                MenuHelpers.Confirm("Shopping list cleared");
                WaitForContinue();
            }
            else if (input == "7")
            {
                ShowPurchaseHistory();
            }
            else if (input == "8")
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
            if (item.MinAge > 0 && account != null && account.Age < item.MinAge)
            {
                MenuHelpers.Warn($"  ^ You must be {item.MinAge}+ to purchase this product");
            }
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

    private static void AddProductToShoppingList()
    {
        Console.Clear();
        var products = ProductLogic.GetProducts();
        AccountModel? account = AccountsLogic.CurrentAccount;
        MenuHelpers.Announce("--- ADD PRODUCT TO SHOPPING LIST ---");
        foreach (var item in products)
        {
            string ageLabel = item.MinAge > 0 ? $" | Age: {item.MinAge}+" : "";
            MenuHelpers.Confirm($"ID: {item.ProductID} | Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR{ageLabel}");
        }

        string rawId = MenuHelpers.Prompt("Enter product ID") ?? string.Empty;
        if (!long.TryParse(rawId, out long productId))
        {
            MenuHelpers.Warn("Invalid product ID");
            WaitForContinue();
            return;
        }

        ProductModel? selectedProduct = products.FirstOrDefault(p => p.ProductID == productId);
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

        var shoppingItem = new ShoppingListModel(
            selectedProduct.Name,
            selectedProduct.Category,
            selectedProduct.Price,
            selectedProduct.Brand,
            selectedProduct.Ingredients
        );

        var cartItem = new ShoppingCartItem(shoppingItem, 1, (double)selectedProduct.Price);
        ShoppingCart.AddItem(cartItem);

        MenuHelpers.Confirm($"Added {selectedProduct.Name} to shopping list");
        WaitForContinue();
    }

    private static void ShowShoppingList()
    {
        Console.Clear();
        MenuHelpers.Announce("--- YOUR SHOPPING LIST ---");

        var items = ShoppingCart.GetAllItems();
        if (items.Count == 0)
        {
            MenuHelpers.Warn("Shopping list is empty");
            WaitForContinue();
            return;
        }

        decimal total = 0;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            total += (decimal)item.Price * item.Quantity;
            MenuHelpers.Confirm($"{i + 1}. {item.Product.Name} | Category: {item.Product.Category} | Brand: {item.Product.Brand} | Qty: {item.Quantity} | Price: {item.Price} EUR");
        }

        MenuHelpers.Announce($"Total (preview): {total} EUR");
        WaitForContinue();
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

    private static void WaitForContinue()
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

        var receipts = ReceiptLogic.GetPurchasesByAccountID((int)account.Id);
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