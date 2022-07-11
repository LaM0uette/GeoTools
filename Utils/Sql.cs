using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;
using GeoTools.Configs;

namespace GeoTools.Utils;

public static class Sql
{
    public static NpgsqlConnection? Connection { get; } = PgConnect();
    //private static NpgsqlConnection Connection = Connect();
    private static NpgsqlTransaction _transaction = Connection.BeginTransaction();

    //
    // INITIALISATIONS

    private static NpgsqlConnection PgConnect()
    {
        NpgsqlConnection connect = new(ConnectString());
        connect.Open();
        return connect;
    }

    public static async Task PgConfig()
    {
        await using var conn = new NpgsqlConnection(ConnectString());
        await conn.OpenAsync();
        conn.Notification += ConnOnNotification;
        await using var cmd = new NpgsqlCommand("LISTEN datachange;", conn);
        cmd.ExecuteNonQuery();
        while (true)
            await conn.WaitAsync();
    }

    private static void ConnOnNotification(object sender, NpgsqlNotificationEventArgs e)
    {
        MessageBox.Show($"{e.Payload}");
    }

    private static string ConnectString()
    {
        var connexionString = 
            @$"HOST={Login.PgHost};
               Username={Login.PgUser};
               Password={Login.PgPassword};
               Database={Login.PgDatabase}";
        return connexionString;
    }
    

    private static void Exec(string req)
    {
        new NpgsqlCommand(req, Connection).ExecuteNonQuery();
        Commit();
    }
    
    private static NpgsqlDataReader GetSqlData(string req)
    {
        var command = new NpgsqlCommand(req, Connection);
        return command.ExecuteReader();
    }
    
    private static void Commit()
    {
        _transaction.Commit();
    }
    
    public static void Close()
    {
        Connection.Close();
    }
    
    //
    // FONCTIONS
    public static void AddDlg(string proj, string refcode3, string dateInit, string phase, string typeExport, int livraison, int version)
    {
        Exec(req:$"SELECT * FROM \"GeoTools\".add_dlg('{proj}', '{refcode3}', '{dateInit}', '{phase}', '{typeExport}', {livraison}, {version})");
    }

    //
    // REQUÊTES
    public static NpgsqlDataReader GetAllDlg()
    {
        const string req = 
            @$"SELECT * 
               FROM ""GeoTools"".""v_dlg""";
        
        return GetSqlData(req);
    }
    
    public static NpgsqlDataReader GetAllDlgFiltered(int id)
    {
        var req = 
            @$"SELECT * 
               FROM ""GeoTools"".""v_dlg""
               WHERE id_etat={id}";
        
        return GetSqlData(req);
    }

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
}