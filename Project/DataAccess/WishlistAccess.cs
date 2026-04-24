using Microsoft.Data.Sqlite;

using Dapper;
public class WishlistAccess
{
    private SqliteConnection _connection = DBconnection._c;

    public WishlistAccess()
    {
        EnsureWishlistTable();
    }

    private void EnsureWishlistTable()
    {
        string sql = @"
            CREATE TABLE IF NOT EXISTS Wishlist (
                UserID INTEGER NOT NULL,
                ProductID INTEGER NOT NULL,
                PRIMARY KEY (UserID, ProductID),
                FOREIGN KEY(UserID) REFERENCES Account(UserID),
                FOREIGN KEY(ProductID) REFERENCES Product(ProductID)
            );";

        _connection.Execute(sql);
    }

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