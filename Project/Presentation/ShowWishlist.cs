using System;
using System.Collections.Generic;
using System.Linq;
using Project.Logic;
using Project.Models;

public static class ShowWishlist
{
    private static readonly ProductLogic ProductLogic = new();

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

        MenuNavigation menu = new MenuNavigation(options, "--- Wishlist Options ---");
        int selection = menu.Start();

        switch (selection)
        {
            case 0:
                Console.Clear();
                List<ProductModel> wishlist = WLlogic.GetWishlist();
                if (!wishlist.Any())
                {
                    MenuHelpers.Warn("Your wishlist is currently empty.");
                }
                else
                {
                    MenuHelpers.Announce("--- YOUR WISHLIST ITEMS ---");
                    foreach (var p in wishlist)
                    {
                        MenuHelpers.Confirm($"Name: {p.Name} | Category: {p.Category} | Price: {p.Price} EUR");
                    }
                }
                MenuHelpers.Pause();
                Start();
                break;

            case 1:
                Console.Clear();
                var availableProducts = ProductLogic.GetProducts();
                if (availableProducts == null || !availableProducts.Any())
                {
                    MenuHelpers.Warn("No products available to add.");
                    MenuHelpers.Pause();
                    Start();
                    return;
                }

                List<string> addOptions = new List<string>();
                foreach (var p in availableProducts)
                {
                    addOptions.Add($"{p.Name} ({p.Category}) - {p.Price} EUR");
                }
                addOptions.Add("Cancel and go back");

                MenuNavigation addMenu = new MenuNavigation(addOptions, "--- SELECT A PRODUCT TO ADD TO WISHLIST ---");
                int addSelection = addMenu.Start();

                if (addSelection == addOptions.Count - 1)
                {
                    Start();
                    return;
                }

                ProductModel selectedToAdd = availableProducts[addSelection];
                bool success = WLlogic.AddProduct((int)selectedToAdd.ProductID);

                if (success)
                    MenuHelpers.Confirm($"{selectedToAdd.Name} added to wishlist!");
                else
                    MenuHelpers.Error("Product already exists in your wishlist.");

                MenuHelpers.Pause();
                Start();
                break;

            case 2:
                Console.Clear();
                List<ProductModel> currentWishlist = WLlogic.GetWishlist();
                if (!currentWishlist.Any())
                {
                    MenuHelpers.Warn("No products in wishlist to remove.");
                    MenuHelpers.Pause();
                    Start();
                    return;
                }

                List<string> removeOptions = new List<string>();
                foreach (var p in currentWishlist)
                {
                    removeOptions.Add($"{p.Name} - {p.Price} EUR");
                }
                removeOptions.Add("Cancel and go back");

                MenuNavigation removeMenu = new MenuNavigation(removeOptions, "--- SELECT A PRODUCT TO REMOVE FROM WISHLIST ---");
                int removeSelection = removeMenu.Start();

                if (removeSelection == removeOptions.Count - 1)
                {
                    Start();
                    return;
                }

                ProductModel selectedToRemove = currentWishlist[removeSelection];
                MenuHelpers.Confirm(WLlogic.RemoveProduct((int)selectedToRemove.ProductID));
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