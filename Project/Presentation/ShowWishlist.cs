public static class ShowWishlist
{
    public static void Start()
    {
        AccountModel? account = AccountsLogic.CurrentAccount;
        if (account == null)
        {
            Console.WriteLine("Error: Not logged in.");
            return;
        }
        WishlistLogic WLlogic = new WishlistLogic(account.Id);
        
        Console.Clear();

        List<string> options = new List<string>
        {
            "View Wishlist",
            "Add product",
            "Remove product",
            "Clear wishlist",
            "Transfer wishlist to cart",
            "Return to main menu"
        };

        MenuNavigation menu = new MenuNavigation(options, "--- Wishlist Options  ---");
        int selection = menu.Start();

        switch (selection)
        {
            case 0:
                List<ProductModel> wishlist = WLlogic.GetWishlist();
                foreach (var p in wishlist)
                {
                    MenuHelpers.Announce($"{p.ProductID}: {p.Name} - {p.Price}");
                }
                MenuHelpers.Pause();
                Start();
                break;
            case 1:
                AdminInformationOverview.ShowProducts();
                int addId = MenuHelpers.PromptInt("Enter Product ID to add");
                bool success = WLlogic.AddProduct(addId);
                if (success) MenuHelpers.Confirm("Product added!");
                else MenuHelpers.Error("Invalid ID or already in wishlist.");
                MenuHelpers.Pause();
                Start();
                break;
            case 2:
                List<ProductModel> currentWishlist = WLlogic.GetWishlist();
                foreach (var p in currentWishlist)
                {
                    MenuHelpers.Announce($"{p.ProductID}: {p.Name} - {p.Price}");
                }
                int removeId = MenuHelpers.PromptInt("Enter Product ID to remove");
                MenuHelpers.Confirm(WLlogic.RemoveProduct(removeId));
                MenuHelpers.Pause();
                Start();
                break;
            case 3:
                WLlogic.ClearWishlist();
                MenuHelpers.Confirm("Wishlist cleared.");
                MenuHelpers.Pause();
                Start();
                break;
            case 4:
                WLlogic.TransferToCart();
                MenuHelpers.Confirm("Items moved to cart.");
                MenuHelpers.Pause();
                Start();
                break;
            case 5:
                Dashboard.Start();
                break;
        }
    }
}