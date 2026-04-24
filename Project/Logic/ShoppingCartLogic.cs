using Project.Models;
namespace Project.Logic;

public class ShoppingCartLogic
{
    private readonly ShoppingCartAccess _cartAccess = new();

    private static int GetCurrentUserId()
    {
        AccountModel? account = AccountsLogic.CurrentAccount;
        if (account == null)
        {
            throw new InvalidOperationException("User must be logged in.");
        }

        return account.UserId;
    }

    public void AddItem(int userId, int productId, int quantity)
    {
        _cartAccess.AddItemsToCart(userId, productId, quantity);
    }

    public void AddItem(ShoppingCartItem item)
    {
        if (item == null)
            throw new ArgumentException("Item doesn't exist");

        int productId = item.ProductId ?? item.Product.ProductID;
        _cartAccess.AddItemsToCart(GetCurrentUserId(), productId, item.Quantity);
    }
    
    public void RemoveItem(int cartItemId)
    {
        _cartAccess.RemoveItemsFromCart(cartItemId);
    }

    public void RemoveItem(ShoppingCartItem item)
    {
        _cartAccess.RemoveItemsFromCart(item.CartItemId);
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

    public List<ShoppingCartItem> GetAllItems()
    {
        return _cartAccess.GetAll(GetCurrentUserId());
    }

    public void AddMultiple(List<ProductModel> products)
    {
        foreach (var product in products)
        {
            AddItem(new ShoppingCartItem(product, 1));
        }
    }

    public void ClearCurrentCart()
    {
        int userId = GetCurrentUserId();
        foreach (var item in _cartAccess.GetAll(userId).ToList())
        {
            _cartAccess.RemoveItemsFromCart(item.CartItemId);
        }
    }
}