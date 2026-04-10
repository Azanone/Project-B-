    using Microsoft.Data.Sqlite;

using Dapper;


public class AccountsAccess
{
    private SqliteConnection _connection = DBconnection._c;

    private string Table = "\"USER\"";

    public void Write(AccountModel account)
    {
        string sql = $"INSERT INTO {Table} (Name, Username, EmailAddress, Password, FullName, Postcode, HouseNumber, phoneNumber, Role) VALUES (@Username, @Username, @EmailAddress, @Password, @FullName, @Postcode, @HouseNumber, @phoneNumber, @Role)";
        _connection.Execute(sql, account);
    }

    public AccountModel? GetByIdentifier(string identifier)
    {
        string sql = $"SELECT UserID AS Id, CASE WHEN lower(Username) = 'admin' OR lower(EmailAddress) = 'admin' THEN 'Admin' ELSE 'User' END AS Role, Username, EmailAddress, Password, FullName, Postcode, HouseNumber, phoneNumber FROM {Table} WHERE EmailAddress = @Identifier OR Username = @Identifier";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { Identifier = identifier });
    }

    public void Update(AccountModel account)
    {
        string sql = $"UPDATE {Table} SET Name = @FullName, Username = @Username, EmailAddress = @EmailAddress, Password = @Password, FullName = @FullName, Postcode = @Postcode, HouseNumber = @HouseNumber, phoneNumber = @phoneNumber WHERE UserID = @Id";
        _connection.Execute(sql, account);
    }

    public void Delete(AccountModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE UserID = @Id";
        _connection.Execute(sql, new { Id = account.Id });
    }



}