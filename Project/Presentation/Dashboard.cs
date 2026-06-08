using Project.Logic;
using Project.Models;
using Project;

static class Dashboard
{
    private static readonly ProductLogic ProductLogic = new();
    private static readonly OfferLogic OfferLogic = new();
    private static readonly AccountsLogic AccountsLogic = new();
    public static readonly ShoppingCartLogic ShoppingCart = new();
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
                "Add a product to shopping cart",
                "View shopping cart and total",
                "Clear shopping cart"
            };

            if (account == null)
            {
                options.Add("Checkout and pay");
                options.Add("Show product details");
                options.Add("Login");
            }
            else
            {
                options.Add("Checkout and pay");
                options.Add("Wishlist");
                options.Add("Show purchase history");
                options.Add("Show product details");
                // options.Add("Two-Factor Authentication (2FA) Settings");
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
                        AddProductToShoppingCart();
                        break;
                    case 4:
                        ShowShoppingCart();
                        break;
                    case 5:
                        ShoppingCart.GetAllItems().Clear();
                        MenuHelpers.Confirm("Shopping cart cleared");
                        WaitForContinue();
                        break;
                    case 6:
                        PurchaseShoppingCart.PurchaseChoice();
                        break;
                    case 7:
                        ProductDetailsView.Start();
                        break;
                    case 8:
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
                        AddProductToShoppingCart();
                        break;
                    case 4:
                        ShowShoppingCart();
                        break;
                    case 5:
                        ShoppingCart.GetAllItems().Clear();
                        MenuHelpers.Confirm("Shopping cart cleared");
                        WaitForContinue();
                        break;
                    case 6:
                        PurchaseShoppingCart.PurchaseChoice();
                        break;
                    case 7:
                        ShowWishlist.Start();
                        break;
                    case 8:
                        ShowPurchaseHistory();
                        break;
                    // case 9:
                    //     ManageTwoFactor();
                    //     break;
                    case 9:
                        ProductDetailsView.Start();
                        break;
                    case 10:
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

    private static void AddProductToShoppingCart()
    {
        Console.Clear();
        var products = ProductLogic.GetProducts();

        if (products == null || !products.Any())
        {
            MenuHelpers.Warn("No products available");
            WaitForContinue();
            return;
        }

        List<string> options = new List<string>();
        foreach (var item in products)
        {
            options.Add($"Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR     [Product details →]");
        }
        options.Add("Back to Dashboard");

        MenuNavigation menu = new MenuNavigation(options, "--- SELECT A PRODUCT TO ADD (Right arrow: details) ---");
        menu.OnRightArrow = idx =>
        {
            if (idx >= 0 && idx < products.Count)
            {
                ProductDetailsView.Show(products[idx]);
            }
        };
        int selection = menu.Start();

        if (selection == options.Count - 1)
        {
            return;
        }

        ProductModel selectedProduct = products[selection];

        if (selectedProduct.Stock <= 0)
        {
            MenuHelpers.Warn("Selected product is out of stock");
            WaitForContinue();
            return;
        }

        var cartItem = new ShoppingCartItem(selectedProduct, 1, selectedProduct.Price);
        ShoppingCart.AddItem(cartItem);

        MenuHelpers.Confirm($"Added {selectedProduct.Name} to shopping cart");
        WaitForContinue();
    }

    private static void ShowShoppingCart()
{
    Console.Clear();
    MenuHelpers.Announce("--- YOUR SHOPPING CART ---");

    var items = ShoppingCart.GetAllItems();
    if (items.Count == 0)
    {
        MenuHelpers.Warn("Shopping CART is empty");
        WaitForContinue();
        return;
    }

    decimal total = 0;
    for (int i = 0; i < items.Count; i++)
    {
        var item = items[i];
        decimal lineTotal = (decimal)item.Product.Price * item.Quantity;
        total += lineTotal;

        MenuHelpers.Confirm($"{i + 1}. {item.Product.Name} | Category: {item.Product.Category} | Brand: {item.Product.Brand} | Qty: {item.Quantity} | Price: {item.Product.Price} EUR (Total: {lineTotal} EUR)");
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
║  │ Deli  │ │ Dry Food │ │rage │ │ And     │         ║
║  │       │ │          │ │     │ │ Goods   │         ║
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

    // private static void ManageTwoFactor()
    // {
    //     Console.Clear();
    //     AccountModel? account = AccountsLogic.CurrentAccount;
    //     if (account == null)
    //     {
    //         return;
    //     }

    //     TwoFactorService tfaService = new();
    //     bool isCurrentlyEnabled = !string.IsNullOrWhiteSpace(account.TwoFactorSecret);

    //     List<string> options = new List<string>();
    //     string title = "--- TWO-FACTOR AUTHENTICATION ---";

    //     if (!isCurrentlyEnabled)
    //     {
    //         options.Add("Setup / Enable Google Authenticator");
    //     }
    //     else
    //     {
    //         options.Add("Disable Google Authenticator");
    //     }
    //     options.Add("Back to Dashboard");

    //     MenuNavigation subMenu = new MenuNavigation(options, title);
    //     int selection = subMenu.Start();

    //     if (selection == options.Count - 1)
    //     {
    //         return;
    //     }

    //     if (!isCurrentlyEnabled && selection == 0)
    //     {
    //         Console.Clear();
    //         var setup = tfaService.EnableTwoFactor(account.EmailAddress);

    //         MenuHelpers.Announce("1. Install Google Authenticator on your mobile device.");
    //         MenuHelpers.Announce("2. Add an account manually using this shared secret key:");
    //         MenuHelpers.Confirm($"   Key: {setup.Base32Secret}");
    //         Console.WriteLine();

    //         string verificationCode = MenuHelpers.Prompt("Enter the 6-digit code shown on your app to verify setup") ?? string.Empty;

    //         if (tfaService.VerifyToken(setup.Base32Secret, verificationCode.Trim()))
    //         {
    //             account.TwoFactorSecret = setup.Base32Secret;
    //             AccountsLogic.UpdateAccount(account);
    //             MenuHelpers.Confirm("Two-Factor Authentication successfully enabled!");
    //         }
    //         else
    //         {
    //             MenuHelpers.Error("Verification failed. Invalid code. 2FA configuration aborted.");
    //         }
    //         WaitForContinue();
    //     }
    //     else if (isCurrentlyEnabled && selection == 0)
    //     {
    //         Console.Clear();
    //         string verificationCode = MenuHelpers.Prompt("Enter your current 6-digit code to confirm removal") ?? string.Empty;

    //         if (tfaService.VerifyToken(account.TwoFactorSecret, verificationCode.Trim()))
    //         {
    //             account.TwoFactorSecret = string.Empty;
    //             AccountsLogic.UpdateAccount(account);
    //             MenuHelpers.Confirm("Two-Factor Authentication has been completely disabled.");
    //         }
    //         else
    //         {
    //             MenuHelpers.Error("Verification failed. Incorrect code. 2FA remains active.");
    //         }
    //         WaitForContinue();
    //     }
    // }
}