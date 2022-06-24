using Npgsql;
using GeoTools.Configs;

namespace GeoTools.Utils;

public static class Sql
{
    private static NpgsqlConnection Connection = Connect();
    private static NpgsqlTransaction Transaction = Connection.BeginTransaction();

    public static NpgsqlDataReader GetAllDlg()
    {
        return GetSql("SELECT * FROM \"GeoTools\".v_dlg");
    }
    
    public static NpgsqlDataReader GetUserInformation(string guid)
    {
        return GetSql($"SELECT * FROM \"GeoTools\".t_users WHERE us_guid='{guid}'");
    }

    public static NpgsqlDataReader GetAllUser()
    {
        return GetSql($"SELECT * FROM \"GeoTools\".t_users");
    }
    
    public static NpgsqlDataReader GetSql(string req)
    {
        NpgsqlCommand command = new NpgsqlCommand(req, Connection);
        return command.ExecuteReader();
    }

    public static NpgsqlDataReader GetDlgExports(int dlg)
    {
        return GetSql(req: $"SELECT * FROM \"GeoTools\".get_dlg_exports({dlg})");
    }
    
    public static void AddDlg(string proj, string refcode3, string dateInit, string phase, string typeExport, int livraison, int version)
    {
        Exec(req:$"SELECT * FROM \"GeoTools\".add_dlg('{proj}', '{refcode3}', '{dateInit}', '{phase}', '{typeExport}', {livraison}, {version})");
    }
    
    public static void Exec(string req)
    {
        new NpgsqlCommand(req, Connection).ExecuteNonQuery();
        Commit();
    }

    public static void Close()
    {
        Connection.Close();
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