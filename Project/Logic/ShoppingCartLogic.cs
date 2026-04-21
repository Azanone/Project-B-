using Project.Models;
namespace Project.Logic;

public class ShoppingCartLogic
{
    private readonly ShoppingCartAccess _cartAccess = new();

    public void AddItem(int userId, int productId, int quantity)
    {
        _cartAccess.AddItemsToCart(userId, productId, quantity);
    }
    
    public void RemoveItem(ShoppingCartItem item, int userId)
    {
        if (item == null)
            throw new ArgumentException("Item doesn't exist in cart");

        _cartAccess.RemoveItemsFromCart(item, userId);
    }

    public void ChangeCount(ShoppingCartModel item, int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be > 0");

        ShoppingCartItem q = new();
        q.Quantity = newQuantity;

        _cartAccess.UpdateCart(item);
    }

    public List<ShoppingCartItem> GetAllItems(int userId)
    {
        return _cartAccess.GetAll(userId);
    }
}