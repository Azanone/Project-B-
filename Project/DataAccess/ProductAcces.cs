using Microsoft.Data.Sqlite;

using Dapper;
public class ProductAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private string Table = "User";
    public List<ProductModel> GetAll()
    {
        string sql = $"SELECT * FROM Product";
        return _connection.Query<ProductModel>(sql).ToList();
    }
}