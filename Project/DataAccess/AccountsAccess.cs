using Microsoft.Data.Sqlite;

using Dapper;


public class AccountsAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "\"USER\"";

    public void Write(AccountModel account)
    {
        string sql = $"INSERT INTO {Table} (Name, Username, EmailAddress, Password, FullName, Postcode, HouseNumber, phoneNumber) VALUES (@FullName, @Username, @EmailAddress, @Password, @FullName, @Postcode, @HouseNumber, @phoneNumber)";
        _connection.Execute(sql, account);
    }

    public AccountModel? GetByIdentifier(string identifier)
    {
        string sql = $"SELECT UserID AS Id, Username, EmailAddress, Password, FullName, Postcode, HouseNumber, phoneNumber FROM {Table} WHERE EmailAddress = @Identifier OR Username = @Identifier";
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