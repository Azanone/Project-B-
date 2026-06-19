using Project.Logic;
using Project.Models;
using Project.Presentation;

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
                "Products",
                "Offers",
                "Layout",
                "View cart",
                "Profile"
            };

            MenuNavigation menu = new MenuNavigation(options, userGreeting);
            int selection = menu.Start();

            switch (selection)
            {
                case 0:
                    ShowProductsMenu();
                    break;
                case 1:
                    ShowOffers();
                    break;
                case 2:
                    ShowLayout();
                    break;
                case 3:
                    ShowShoppingCart();
                    break;
                case 4:
                    ShowProfileMenu();
                    break;
            }
        }
    }

    private static void ShowOffers()
    {
        Console.Clear();
        var list = OfferLogic.GetOffers();
        MenuHelpers.Announce("--- ALL OFFERS ---");
        foreach (var item in list)
        {
            MenuHelpers.Line($"{item.Description} | Valid: {item.StartDate:dd-MM-yyyy} -> {item.EndDate:dd-MM-yyyy} | Price: {item.RegularPrice} EUR | Discount: {item.DiscountPercentage}% | Now: {item.DiscountPrice} EUR");
        }
        WaitForContinue();
    }

    private static void ShowProductsMenu()
    {
        while (true)
        {
            Console.Clear();
            List<string> options = new List<string>
            {
                "Browse all products",
                "Add product to shopping cart",
                "Show product details",
                "Leave a review",
                "Back to Dashboard"
            };

            MenuNavigation menu = new MenuNavigation(options, "--- PRODUCTS ---");
            int selection = menu.Start();

            switch (selection)
            {
                case 0:
                    ShowProducts();
                    break;
                case 1:
                    AddProductToShoppingCart();
                    break;
                case 2:
                    ProductDetailsView.Start();
                    break;
                case 3:
                    ProductReviews.Start();
                    break;
                case 4:
                    return;
            }
        }
    }

    private static void ShowProducts()
    {
        while (true)
        {
            Console.Clear();
            var categoryFilter = PromptProductCategoryFilter();
            if (categoryFilter.Cancelled)
            {
                return;
            }

            while (true)
            {
                Console.Clear();
                var list = ProductLogic.GetProductsByPopularity(categoryFilter.CategoryId);

                MenuHelpers.Announce("--- ALL PRODUCTS ---");
                MenuHelpers.Line($"Active sort: Popularity (most purchased first) | Active filter: {categoryFilter.Label}");

                if (list == null || list.Count == 0)
                {
                    MenuHelpers.Warn($"No products available for {categoryFilter.Label}.");
                    WaitForContinue();
                    break;
                }

                var offers = OfferLogic.GetOffers();
                var productOfferMap = OfferLogic.GetProductToOfferMapping();
                List<string> options = new List<string>();
                List<bool> offerFlags = new List<bool>();
                foreach (var item in list)
                {
                    OfferModel? offer = OfferLogic.GetActiveOfferForProduct(item.ProductID, offers, productOfferMap);
                    offerFlags.Add(offer != null);

                    string ageLabel = item.MinAge > 0 ? $" | Age: {item.MinAge}+" : "";
                    string popularityLabel = item.PurchaseCount > 0 ? " | Best Seller" : string.Empty;
                    string pricePart = offer != null
                        ? $"Price: {offer.DiscountPrice} EUR (Old Price: {offer.RegularPrice} EUR)"
                        : $"Price: {item.Price} EUR";
                    options.Add($"{item.Name} | {item.Category}{popularityLabel} | {pricePart}{ageLabel}  [→ details]");
                }
                options.Add("Back to categories");
                offerFlags.Add(false);

                MenuNavigation browseMenu = new MenuNavigation(options, "--- BROWSE PRODUCTS (Right arrow: details) ---");
                browseMenu.OfferIndicators = offerFlags;
                browseMenu.OnRightArrow = idx =>
                {
                    if (idx >= 0 && idx < list.Count)
                    {
                        ProductDetailsView.Show(list[idx]);
                    }
                };
                int selection = browseMenu.Start();

                if (selection >= 0 && selection < list.Count)
                {
                    ProductDetailsView.Show(list[selection]);
                    continue;
                }

                break;
            }
        }
    }

    private static (bool Cancelled, long? CategoryId, string Label) PromptProductCategoryFilter()
    {
        List<CategoryModel> categories = ProductLogic.GetCategories();
        if (categories.Count == 0)
        {
            return (false, null, "All categories");
        }

        List<string> options = categories.Select(category => category.Name).ToList();
        options.Add("All categories");
        options.Add("Cancel");

        MenuNavigation menu = new MenuNavigation(options, "--- SELECT A CATEGORY FILTER ---");
        int selection = menu.Start();

        if (selection == options.Count - 1)
        {
            return (true, null, string.Empty);
        }

        if (selection == options.Count - 2)
        {
            return (false, null, "All categories");
        }

        CategoryModel selectedCategory = categories[selection];
        return (false, selectedCategory.CategoryID, selectedCategory.Name);
    }

    private static void AddProductToShoppingCart()
{
    Console.Clear();
    var categoryFilter = PromptProductCategoryFilter();
    if (categoryFilter.Cancelled)
    {
        return;
    }

    var products = ProductLogic.GetProductsByPopularity(categoryFilter.CategoryId);
    var offers = OfferLogic.GetOffers();

    if (products == null || !products.Any())
    {
        MenuHelpers.Warn($"No products available for {categoryFilter.Label}");
        WaitForContinue();
        return;
    }

    MenuHelpers.Announce("--- ALL PRODUCTS ---");
    MenuHelpers.Line($"Active sort: Popularity (most purchased first) | Active filter: {categoryFilter.Label}");

    var productOfferMap = OfferLogic.GetProductToOfferMapping();
    List<string> options = new List<string>();
    List<bool> offerFlags = new List<bool>();
    foreach (var item in products)
    {
        OfferModel? offer = OfferLogic.GetActiveOfferForProduct(item.ProductID, offers, productOfferMap);
        offerFlags.Add(offer != null);

        string popularityLabel = item.PurchaseCount > 0 ? " | Best Seller" : string.Empty;
        string pricePart = offer != null
            ? $"Price: {offer.DiscountPrice} EUR (Old Price: {offer.RegularPrice} EUR)"
            : $"Price: {item.Price} EUR";
        options.Add($"Name: {item.Name} | Category: {item.Category}{popularityLabel} | {pricePart}");
    }
    options.Add("Cancel");
    offerFlags.Add(false);

    MenuNavigation menu = new MenuNavigation(options, "--- SELECT A PRODUCT TO ADD (←/→: qty, D: details) ---", true);
    menu.OfferIndicators = offerFlags;
    menu.OnRightArrow = idx =>
    {
        if (idx >= 0 && idx < products.Count)
        {
            ProductDetailsView.Show(products[idx]);
        }
    };
    int selection = menu.Start();
    int quantity = menu.GetQuantity();

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
    if (quantity <= 0)
    {
        MenuHelpers.Warn("Please select a quantity greater than 0");
        WaitForContinue();
        return;
    }
    if (quantity > selectedProduct.Stock)
    {
        MenuHelpers.Warn($"Amount selected too high. Only {selectedProduct.Stock} items in stock");
        WaitForContinue();
        return;
    }

    OfferModel? finalOffer = OfferLogic.GetActiveOfferForProduct(selectedProduct.ProductID, offers, productOfferMap);
    decimal finalPrice = finalOffer != null ? (decimal)finalOffer.DiscountPrice : selectedProduct.Price;

    var cartItem = new ShoppingCartItem(selectedProduct, quantity, finalPrice);
    ShoppingCart.AddItem(cartItem);

    MenuHelpers.Confirm($"Added {selectedProduct.Name} to shopping cart");
    WaitForContinue();
}

    private static void ShowShoppingCart()
    {
        while (true)
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

                MenuHelpers.Line($"{i + 1}. {item.Product.Name} | Category: {item.Product.Category} | Brand: {item.Product.Brand} | Qty: {item.Quantity} | Price: {item.Product.Price} EUR (Total: {lineTotal} EUR)");
            }

            MenuHelpers.Announce($"Total (preview): {total} EUR");
            MenuHelpers.Line();

            List<string> options = new List<string>
            {
                "Checkout and pay",
                "Clear shopping cart",
                "Back to Dashboard"
            };

            MenuNavigation cartMenu = new MenuNavigation(options, "--- CART OPTIONS ---");
            int selection = cartMenu.Start();

            if (selection == 0)
            {
                PurchaseShoppingCart.PurchaseChoice();
                break;
            }
            else if (selection == 1)
            {
                ShoppingCart.GetAllItems().Clear();
                MenuHelpers.Confirm("Shopping cart cleared");
                WaitForContinue();
                break;
            }
            else if (selection == 2)
            {
                break;
            }
        }
    }

    private static void ShowProfileMenu()
    {
        while (true)
        {
            Console.Clear();
            AccountModel? account = AccountsLogic.CurrentAccount;

            List<string> options = new List<string>();
            if (account == null)
            {
                options.Add("Login");
                options.Add("Back to Dashboard");
            }
            else
            {
                options.Add("Wishlist");
                options.Add("Show purchase history");
                options.Add("Logout");
                options.Add("Back to Dashboard");
            }

            MenuNavigation profileMenu = new MenuNavigation(options, "--- PROFILE ---");
            int selection = profileMenu.Start();

            if (account == null)
            {
                if (selection == 0)
                {
                    Menu.Start();
                    return;
                }
                else if (selection == 1)
                {
                    break;
                }
            }
            else
            {
                if (selection == 0)
                {
                    ShowWishlist.Start();
                }
                else if (selection == 1)
                {
                    ShowPurchaseHistory();
                }
                else if (selection == 2)
                {
                    AccountsLogic.Logout();
                    Menu.Start();
                    return;
                }
                else if (selection == 3)
                {
                    break;
                }
            }
        }
    }

    private static void ShowLayout()
    {
        Console.Clear();
        MenuHelpers.Line(@"╔══════════════╦══════════════════╦═══════════════════╗
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
            MenuHelpers.Line("----------------------------------------");
            MenuHelpers.Announce($"  Purchase #{group.Key}  |  {first.CreatedAt:dd-MM-yyyy}");
            MenuHelpers.Line("----------------------------------------");
            foreach (var item in group)
            {
                string qtyLabel = item.Quantity > 1 ? $"x{item.Quantity} " : "";
                decimal lineTotal = item.ProductPrice * item.Quantity;
                MenuHelpers.Line($"  {item.ProductName,-22} {qtyLabel}{lineTotal:F2}");
            }
            MenuHelpers.Line("----------------------------------------");
            MenuHelpers.Line($"  Total:                    {first.TotalPrice:F2}");
            MenuHelpers.Line();
        }

        WaitForContinue();
    }
}