namespace Project.Models;

public class ShoppingCartItem
{
    public ShoppingListModel Product { get; }
    public int Quantity { get; set; }

    public ShoppingCartItem(ShoppingListModel product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }
}