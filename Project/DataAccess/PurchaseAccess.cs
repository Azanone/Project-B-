using Microsoft.Data.Sqlite;

using Dapper;

public class PurchaseAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private string Table = "Purchase";
    public List<PurchaseModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<PurchaseModel>(sql).ToList();
    }

}