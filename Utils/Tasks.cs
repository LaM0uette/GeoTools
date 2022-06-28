using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Npgsql;

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

    public static Dictionary<string, object> sqlDict(NpgsqlDataReader cdReader)
    {  
        Dictionary<string, object> dict = new Dictionary<string, object>();
        for( int lp = 0 ; lp < cdReader.FieldCount ; lp++ )
        {
            dict.Add(cdReader.GetName(lp), cdReader.GetValue(lp));
        }
        return dict;
    }


    public static DateTime GetDayOfWeek(int week, int year, DayOfWeek dayOfWeek=DayOfWeek.Monday)
    {
        return ISOWeek.ToDateTime(year: year, week: week, dayOfWeek);
    }
    public static IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
    {
        for(var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            yield return day;
    }
    public static string FistLetterUpper(string s)
    {
        return char.ToUpper(s[0]) + s.Substring(1);
    }
    public static string GetUserSession()
    {
        return Environment.UserName;
    }

}

