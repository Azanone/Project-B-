public class WishlistLogic
{
    private static readonly Project.Logic.ShoppingCartLogic ShoppingCart = Dashboard.ShoppingCart;

    private long UserId;
    private WishlistAccess _data = new WishlistAccess();
    private List<ProductModel> current;

    public WishlistLogic(long USERID)
    {
        UserId = USERID;
        current = _data.GetAll(UserId);
    }

    public List<ProductModel> GetWishlist()
    {
        current = _data.GetAll(UserId);
        return current;
    }

    public bool AddProduct(int productId)
    {
        if (current.Any(p => p.ProductID == productId) || !_data.ProductExists(productId))
        {
            return false;
        }

        _data.Add(UserId, productId);
        return true;
    }

    public string RemoveProduct(int productId)
    {
        _data.Remove(UserId, productId);
        return "Product removed.";
    }

    public void ClearWishlist()
    {
        _data.Clear(UserId);
    }

    public void TransferToCart()
    {
        List<ProductModel> wishlist = GetWishlist();
        ShoppingCart.AddMultiple(wishlist);
        ClearWishlist();
    }
}