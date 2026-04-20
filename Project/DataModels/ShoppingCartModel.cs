namespace Project.Models;

public class ShoppingCartModel
{
    public int CartId { get; set; }
    public int UserId { get; set; }
    public string CreatedAt { get; set; }
    
    public ShoppingCartModel() { }

    public ShoppingCartModel(int cartId, int userId, string createdAt)
    {
        CartId = cartId;
        UserId = userId;
        CreatedAt = createdAt;
    }
}