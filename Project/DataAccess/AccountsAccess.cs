using Microsoft.Data.Sqlite;

using Dapper;


public class AccountsAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "\"USER\"";

    public void Write(AccountModel account)
    {
        string sql = $"INSERT INTO {Table} (EmailAddress, Password, FullName) VALUES (@EmailAddress, @Password, @FullName)";
        _connection.Execute(sql, account);
    }

    public AccountModel? GetByEmail(string email)
    {
        string sql = $"SELECT UserID AS Id, EmailAddress, Password, FullName FROM {Table} WHERE EmailAddress = @Email";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { Email = email });
    }

    public void Update(AccountModel account)
    {
        string sql = $"UPDATE {Table} SET EmailAddress = @EmailAddress, Password = @Password, FullName = @FullName WHERE UserID = @Id";
        _connection.Execute(sql, account);
    }

    public void Delete(AccountModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE UserID = @Id";
        _connection.Execute(sql, new { Id = account.Id });
    }



}