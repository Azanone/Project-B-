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
        string userGreeting = "Welcome guest";
        if (account != null)
        {
            userGreeting = "Welcome back " + account.FullName;
        }

        List<string> options = new List<string>
        {
            "See all products",
            "See all offers",
            "See store layout",
            "Add a product to shopping list",
            "View shopping list and total",
            "Clear shopping cart"
        };

        if (account == null)
        {
            options.Add("Login");
        }
        else
        {
            options.Add("Wishlist");
            options.Add("Show purchase history");
            options.Add("Logout");
        }

        MenuNavigation menu = new MenuNavigation(options, userGreeting);
        int selection = menu.Start();

        if (account == null)
        {
            switch (selection)
            {
                case 0:
                    ShowProducts();
                    break;
                case 1:
                    ShowOffers();
                    break;
                case 2:
                    ShowLayout();
                    break;
                case 3:
                    AddProductToShoppingList();
                    break;
                case 4:
                    ShowShoppingList();
                    break;
                case 5:
                    ShoppingCart.GetAllItems().Clear();
                    MenuHelpers.Confirm("Shopping list cleared");
                    WaitForContinue();
                    break;
                case 6:
                    Menu.Start();
                    return;
            }
        }
        else
        {
            switch (selection)
            {
                case 0:
                    ShowProducts();
                    break;
                case 1:
                    ShowOffers();
                    break;
                case 2:
                    ShowLayout();
                    break;
                case 3:
                    AddProductToShoppingList();
                    break;
                case 4:
                    ShowShoppingList();
                    break;
                case 5:
                    ShoppingCart.GetAllItems().Clear();
                    MenuHelpers.Confirm("Shopping list cleared");
                    WaitForContinue();
                    break;
                case 6:
                    ShowWishlist.Start();
                    break;
                case 7:
                    ShowPurchaseHistory();
                    break;
                case 8:
                    AccountsLogic.Logout();
                    Menu.Start();
                    return;
            }
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
            MenuHelpers.Confirm($"Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR");
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
        MenuHelpers.Announce("--- ADD PRODUCT TO SHOPPING LIST ---");
        foreach (var item in products)
        {
            MenuHelpers.Confirm($"ID: {item.ProductID} | Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR");
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

        if (selectedProduct.Stock <= 0)
        {
            MenuHelpers.Warn("Selected product is out of stock");
            WaitForContinue();
            return;
        }

        var cartItem = new ShoppingCartItem(selectedProduct, 1, selectedProduct.Price);
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
║   BAKERY     ║    DAIRY         ║     FROZEN        ║
║              ║                  ║                   ║
╠══════════════╩══════════════════╣                   ║
║                                 ╚═══════════════════╣
║  ┌───────┐ ┌──────────┐ ┌─────┐ ┌_________┐         ║
║  │       │ │ Canned & │ │Beve-│ │ Snacks  │         ║
║  │ Deli  │ │ Dry Food │ │rage │ │  And    │         ║
║  │       │ │          │ │     │ │  Goods  │         ║
║  └───────┘ └──────────┘ └─────┘ └_________┘         ║
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