using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace GeoTools.Utils;

public class Tasks
{

    private static BrushConverter converter = new();
    
    public static double GetWindowSize()
    {
        return Application.Current.MainWindow.ActualWidth;
    }

    public static WindowState GetWindowState()
    {
        return Application.Current.MainWindow.WindowState;
    }

    public static Brush HexBrush(string hexColor)
    {
        return (Brush)converter.ConvertFromString(hexColor);
    }

    public static byte DaysInMonth(int year, int month)
    {
        return (byte)DateTime.DaysInMonth(year: year, month: month);
    }
    public static byte WeekInMonth(int year, int month)
    {
        DateTime date = new DateTime(year, month, 1);
        
        var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
        var weekends = from d in dates
            where d.DayOfWeek == DayOfWeek.Monday
            select d;
        return (byte)weekends.Count();

    }
    public static string GetUserSession()
    {
        return Environment.UserName;
    }

}

