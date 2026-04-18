using Microsoft.Data.Sqlite;

using Dapper;

public class PurchaseAccess
{
    private SqliteConnection _connection = DBconnection._c;
    public List<PurchaseModel> GetAll()
    {
        string sql = $"SELECT PURCHASE.*, USER.Name AS UserName FROM PURCHASE JOIN USER ON PURCHASE.UserID = USER.UserID";
        return _connection.Query<PurchaseModel>(sql).ToList();
    }

}