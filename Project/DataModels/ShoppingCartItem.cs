namespace Project.Models;

public class ShoppingCartItem : ShoppingCartModel
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }
    public int? ProductId { get; set; }

    public ProductModel Product { get; set; }

    public int Quantity { get; set; }

    public ShoppingCartItem() {}

    public ShoppingCartItem(ProductModel product, int quantity)
    {
        Product = product;
        ProductId = product.ProductID;
        Quantity = quantity;
    }
}