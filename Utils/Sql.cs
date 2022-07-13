using System;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;
using GeoTools.Configs;

namespace GeoTools.Utils;

public static class Sql
{
    private static NpgsqlConnection? PgConnection { get; set; } = PgConnect();
    private static NpgsqlTransaction PgTransaction { get; } = PgConnection.BeginTransaction();

    //
    // INITIALISATIONS
    private static NpgsqlConnection PgConnect()
    {
        var connect = new NpgsqlConnection(Login.GeoTools.ToString());
        connect.Open();
        return connect;
    }

    public static async Task PgConfig()
    {
        await using var conn = new NpgsqlConnection(Login.GeoToolsNotif.ToString());
        await conn.OpenAsync();
        conn.Notification += ConnOnNotification;
        await using var cmd = new NpgsqlCommand("LISTEN datachange;", conn);
        cmd.ExecuteNonQuery();
        while (true)
            await conn.WaitAsync();
    }

    private static void Commit() => PgTransaction.Commit();
    public static void Close() => PgConnection?.Close();
    
    //
    // FONCTIONS
    private static void Exec(string req)
    {
        PgConnectionIsOpen();
        
        new NpgsqlCommand(req, PgConnection).ExecuteNonQuery();
        Commit();
    }
    
    private static NpgsqlDataReader GetSqlData(string req)
    {
        PgConnectionIsOpen();
        
        var command = new NpgsqlCommand(req, PgConnection);
        return command.ExecuteReader();
    }
    
    private static void PgConnectionIsOpen()
    {
        try
        {
            var command = new NpgsqlCommand("SELECT * FROM \"GeoTools\".t_logs", PgConnection);
            var re = command.ExecuteReader();
            re.Close();
        }
        catch (NpgsqlException)
        {
            Console.WriteLine("PgConnection close connard"); // :) todo: à Renommer + Relancer la fonction notif
            PgConnection = PgConnect();
            //MainWindow.PgSql = PgConfig();
        }
    }
    
    // TODO: à renommer
    private static void ConnOnNotification(object sender, NpgsqlNotificationEventArgs evt)
    {
        MessageBox.Show($"{evt.Payload}");
    }
    
    public static void AddDlg(string proj, string refcode3, string dateInit, string phase, string typeExport, int livraison, int version)
    {
        var req = 
            @$"SELECT * 
               FROM ""GeoTools"".""add_dlg""('{proj}', '{refcode3}', '{dateInit}', '{phase}', '{typeExport}', {livraison}, {version})";
        
        Exec(req);
    }

    

    //
    // REQUÊTES
    public static NpgsqlDataReader GetAllDlg()
    {
        const string req = 
            @"SELECT * 
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
    public static NpgsqlDataReader GetDlgFilteredByWeek(byte week, int year, int id)
    {
        var req = 
            @$"SELECT * 
               FROM ""GeoTools"".get_dlg_by_weeks({week}, {year})
               WHERE id_etat = {id}";
        
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