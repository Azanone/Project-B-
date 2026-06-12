using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

public static class DatabaseMigration
{
    public static void EnsureSchema()
    {
        SqliteConnection connection = DBconnection._c;
        bool wasOpen = connection.State == ConnectionState.Open;
        if (!wasOpen)
        {
            connection.Open();
        }

        try
        {
            EnsureReviewTable(connection);

            HashSet<string> columns = connection
                .Query<(int cid, string name, string type, int notnull, string? dflt_value, int pk)>(
                    "PRAGMA table_info(Product)")
                .Select(column => column.name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            AddColumnIfMissing(connection, columns, "Calories", "INTEGER NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, columns, "Fats", "REAL NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, columns, "Carbs", "REAL NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, columns, "Fiber", "REAL NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, columns, "Protein", "REAL NOT NULL DEFAULT 0");
            AddColumnIfMissing(connection, columns, "Salt", "REAL NOT NULL DEFAULT 0");
        }
        finally
        {
            if (!wasOpen)
            {
                connection.Close();
            }
        }
    }

    private static void EnsureReviewTable(SqliteConnection connection)
    {
        if (TableExists(connection, "REVIEW"))
        {
            return;
        }

        connection.Execute(@"
            CREATE TABLE REVIEW (
                ReviewID INTEGER PRIMARY KEY AUTOINCREMENT,
                ProductID INTEGER NOT NULL,
                Review TEXT NOT NULL,
                Rating REAL NOT NULL,
                FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
            )");
    }

    private static bool TableExists(SqliteConnection connection, string tableName)
    {
        return connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = @tableName",
            new { tableName }) > 0;
    }

    private static void AddColumnIfMissing(SqliteConnection connection, HashSet<string> columns, string columnName, string columnDefinition)
    {
        if (!columns.Contains(columnName))
        {
            connection.Execute($"ALTER TABLE Product ADD COLUMN {columnName} {columnDefinition}");
        }
    }
}
