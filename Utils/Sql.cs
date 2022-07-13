﻿using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;
using GeoTools.Configs;

namespace GeoTools.Utils;

public static class Sql
{
    public static NpgsqlConnection? Connection { get; set; } = PgConnect();
    private static NpgsqlTransaction _transaction = Connection.BeginTransaction();

    //
    // INITIALISATIONS
    private static string ConnectString(string name)
    {
        var connexionString = 
            @$"HOST={Login.PgHost};
               Username={Login.PgUser};
               Password={Login.PgPassword};
               Database={Login.PgDatabase};
               ApplicationName={name}";
        return connexionString;
    }
    
    private static NpgsqlConnection PgConnect()
    {
        NpgsqlConnection connect = new(ConnectString("GeoTools"));
        connect.Open();
        return connect;
    }

    public static async Task PgConfig()
    {
        await using var conn = new NpgsqlConnection(ConnectString("Geotools_Notif"));
        await conn.OpenAsync();
        conn.Notification += ConnOnNotification;
        await using var cmd = new NpgsqlCommand("LISTEN datachange;", conn);
        cmd.ExecuteNonQuery();
        while (true)
            await conn.WaitAsync();
    }

    private static void Exec(string req)
    {
        PgConnectionIsOpen();
        new NpgsqlCommand(req, Connection).ExecuteNonQuery();
        Commit();
    }
    
    private static NpgsqlDataReader GetSqlData(string req)
    {
        PgConnectionIsOpen();
        var command = new NpgsqlCommand(req, Connection);
        return command.ExecuteReader();
    }
    
    private static void Commit()
    {
        _transaction.Commit();
    }
    
    public static void Close()
    {
        Connection?.Close();
    }
    
    //
    // FONCTIONS
    //TODO: à renommer
    private static void ConnOnNotification(object sender, NpgsqlNotificationEventArgs e)
    {
        MessageBox.Show($"{e.Payload}");
    }
    
    public static void AddDlg(string proj, string refcode3, string dateInit, string phase, string typeExport, int livraison, int version)
    {
        Exec(req:$"SELECT * FROM \"GeoTools\".add_dlg('{proj}', '{refcode3}', '{dateInit}', '{phase}', '{typeExport}', {livraison}, {version})");
    }

    private static void PgConnectionIsOpen()
    {
        try
        {
            var command = new NpgsqlCommand("SELECT * FROM \"GeoTools\".t_logs", Connection);
            var re = command.ExecuteReader();
            re.Close();
        }
        catch (NpgsqlException)
        {
            Console.WriteLine("Connection close connard"); // :) todo: à Renommer + Relancer la fonction notif
            Connection = PgConnect();
            //MainWindow.PgSql = PgConfig();
        }
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