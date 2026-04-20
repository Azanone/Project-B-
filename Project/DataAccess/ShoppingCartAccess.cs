using Microsoft.Data.Sqlite;
using Dapper;
using Project.Models;

public class ShoppingCartAccess
{
    private string Table = "\"CART\"";

    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    public List<ShoppingCartItem> GetAll(int userId)
    {
        {
            string sql = @"
        SELECT 
            CI.CartItemId,
            CI.CartId,
            CI.ProductId,
            CI.Quantity,
            P.ProductID,
            P.Name,
            P.Price,
            P.Brand,
            P.Category,
            P.Ingredients
        FROM CART C
        JOIN CART_ITEM CI ON CI.CartId = C.CartId
        JOIN PRODUCT P ON P.ProductId = CI.ProductId
        WHERE C.UserId = @UserId";

            return _connection.Query<ShoppingCartItem, ProductModel, ShoppingCartItem>(
                sql,
                (cartItem, product) =>
                {
                    cartItem.Product = product;
                    return cartItem;
                },
                new { UserId = userId },
                splitOn: "ProductID"
            ).ToList();
        }
    }

    public void AddItemsToCart(ShoppingCartModel cart, int userId)
    {
        string sql = $"INSERT INTO {Table} (UserID, ProductID) VALUES (@UserId, @CartId)";

        _connection.Execute(sql, cart);
    }

    public void UpdateCart(ShoppingCartModel cart)
    {
        string sql = $"UPDATE CART_ITEM SET Quantity=@Quantity WHERE CartItemID=@CartItemId";

        _connection.Execute(sql, cart);
    }

    public void RemoveItemsFromCart(ShoppingCartModel cart, int userId)
    {
        string sql = $"DELETE FROM CART_ITEM WHERE UserID=@UserId AND CartItemId = @CartItemId;";

        _connection.Execute(sql, cart);
    }
}