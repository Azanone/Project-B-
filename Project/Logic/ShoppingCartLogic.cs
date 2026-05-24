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
 
        return (int) account.Id;
    }
 
    public void AddItem(int userId, int productId, int quantity)
    {
        _cartAccess.AddItemsToCart(userId, productId, quantity);
    }
 
    public void AddItem(ShoppingCartItem item)
    {
        if (item == null)
            throw new ArgumentException("Item doesn't exist");
 
        int productId = item.ProductId.HasValue ? item.ProductId.Value : (int)item.Product.ProductID;
        _cartAccess.AddItemsToCart(GetCurrentUserId(), productId, item.Quantity);
    }
   
    public void RemoveItem(int cartItemId)
    {
        _cartAccess.RemoveItemsFromCart(cartItemId);
    }
 
    public void RemoveItem(ShoppingCartItem item)
    {
        if (item.CartItemId.HasValue)
            _cartAccess.RemoveItemsFromCart(item.CartItemId.Value);
    }
 
    public void ChangeCount(ShoppingCartItem item, int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be > 0");
 
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
            AddItem(new ShoppingCartItem(product, 1, product.Price));
        }
    }
 
    public void ClearCurrentCart()
    {
        int userId = GetCurrentUserId();
        foreach (var item in _cartAccess.GetAll(userId).ToList())
        {
            if (item.CartItemId.HasValue)
                _cartAccess.RemoveItemsFromCart(item.CartItemId.Value);
        }
    }
 
   
    public void PurchaseShoppingCart()
    {
        List<ShoppingCartItem> currentShoppingCart = _cartAccess.GetAll(GetCurrentUserId());
        //create Receipt
    }
 
     public (bool Allowed, int RequiredAge, string ProductName) CheckAgeRestriction()
    {
        var items = _cartAccess.GetAll(GetCurrentUserId());
        var restricted = items
            .Where(i => i.Product != null && i.Product.MinAge > 0)
            .OrderByDescending(i => i.Product.MinAge)
            .FirstOrDefault();

        if (restricted == null)
        {
            return (true, 0, string.Empty);
        }

        int required = (int)restricted.Product.MinAge;
        int? age = new AccountsLogic().CalculateAge(AccountsLogic.CurrentAccount?.Birthdate);

        if (age == null || age < required)
        {
            return (false, required, restricted.Product.Name);
        }

        return (true, required, string.Empty);
    }

    public decimal GetTotal(int userId)
    {
        _cartAccess.Connection.Open();
        var cartItems = _cartAccess.GetAll(userId);
        decimal totalAmount = 0;
        foreach (var item in cartItems)
        {
            totalAmount += item.Product.Price * item.Quantity;
        }
        return totalAmount;
    }
 
 
    public bool CompletePurchase(int userId)
    {
        _cartAccess.Connection.Open();
        using var transaction = _cartAccess.Connection.BeginTransaction();
 
        try
        {            
            decimal totalAmount = GetTotal(userId);
            if(totalAmount != 0)
            {
                decimal vat = totalAmount * 0.21m;
                var cartItems = _cartAccess.GetAll(userId);
 
 
                int purchaseId = _cartAccess.CreatePurchase(userId,totalAmount,vat,transaction);
 
                foreach (var item in cartItems)//loop maakt PurchaseItems
                {
                    int prodId = item.ProductId.HasValue ? item.ProductId.Value : (int)item.Product.ProductID;
                    _cartAccess.CreatePurchaseItem(purchaseId, prodId, item.Quantity, item.Product.Price, transaction);
                }
 
                _cartAccess.CreateReceipt(purchaseId,transaction);
 
                // Clear cart
                _cartAccess.ClearCart(userId,transaction);
                transaction.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
        _cartAccess.Connection.Close();
        }
    }
}
 