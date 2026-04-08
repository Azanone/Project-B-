namespace Project.Models;

public class ShoppingCartItem
{
    public ShoppingListModel Product { get; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public ShoppingCartItem(ShoppingListModel product, int quantity, double price)
    {
        Product = product;
        Quantity = quantity;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Product.Name} - {Quantity}";
    }
}