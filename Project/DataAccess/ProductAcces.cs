using Microsoft.Data.Sqlite;

using Dapper;
public class ProductAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    public List<ProductModel> GetAll()
    {
        string sql = $"SELECT PRODUCT.*, CATEGORY.Name as Category FROM Product JOIN Category ON CATEGORY.CategoryID = PRODUCT.CategoryID";
        return _connection.Query<ProductModel>(sql).ToList();
    }

    public int IDSearchByName(string name)
    {
        string sql = $"SELECT ProductID FROM Product WHERE Name = @name";
        return _connection.QuerySingleOrDefault<int>(sql, new { name });
    }

    public void UpdateStock(int productId,int amount)
    {
        string sql = "UPDATE Product SET Stock = Stock + @amount WHERE ProductID = @productId";
        _connection.Execute(sql, new {amount,productId});
    }
}