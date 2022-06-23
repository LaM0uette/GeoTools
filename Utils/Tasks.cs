using System;
using System.Data.SQLite;

namespace GeoTools.Utils;

public class Tasks
{
    public static string GetUserSession()
    {
        return Environment.UserName;
    }

    public static SQLiteDataReader GetData(string cmd)
    {
        SQLiteCommand command = new SQLiteCommand(cmd, MainWindow.Connection);

        return command.ExecuteReader();
    }
    
}

