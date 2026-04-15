using Project.Models;

namespace Project.Logic;

public class ShoppingCartLogic
{
    private ShoppingCartAccess _cartAccess = new();
    private ShoppingCartModel _cartModel = new();

    public void AddItem(Models.ShoppingCartItem item)
    {
        if (item == null)
            throw new ArgumentException("Item doesn't exist");

        _cartAccess.AddItemsToCart(item.Product);
    }

    public void RemoveItem(Models.ShoppingCartItem item)
    {
        if (item == null)
            throw new ArgumentException("Item doesn't exist in current shopping cart");
        
        _cartAccess.RemoveItemsFromCart(item.Product);
    }

    public void ChangeCount(Models.ShoppingCartItem item, int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity cannot be negative");
        
        
        item.Quantity = newQuantity;

        item.Product.Quantity = newQuantity;

        _cartAccess.UpdateCart(item.Product);
    }

    public List<ShoppingCartItem> GetAllItems(int userId)
    {
        return _cartAccess.GetAll(new ShoppingCartModel(0, userId, ""));
    }
}