namespace Project.Models;

public class ShoppingCartModel : ShoppingCartItem
{
    public int CartId { get; }
    public int UserId { get; }
    public string CreatedAt { get; }

    public ShoppingCartModel() : base() { }

    public ShoppingCartModel(int cartId, int userId, string createdAt)
    {
        CartId = cartId;
        UserId = userId;
        CreatedAt = createdAt;
    }
}