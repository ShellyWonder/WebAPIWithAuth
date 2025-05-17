using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace WebAPIWithAuth.Data
{
    public class DataUtil
    {
        public static string? GetConnectionString(IConfiguration configuration) 
        { 
            string? connectionString= configuration.GetConnectionString("DefaultConnection");
            string? databaseURL = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseURL) ? connectionString : BuildConnectionString(databaseURL); 
        }
        /// <summary>
        /// This method specific to Railway and PostgreSQL; it will parse the database URL and build a connection string.
        /// if using any other pair combination of database and hosting service, this method will not work.
        /// Check host documentation for specific connection string format.
        /// </summary>
        /// <param name="databaseURL"></param>
        /// <returns></returns>
        private static string? BuildConnectionString(string databaseUrl)
        {
            Uri databaseUri = new Uri(databaseUrl);
            string[] userInfo = databaseUri.Host.Split(":");
            NpgsqlConnectionStringBuilder builder = new()
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,

            };
            return builder.ToString();
        }
    }
}
