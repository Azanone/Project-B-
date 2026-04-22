using Project.Logic;
using Project.Models;

static class Dashboard
{
    private static readonly ProductLogic ProductLogic = new();
    private static readonly OfferLogic OfferLogic = new();
    private static readonly AccountsLogic AccountsLogic = new();
    private static readonly ShoppingCartLogic ShoppingCart = new();

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
            MenuHelpers.Confirm("Enter 4 to add a product to shopping cart");
            MenuHelpers.Confirm("Enter 5 to view shopping cart and total");
            MenuHelpers.Confirm("Enter 6 to clear shopping cart");
            MenuHelpers.Confirm("Enter 7 to logout");

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
            }
            else if (input == "6")
            {
                RemoveItemFromCart();
                MenuHelpers.Confirm("Shopping list cleared");
                WaitForContinue();
            }
            else if (input == "7")
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
        MenuHelpers.Announce("--- ALL PRODUCTS ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.ProductID} | Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR");
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
            MenuHelpers.Confirm(
                $"ID: {item.ProductID} | Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR"
            );
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

        if (selectedProduct.Stock <= 0)
        {
            MenuHelpers.Warn("Selected product is out of stock");
            WaitForContinue();
            return;
        }
        
        ShoppingCart.AddItem(account.UserId, selectedProduct.ProductID, 1);

        MenuHelpers.Confirm($"Added {selectedProduct.Name} to shopping cart");

        WaitForContinue();
    }
    
    private static void RemoveItemFromCart()
    {
        Console.Clear();

        var account = AccountsLogic.CurrentAccount;
        var products = ProductLogic.GetProducts();

        if (account == null)
        {
            MenuHelpers.Warn("Account not found");
            WaitForContinue();
            return;
        }

        MenuHelpers.Announce("--- REMOVE PRODUCT FROM SHOPPING LIST ---");

        foreach (var item in products)
        {
            MenuHelpers.Confirm($"ID: {item.ProductID} | Name: {item.Name} | Price: {item.Price} EUR");
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

        ShoppingCart.RemoveItem(
            new ShoppingCartItem(selectedProduct, 1),
            account.UserId
        );

        MenuHelpers.Confirm($"Removed {selectedProduct.Name} from cart");
        WaitForContinue();
    }

    private static void ShowShoppingCart()
    {
        Console.Clear();
        MenuHelpers.Announce("--- YOUR SHOPPING LIST ---");

        var account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            MenuHelpers.Warn("You must be logged in.");
            WaitForContinue();
            return;
        }

        var items = ShoppingCart.GetAllItems(account.UserId);

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

            total += item.Product.Price;

            MenuHelpers.Confirm(
                $"{i + 1}. {item.Product.Name} | " +
                $"Category: {item.Product.Category} | " +
                $"Brand: {item.Product.Brand} | " +
                $"Price: {item.Product.Price} EUR | " +
                $"Qty: {item.Quantity}"
            );
        }

        MenuHelpers.Announce($"Total: {total} EUR");
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
}