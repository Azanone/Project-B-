    using Microsoft.Data.Sqlite;

using Dapper;

static class DBconnection
{
    public static SqliteConnection _c = new SqliteConnection($"Data Source=DataSources/project.db");

}