using Microsoft.Data.Sqlite;

using Dapper;
public class ProductAccess
{
    private SqliteConnection _connection = DBconnection._c;
    public List<ProductModel> GetAll()
    {
        string sql = $"SELECT PRODUCT.*, CATEGORY.Name as Category FROM Product JOIN Category ON CATEGORY.CategoryID = PRODUCT.CategoryID";
        return _connection.Query<ProductModel>(sql).ToList();
    }

    public List<CategoryModel> GetCategories()
    {
        string sql = "SELECT CategoryID, Name FROM Category ORDER BY Name";
        return _connection.Query<CategoryModel>(sql).ToList();
    }

    public ProductModel? GetById(long productId)
    {
        string sql = "SELECT PRODUCT.*, CATEGORY.Name as Category FROM Product JOIN Category ON CATEGORY.CategoryID = PRODUCT.CategoryID WHERE PRODUCT.ProductID = @ProductID";
        return _connection.QueryFirstOrDefault<ProductModel>(sql, new { ProductID = productId });
    }

    public long Write(ProductModel product)
    {
        string sql = "INSERT INTO Product (Name, Price, Brand, Ingredients, CategoryID, Stock, MinAge) VALUES (@Name, @Price, @Brand, @Ingredients, @CategoryID, @Stock, @MinAge); SELECT last_insert_rowid();";
        return _connection.ExecuteScalar<long>(sql, product);
    }

    public bool Update(ProductModel product)
    {
        string sql = "UPDATE Product SET Name = @Name, Price = @Price, Brand = @Brand, Ingredients = @Ingredients, CategoryID = @CategoryID, Stock = @Stock, MinAge = @MinAge WHERE ProductID = @ProductID";
        int changedRows = _connection.Execute(sql, product);
        return changedRows > 0;
    }

    public bool Delete(long productId)
    {
        string sql = "DELETE FROM Product WHERE ProductID = @ProductID";
        int changedRows = _connection.Execute(sql, new { ProductID = productId });
        return changedRows > 0;
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