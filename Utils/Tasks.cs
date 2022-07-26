using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GeoTools.Views;
using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using Npgsql;
using Parser;

namespace GeoTools.Utils;

public static class Tasks
{
    #region Struct

    public struct DlgStruct
    {
        public int Id;
        public string Dlg;
        public Constants.WeekDays Day;
    }

    #endregion
    
    //
    
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

    #region Functions viewDlg

    private static readonly Style Style = (Application.Current.FindResource("ButtonDLGTemp") as Style)!;
    
    public static void Update(JObject evt)
    {
        Console.WriteLine($"{evt}");

        var dlgId = $"{evt["data"]!["ex_dl_id"]}".ParseToInt();
        var dlgName = $"dlg_{dlgId}";

        Views.Dlg.AllDlgView.Instance.AllDlgPanel.BeginInvoke((Action)(() =>
        {
            var panel = Views.Dlg.AllDlgView.Instance.AllDlgPanel;
            var cdReader = Sql.Get(Req.GetDlg(2));
            // cdReader.Read();
            //
            // Console.WriteLine($"{cdReader["refcode2"]}");
            //
            // var dict = SqlDict(cdReader);
            //
            //  foreach (Button i in panel.Children)
            //  {
            //      if (!i.Name.Equals(dlgName)) continue;
            //      var idx = panel.Children.IndexOf(i);
            //
            //      var button = Widget.MakeBtnDlg(dictionary: dict, style:Style);
            //      button.Click += DlgAllView.BtnDlgAll_Click;
            //      panel.Children.RemoveAt(idx);
            //      panel.Children.Insert(idx, button);
            //      break;
            // }
            
        }));
    
    }

    public static void Delete(JObject evt)
    {
        var dlId = $"dlg_{evt["data"]!["ex_dl_id"]}";
        Views.Dlg.AllDlgView.Instance.AllDlgPanel.BeginInvoke((Action)(() =>
        {
            var panel = Views.Dlg.AllDlgView.Instance.AllDlgPanel;

            foreach (Button i in panel.Children)
            {
                if (!i.Name.Equals(dlId)) continue;
                panel.Children.Remove(i);
                break;
            }
            
        }));
    }

    #endregion
    
    //

    #region TestStruct

    public static List<DlgStruct> GetListOfDlgStructs(NpgsqlDataReader cdReader)
    {
        var dlgStructs = new List<DlgStruct>();
        
        while (cdReader.Read())
        {
            var dlgStruct = new DlgStruct();
            var dateTime = (DateTime)cdReader["date_initial"];

            dlgStruct.Id = $"{cdReader["id"]}".ParseToInt();
            dlgStruct.Dlg = $"{cdReader["dlg"]}";

            dlgStruct.Day = dateTime.ToString("dddd", Constants.LangFr).Capitalize() switch
            {
                "Lundi" => Constants.WeekDays.Lundi,
                "Mardi" => Constants.WeekDays.Mardi,
                "Mercredi" => Constants.WeekDays.Mercredi,
                "Jeudi" => Constants.WeekDays.Jeudi,
                "Vendredi" => Constants.WeekDays.Vendredi,
                _ => dlgStruct.Day
            };

            dlgStructs.Add(dlgStruct);
        }
        
        cdReader.Close();
        return dlgStructs;
    }

    #endregion
    
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