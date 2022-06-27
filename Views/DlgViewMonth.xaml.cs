using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GeoTools.Utils;
using Npgsql;

namespace GeoTools.Views;

public partial class DlgViewMonth : UserControl
{
    public static DlgViewMonth InstanceDlgViewMonth;

    private static List<string> semaine = new List<string>(){"Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi"};
    private static readonly CultureInfo lang = CultureInfo.CreateSpecificCulture("fr-FR");
    private static DateTime dateNow = DateTime.Now;

    public DlgViewMonth()
    {
        InstanceDlgViewMonth = this;
        InitializeComponent();
        
        CreateBtnDlgMonth(year:2022, month:6);
    }

    public void CreateBtnDlgMonth(int year, byte month)
    {
        int yearEnd = year;
        byte monthEnd = (byte)(month + 1);
        if (month == 13){ yearEnd++; monthEnd = 1;}
        
        var style = FindResource("ButtonDLGTemp") as Style;
        
        byte nbWeeks = Tasks.WeekInMonth(year: year, month: month);
        DateTime startDate = new DateTime(day: 1, month: month, year: year);
        DateTime endDate = new DateTime(day: 1, month: monthEnd, year: yearEnd).AddDays(-1);

        for (int i = 0; i < nbWeeks; i++)
        {
            byte row = 0;
            Grid grid = new Grid();
            foreach (DateTime date in Tasks.EachDay(from: startDate, to: endDate))
            {
                string nameOfWeek = Tasks.FistLetterUpper(date.ToString("dddd", lang));
                if (semaine.Contains(nameOfWeek))
                {
                    int col = semaine.IndexOf(nameOfWeek);
                    Brush foreground = dateNow == date ? Brushes.White : Brushes.Brown;

                    Label lbJourNom = new Label()
                    {
                        Content = nameOfWeek,
                        Foreground = foreground,
                    };
                    Label lbNumJour = new Label()
                    {
                        Content = $"{date.Day}",
                        Foreground = foreground
                    };

                    Grid gridDlg = new Grid();
                    int dlgRow = 0;

                    NpgsqlDataReader cdReader = Sql.GetDlgByDate(date: date.ToString("yyyy-MM-dd"));
                    while (cdReader.Read())
                    {
                        Button button = Widget.MakeBtnDlg(cdReader: cdReader, style: style);
                        Widget.SetElementGrid(button, row: dlgRow);

                        gridDlg.Children.Add(button);
                        dlgRow++;
                    }

                    cdReader.Close();

                    Widget.SetElementGrid(gridDlg, row: row, column: col);
                    grid.Children.Add(gridDlg);
                }
            }
            Widget.SetElementGrid(grid, row:i);
        }
    }
}