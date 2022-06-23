using Npgsql;
using GeoTools.Configs;

namespace GeoTools.Utils;

public class Sql
{
    public struct SqlCommand
    {
        public NpgsqlConnection Connection;
        public NpgsqlTransaction Transaction;
    }

    public static SqlCommand Connect()
    {
        var connexionString = $"HOST={Login.PgHost};" +
                              $"Username={Login.PgUser};" +
                              $"Password={Login.PgPassword};" +
                              $"Database={Login.PgDatabase}";
        
        var connexion = new NpgsqlConnection(connexionString);
        connexion.Open();

        var transaction = connexion.BeginTransaction();
        
        return new SqlCommand {Connection = connexion, Transaction = transaction};
    }
}