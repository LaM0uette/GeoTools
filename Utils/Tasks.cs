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
        public string GuidProj;
        public string Proj;
        public string GuidExec;
        public string Exec;
        public int Nro;
        public int Sro;
        public int Refcode1;
        public string Refcode2;
        public string Refcode3;
        public DateTime DateInit;
        public int Semaine;
        public int Mois;
        public int Annee;
        public string Phase;
        public string TypeExport;
        public int Livraison;
        public int Version;
        public int IdExport;
        public int IdEtat;
        public string CodeEtat;
        public string NomEtat;
        public string CouleurEtat;
        public DateTime DateEtat;
        public string Dlg;
        public string DlgInfos;
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
    
    public static int GetMonthOfWeek(int week, int year)
    {
        var currentDateTime = new DateTime(year, 1, 1);
        var dayOfWeek = currentDateTime.DayOfWeek;
        var dayOfYear = week * 7 - 6;

        if (dayOfWeek != DayOfWeek.Sunday)
            currentDateTime = currentDateTime.AddDays(7 - (int)currentDateTime.DayOfWeek);
        
        return currentDateTime.AddDays(dayOfYear).Month;
    }

    public static int GetWeekNumber(DateTime now) 
    { 
        var ci = CultureInfo.CurrentCulture; 
        var weekNumber = ci.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday); 
        return weekNumber; 
    } 
    
    public static void SetCurrentTabItem(TabItem tabItem) => tabItem.IsSelected = true;

    #endregion

    //

    #region Functions viewDlg
    
    public static void Update(JObject evt)
    {
        Console.WriteLine($"{evt}");

        var dlgId = $"{evt["data"]!["ex_dl_id"]}".ParseToInt();
        var dlgName = $"dlg_{dlgId}";

        Views.Dlg.AllDlgView.Instance.AllDlgWrapPanel.BeginInvoke((Action)(() =>
        {
            var panel = Views.Dlg.AllDlgView.Instance.AllDlgWrapPanel;
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
            //      var button = DlgButtons.MakeBtnDlg(dictionary: dict, style:Style);
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
        Views.Dlg.AllDlgView.Instance.AllDlgWrapPanel.BeginInvoke((Action)(() =>
        {
            var panel = Views.Dlg.AllDlgView.Instance.AllDlgWrapPanel;

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

    #region DlgStruct

    public static List<DlgStruct> GetListOfDlgStructs(NpgsqlDataReader cdReader)
    {
        var dlgStructs = new List<DlgStruct>();
        
        while (cdReader.Read())
        {
            var dlgStruct = new DlgStruct();
            var dateTime = (DateTime)cdReader["date_initial"];

            dlgStruct.Id = $"{cdReader["id"]}".ParseToInt();
            dlgStruct.GuidProj = $"{cdReader["guid_projeteur"]}";
            dlgStruct.Proj = $"{cdReader["projeteur"]}";
            dlgStruct.GuidExec = $"{cdReader["guid_executant"]}";
            dlgStruct.Exec = $"{cdReader["executant"]}";
            dlgStruct.Nro = $"{cdReader["nro"]}".ParseToInt();
            dlgStruct.Sro = $"{cdReader["sro"]}".ParseToInt();
            dlgStruct.Refcode1 = $"{cdReader["refcode1"]}".ParseToInt();
            dlgStruct.Refcode2 = $"{cdReader["refcode2"]}";
            dlgStruct.Refcode3 = $"{cdReader["refcode3"]}";
            dlgStruct.DateInit = DateTime.Parse($"{cdReader["date_initial"]}");
            dlgStruct.Semaine = $"{cdReader["semaine"]}".ParseToInt();
            dlgStruct.Mois = $"{cdReader["mois"]}".ParseToInt();
            dlgStruct.Annee = $"{cdReader["annee"]}".ParseToInt();
            dlgStruct.Phase = $"{cdReader["phase"]}";
            dlgStruct.TypeExport = $"{cdReader["type_export"]}";
            dlgStruct.Livraison = $"{cdReader["livraison"]}".ParseToInt();
            dlgStruct.Version = $"{cdReader["version"]}".ParseToInt();
            dlgStruct.IdExport = $"{cdReader["id_export"]}".ParseToInt();
            dlgStruct.IdEtat = $"{cdReader["id_etat"]}".ParseToInt();
            dlgStruct.CodeEtat = $"{cdReader["code_etat"]}";
            dlgStruct.NomEtat = $"{cdReader["nom_etat"]}";
            dlgStruct.CouleurEtat = $"{cdReader["couleur_etat"]}";
            dlgStruct.DateEtat = DateTime.Parse($"{cdReader["date_etat"]}");
            dlgStruct.Dlg = $"{cdReader["dlg"]}";
            dlgStruct.DlgInfos = $"{cdReader["dlg_infos"]}";
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
}