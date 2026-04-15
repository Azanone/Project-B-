namespace Project.Models;

public class ShoppingCartItem : ProductModel
{
    public int CartItemId { get; set; }
    public ShoppingCartModel Product { get; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    
    public ShoppingCartItem() { }

    public ShoppingCartItem(int cartItemId, ShoppingCartModel product, int quantity, double price)
    {
        CartItemId = cartItemId;
        Product = product;
        Quantity = quantity;
        Price = price;
        Name = product.Name;
        Brand = product.Brand;
        Ingredients = product.Ingredients;
        CategoryID = product.CategoryID;
    }
}