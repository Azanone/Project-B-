namespace Project.Models;

public class ShoppingCartItem
{
    public ProductModel Product { get; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public ShoppingCartItem(ProductModel product, int quantity, decimal price)
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