using Npgsql;
using GeoTools.Configs;

namespace GeoTools.Utils;

public static class Sql
{
    private static NpgsqlConnection _connection = Connect();
    private static NpgsqlTransaction _transaction = _connection.BeginTransaction();

    private static NpgsqlDataReader GetSqlData(string req)
    {
        var command = new NpgsqlCommand(req, _connection);
        return command.ExecuteReader();
    }
    
    public static NpgsqlDataReader GetDlgByDate(string date)
    {
        var req = @$"
                SELECT * 
                FROM `GeoTools`.`v_dlg` 
                WHERE date_initial='{date}'
                ";
        
        return GetSqlData(req);
    }

    public static NpgsqlDataReader GetDlgByWeek(byte week, int year)
    {
        // return GetSqlData($"SELECT * FROM \"GeoTools\".v_dlg WHERE semaine={week} AND annee={year} ORDER BY date_initial");
        return GetSqlData($"SELECT * FROM \"GeoTools\".get_dlg_by_weeks({week}, {year});");
        
    }

    public static NpgsqlDataReader GetAllDlgATraiter()
    {
        return GetSqlData("SELECT * FROM \"GeoTools\".v_dlg WHERE id=1");
    }  
    
    public static NpgsqlDataReader GetAllDlg()
    {
        return GetSqlData("SELECT * FROM \"GeoTools\".v_dlg");
    }    
    
    public static NpgsqlDataReader GetSqlFait()
    {
        return GetSqlData("SELECT * FROM \"GeoTools\".v_dlg WHERE id=5");
    }
    
    public static NpgsqlDataReader GetUserInformation(string guid)
    {
        return GetSqlData($"SELECT * FROM \"GeoTools\".t_users WHERE us_guid='{guid}'");
    }

    public static NpgsqlDataReader GetAllUser()
    {
        return GetSqlData($"SELECT * FROM \"GeoTools\".t_users");
    }
    
    
    
    public static NpgsqlDataReader GetDlgExports(int dlg)
    {
        return GetSqlData(req: $"SELECT * FROM \"GeoTools\".get_dlg_exports({dlg})");
    }
    
    public static void AddDlg(string proj, string refcode3, string dateInit, string phase, string typeExport, int livraison, int version)
    {
        Exec(req:$"SELECT * FROM \"GeoTools\".add_dlg('{proj}', '{refcode3}', '{dateInit}', '{phase}', '{typeExport}', {livraison}, {version})");
    }
    
    public static void Exec(string req)
    {
        new NpgsqlCommand(req, _connection).ExecuteNonQuery();
        Commit();
    }

    public static void Close()
    {
        _connection.Close();
    }
    
    public static void Commit()
    {
        _transaction.Commit();
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