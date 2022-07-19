using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Npgsql;

namespace GeoTools.Utils;

public static class Tasks
{
    #region Brush

    private static BrushConverter _converter = new();

    public static Brush HexBrush(string hexColor) => (Brush) _converter.ConvertFromString(hexColor)!;

    #endregion

    //

    #region Window

    public static WindowState GetWindowState() => Application.Current.MainWindow!.WindowState;

    #endregion

    //
    
    #region Functions
    
    public static string Capitalize(this string s) => char.ToUpper(s[0]) + s[1..];

    public static byte GetDaysInMonth(int year, int month) => (byte) DateTime.DaysInMonth(year, month);

    public static byte GetWeeksInMonth(int year, int month)
    {
        var date = new DateTime(year, month, 1);

        var dateTimes = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n =>
            new DateTime(date.Year, date.Month, n));

        var weekends = from dateTime in dateTimes
            where dateTime.DayOfWeek == DayOfWeek.Monday
            select dateTime;

        return (byte) weekends.Count();
    }

    public static DateTime GetDayOfWeek(int week, int year, DayOfWeek dayOfWeek = DayOfWeek.Monday) =>
        ISOWeek.ToDateTime(year, week, dayOfWeek);
    
    public static void SetCurrentTabItem(TabItem tabItem) => tabItem.IsSelected = true;

    #endregion

    //
    
    // TODO: A MODIFIER !
    public static Dictionary<string, object> SqlDict(NpgsqlDataReader cdReader)
    {
        var dict = new Dictionary<string, object>();

        for (var idxColumn = 0; idxColumn < cdReader.FieldCount; idxColumn++)
            dict.Add(cdReader.GetName(idxColumn), cdReader.GetValue(idxColumn));

        return dict;
    }

    public static IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
    {
        for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            yield return day;
    }
}