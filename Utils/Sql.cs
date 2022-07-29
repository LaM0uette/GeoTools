using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Accessibility;
using Npgsql;
using GeoTools.Configs;
using GeoTools.Views;
using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using Parser;

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

    public static async Task PgNotifierConnect()
    {
        await using var conn = new NpgsqlConnection(Login.GeoToolsNotif.ToString());
        await conn.OpenAsync();
        conn.Notification += EventOnNotification;
        
        await using var cmd = new NpgsqlCommand("LISTEN datachange;", conn);
        cmd.ExecuteNonQuery();
        
        while (true) await conn.WaitAsync();
        // ReSharper disable once FunctionNeverReturns
    }

    private static void Commit() => PgTransaction.Commit();
    public static void Close() => PgConnection?.Close();

    //
    // FONCTIONS
    private static void CheckPgConnection()
    {
        try
        {
            var command = new NpgsqlCommand(Req.Logs(), PgConnection);
            var re = command.ExecuteReader();
            re.Close();
        }
        catch (NpgsqlException)
        {
            Console.WriteLine("Re connection à la base OK");
            PgConnection = PgConnect();
            //TODO: A FAIRE le close puis re co du thread de notif - MainWindow.PgSql = PgNotifierConnect();
        }
    }
    
    private static void EventOnNotification(object sender, NpgsqlNotificationEventArgs evt)
    {
        var notify = JObject.Parse(evt.Payload);

        // Console.WriteLine(i["action"]);

        switch ($"{notify["action"]}")
        {
            case "DELETE":
                Tasks.Delete(notify);
                break;
            case "INSERT":
                break;
            case "UPDATE":
                Tasks.Update(notify);
                break;
        }
    }

    private static async Task TestAsyncTask()
    {
        await Task.Run(() =>
        {
            for (var i = 0; i < 1000; i++)
            {
                Console.WriteLine($"ASync: {i}");
                Task.Delay(40);
            }
        });
    }
    
    private static void TestsyncTask()
    {
        var instance = Views.Dlg.AllDlgView.Instance.AllDlgWrapPanel;
        Console.WriteLine("ereerer");
        foreach (var dlg in instance.Children)
        {
            Console.WriteLine("ereerer");
            Console.WriteLine(dlg);
        }
        Console.WriteLine("ereerer");
    }
    
    

        //
    // DELEGATE
    public delegate void SqlDelegate(string req);

    public delegate NpgsqlDataReader SqlDataDelegate(string req);

    public static readonly SqlDelegate Call = ExecSql;

    public static readonly SqlDataDelegate Get = GetSqlData;

    //
    // REQUÊTES
    private static void ExecSql(string req)
    {
        CheckPgConnection();
        new NpgsqlCommand(req, PgConnection).ExecuteNonQuery();
        Commit();
    }
    
    private static NpgsqlDataReader GetSqlData(string req)
    {
        CheckPgConnection();
        var command = new NpgsqlCommand(req, PgConnection);
        return command.ExecuteReader();
    }
}
