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
}