public static class ShowWishlist
{
    public static void Start()
    {
        AccountModel? account = AccountsLogic.CurrentAccount;
        WishlistLogic WLlogic = new(account.UserId);
        Console.Clear();
        MenuHelpers.Confirm("Enter 1 View Wishlist");
        MenuHelpers.Confirm("Enter 2 Add product");
        MenuHelpers.Confirm("Enter 3 Remove product");
        MenuHelpers.Confirm("Enter 4 Clear wishlist");
        MenuHelpers.Confirm("Enter 5 Transfer wishlist to cart");
        MenuHelpers.Confirm("Enter 6 Return to main menu");

        string? input = MenuHelpers.Prompt("Choose option");

        if (input == "1")
        {
            List<ProductModel> wishlist = WLlogic.GetWishlist();
            foreach (var p in wishlist)
            {
                MenuHelpers.Announce($"{p.ProductID}: {p.Name} - {p.Price}");
            }
            MenuHelpers.Pause();
            Start();
        }
        else if (input == "2")
        {
            ShowProducts.ShowAll();
            int id = MenuHelpers.PromptInt("Enter Product ID to add");
            bool success = WLlogic.AddProduct(id);
            if (success) MenuHelpers.Confirm("Product added!");
            else MenuHelpers.Error("Invalid ID or already in wishlist.");
            MenuHelpers.Pause();
            Start();
        }
        else if (input == "3")
        {
            List<ProductModel> wishlist = WLlogic.GetWishlist();
            foreach (var p in wishlist)
            {
                MenuHelpers.Announce($"{p.ProductID}: {p.Name} - {p.Price}");
            }
            int id = MenuHelpers.PromptInt("Enter Product ID to remove");
            MenuHelpers.Confirm(WLlogic.RemoveProduct(id));
            MenuHelpers.Pause();
            Start();
        }
        else if (input == "4")
        {
            WLlogic.ClearWishlist();
            MenuHelpers.Confirm("Wishlist cleared.");
            MenuHelpers.Pause();
            Start();
        }
        else if (input == "5")
        {
            WLlogic.TransferToCart();
            MenuHelpers.Confirm("Items moved to cart.");
            MenuHelpers.Pause();
            Start();
        }
        else if (input == "6")
        {
            Dashboard.Start();
        }
    }
}