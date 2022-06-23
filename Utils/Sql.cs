using System.Windows.Input;
using Npgsql;
using GeoTools.Configs;

namespace GeoTools.Utils;

public static class Sql
{
    private static NpgsqlConnection Connection = Connect();
    private static NpgsqlTransaction Transaction = Connection.BeginTransaction();

    public static NpgsqlDataReader GetSql(string req)
    {
        NpgsqlCommand command = new NpgsqlCommand(req, Connection);
        return command.ExecuteReader();
    }

    public static void Exec(string req)
    {
        new NpgsqlCommand(req, Connection).ExecuteNonQuery();
        Commit();
    }

    public static void Commit()
    {
        Transaction.Commit();
    }    
    
    private static NpgsqlConnection Connect()
    {
        var connexionString = $"HOST={Login.PgHost};" +
                              $"Username={Login.PgUser};" +
                              $"Password={Login.PgPassword};" +
                              $"Database={Login.PgDatabase}";
        
        NpgsqlConnection connect = new NpgsqlConnection(connexionString);
        connect.Open();
        return connect;
    }
}