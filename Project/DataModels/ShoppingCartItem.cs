namespace Project.Models;

public class ShoppingCartItem
{
    public int? CartItemId { get; set; }
    public int? ProductId { get; set; }
    public ProductModel Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public ShoppingCartItem()
    {
    }

    public ShoppingCartItem(ProductModel product, int quantity, decimal price)
    {
        Product = product;
        ProductId = (int)product.ProductID;
        Quantity = quantity;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Product.Name} - {Quantity}";
    }
}