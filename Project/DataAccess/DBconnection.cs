using Microsoft.Data.Sqlite;

static class DBconnection
{
        public static SqliteConnection _c = new SqliteConnection($"Data Source={ResolveDatabasePath()}");

        static DBconnection()
        {
            DatabaseMigration.EnsureSchema();
        }

        private static string ResolveDatabasePath()
        {
            string fileName = Path.Combine("DataSources", "project.db");
            string? currentDirectory = AppContext.BaseDirectory;
            string? resolvedPath = null;

            while (!string.IsNullOrWhiteSpace(currentDirectory))
            {
                string candidate = Path.Combine(currentDirectory, fileName);
                if (File.Exists(candidate))
                {
                    resolvedPath = candidate;
                }

                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }

            return resolvedPath ?? Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, fileName));
        }

}