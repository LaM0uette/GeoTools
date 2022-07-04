using Npgsql;
using GeoTools.Configs;

namespace GeoTools.Utils;

public static class Sql
{
    private static NpgsqlConnection _connection = Connect();
    private static NpgsqlTransaction _transaction = _connection.BeginTransaction();
    
    //
    // INITIALISATIONS
    private static NpgsqlConnection Connect()
    {
        var connexionString = 
            @$"HOST={Login.PgHost};
               Username={Login.PgUser};
               Password={Login.PgPassword};
               Database={Login.PgDatabase}";

        NpgsqlConnection connect = new(@$"HOST={Login.PgHost};
               Username={Login.PgUser};
               Password={Login.PgPassword};
               Database={Login.PgDatabase}");
        connect.Open();
        return connect;
    }
    
    private static void Exec(string req)
    {
        new NpgsqlCommand(req, _connection).ExecuteNonQuery();
        Commit();
    }
    
    private static NpgsqlDataReader GetSqlData(string req)
    {
        var command = new NpgsqlCommand(req, _connection);
        return command.ExecuteReader();
    }
    
    private static void Commit()
    {
        _transaction.Commit();
    }
    
    public static void Close()
    {
        _connection.Close();
    }
    
    //
    // FONCTIONS
    public static void AddDlg(string proj, string refcode3, string dateInit, string phase, string typeExport, int livraison, int version)
    {
        Exec(req:$"SELECT * FROM \"GeoTools\".add_dlg('{proj}', '{refcode3}', '{dateInit}', '{phase}', '{typeExport}', {livraison}, {version})");
    }
    
    
    /*
        var req = @$"
                SELECT * 
                FROM ""GeoTools"".""t_users""
                WHERE us_guid='{guid}'";
        
        return GetSqlData(req);
     */
    
    //
    // REQUÊTES
    public static NpgsqlDataReader GetDlgByDate(string date)
    {
        var req = 
            @$"SELECT * 
               FROM ""GeoTools"".get_dlg_by_date('{date}')";
        
        return GetSqlData(req);
    }
    
    public static NpgsqlDataReader GetDlgByWeek(byte week, int year)
    {
        var req = 
            @$"SELECT * 
               FROM ""GeoTools"".get_dlg_by_weeks({week}, {year})";
        
        return GetSqlData(req);
    }
    
    public static NpgsqlDataReader GetUserInformation(string guid)
    {
        var req = 
            @$"SELECT * 
               FROM ""GeoTools"".""t_users""
               WHERE us_guid='{guid}'";
        
        return GetSqlData(req);
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
    
    

    public static NpgsqlDataReader GetAllUser()
    {
        return GetSqlData($"SELECT * FROM \"GeoTools\".t_users");
    }
    
    
    
    public static NpgsqlDataReader GetDlgExports(int dlg)
    {
        return GetSqlData(req: $"SELECT * FROM \"GeoTools\".get_dlg_exports({dlg})");
    }
    
    
}