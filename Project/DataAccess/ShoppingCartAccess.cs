using Microsoft.Data.Sqlite;
using Dapper;
using Project.Models;

public class ShoppingCartAccess
{
    private string Table = "\"CART\"";

    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    public List<ShoppingCartItem> GetAll(ShoppingCartModel cart)
    {
        string sql = $"SELECT CI.CartItemId AS CartItemId, CI.Quantity AS Quantity, P.Name AS Name, P.Price AS Price, P.Brand AS Brand, P.Category AS Category, P.Ingredients AS Ingredients FROM CART C JOIN CART_ITEM CI ON CI.CartId = C.CartId JOIN PRODUCT P ON P.ProductId = CI.ProductId WHERE C.UserId = @UserId";

        return _connection.Query<ShoppingCartItem>(sql, new { UserId = cart.UserId }).ToList();
    }

    public void AddItemsToCart(ShoppingCartModel cart)
    {
        string sql = $"INSERT INTO {Table} (UserID, ProductID) VALUES (@UserId, @CartId)";

        _connection.Execute(sql, cart);
    }

    public void UpdateCart(ShoppingCartModel cart)
    {
        string sql = $"UPDATE CART_ITEM SET Quantity=@Quantity WHERE CartItemID=@CartItemId";

        _connection.Execute(sql, cart);
    }

    public void RemoveItemsFromCart(ShoppingCartModel cart)
    {
        string sql = $"DELETE FROM {Table} WHERE UserID=@UserId AND ProductID=@CartId";

        _connection.Execute(sql, cart);
    }
}