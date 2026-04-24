using Microsoft.Data.Sqlite;

using Dapper;
public class WishlistAccess
{
    private SqliteConnection _connection = DBconnection._c;

    public List<ProductModel> GetAll(long UserID)
    {

        string sql = @"
            SELECT p.* FROM PRODUCT p
            JOIN Wishlist w ON p.ProductID = w.ProductID
            WHERE w.UserID = @USID";
        return _connection.Query<ProductModel>(sql, new { USID = UserID }).ToList();
    }

    public bool ProductExists(int ProductID)
    {
        string sql = "SELECT COUNT(1) FROM PRODUCT WHERE ProductID = @PID";
        return _connection.ExecuteScalar<int>(sql, new { PID = ProductID }) > 0;
    }

    public void Add(long UserID, int ProductID)
    {
        string sql = "INSERT INTO Wishlist (UserID, ProductID) VALUES (@USID, @PID)";
        _connection.Execute(sql, new { USID = UserID, PID = ProductID });
    }

    public void Remove(long UserID, int ProductID)
    {
        string sql = "DELETE FROM Wishlist WHERE UserID = @USID AND ProductID = @PID";
        _connection.Execute(sql, new { USID = UserID, PID = ProductID });
    }

    public void Clear(long UserID)
    {
        string sql = "DELETE FROM Wishlist WHERE UserID = @USID";
        _connection.Execute(sql, new { USID = UserID });
    }
}