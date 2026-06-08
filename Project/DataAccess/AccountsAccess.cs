    using Microsoft.Data.Sqlite;

using Dapper;


public class AccountsAccess
{
    private SqliteConnection _connection = DBconnection._c;

    private string Table = "\"USER\"";

    public void Write(AccountModel account)
    {
        string sql = $"INSERT INTO {Table} (Name, Username, EmailAddress, Password, FullName, Postcode, HouseNumber, phoneNumber, Role, Birthdate) VALUES (@Username, @Username, @EmailAddress, @Password, @FullName, @Postcode, @HouseNumber, @phoneNumber, @Role, @Birthdate)";
        _connection.Execute(sql, account);
    }

    public AccountModel? GetByIdentifier(string identifier)
    {
        string sql = $"SELECT UserID AS Id, CASE WHEN Role IS NOT NULL AND trim(Role) <> '' THEN Role WHEN lower(Username) = 'admin' OR lower(EmailAddress) = 'admin' THEN 'Admin' ELSE 'User' END AS Role, Username, EmailAddress, Password, FullName, Postcode, HouseNumber, phoneNumber, Birthdate FROM {Table} WHERE EmailAddress = @Identifier OR Username = @Identifier";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { Identifier = identifier });
    }

    public AccountModel? GetById(long userId)
    {
        string sql = $"SELECT UserID AS Id, CASE WHEN Role IS NOT NULL AND trim(Role) <> '' THEN Role WHEN lower(Username) = 'admin' OR lower(EmailAddress) = 'admin' THEN 'Admin' ELSE 'User' END AS Role, Username, EmailAddress, Password, FullName, Postcode, HouseNumber, phoneNumber, Birthdate FROM {Table} WHERE UserID = @UserId";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { UserId = userId });
    }

    public List<AccountModel> GetAllUsers()
    {
        string sql = $"SELECT UserID AS Id, CASE WHEN Role IS NOT NULL AND trim(Role) <> '' THEN Role WHEN lower(Username) = 'admin' OR lower(EmailAddress) = 'admin' THEN 'Admin' ELSE 'User' END AS Role, Username, EmailAddress, Password, FullName, Postcode, HouseNumber, phoneNumber, Birthdate FROM {Table} ORDER BY UserID";
        return _connection.Query<AccountModel>(sql).ToList();
    }

    public bool UpdateRole(long userId, string role)
    {
        string sql = $"UPDATE {Table} SET Role = @Role WHERE UserID = @UserId";
        int changedRows = _connection.Execute(sql, new { UserId = userId, Role = role });
        return changedRows > 0;
    }

    public bool DeleteById(long userId)
    {
        string sql = $"DELETE FROM {Table} WHERE UserID = @UserId";
        int changedRows = _connection.Execute(sql, new { UserId = userId });
        return changedRows > 0;
    }

    public void Update(AccountModel account)
    {
        string sql = $"UPDATE {Table} SET Name = @FullName, Username = @Username, EmailAddress = @EmailAddress, Password = @Password, FullName = @FullName, Postcode = @Postcode, HouseNumber = @HouseNumber, phoneNumber = @phoneNumber, Role = @Role, Birthdate = @Birthdate WHERE UserID = @Id";
        _connection.Execute(sql, account);
    }

    public void Delete(AccountModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE UserID = @Id";
        _connection.Execute(sql, new { Id = account.Id });
    }

}