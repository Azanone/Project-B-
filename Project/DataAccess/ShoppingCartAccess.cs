using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.VisualBasic;
using Project.Models;

public class ShoppingCartAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    public List<ShoppingCartModel> GetAll()
    {
        string sql = $"SELECT P.Name, P.Price, P.Category, P.Brand, P.Ingredients FROM ShoppingCart SC JOIN Product P ON SC.ProductID = P.ProductID WHERE SC.UserID = @UserId";
        return _connection.Query<ShoppingCartModel>(sql).ToList();
    }

    public void AddItemsToCart()
    {
        string sql = $"INSERT INTO ShoppingCart SC (UserID, ProductID) VALUES (@UserID, @ProductID)";
        _connection.Query<ShoppingCartModel>(sql).ToList();
    }

    public void UpdateCart()
    {
        string sql = $"UPDATE {Table} SET Name = P.Name, Price = P.Price, Category = P.Category, Brand = P.Brand, Ingredients = P.Ingredients WHERE UserID = @UserId";
        _connection.Query<ShoppingCartModel>(sql).ToList();
    }
    
    public void RemoveItemsFromCart()
    {
        string sql = $"DELETE FROM {Table} WHERE UserID = @UserID AND ProductID = @ProductID";
        _connection.Query<ShoppingCartModel>(sql).ToList();
    }
}