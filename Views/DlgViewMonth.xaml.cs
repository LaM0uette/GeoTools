using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GeoTools.Utils;
using Npgsql;
using Calendar = System.Globalization.Calendar;

namespace GeoTools.Views;

public partial class DlgViewMonth : UserControl
{
    public static DlgViewMonth InstanceDlgViewMonth;

    //private static List<string> semaine = new (){"Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi"};
    private static readonly CultureInfo lang = CultureInfo.CreateSpecificCulture("fr-FR");
    private static readonly DateTime dateNow = DateTime.Now;

    public DlgViewMonth()
    {
        InstanceDlgViewMonth = this;
        InitializeComponent();
        
        CreateBtnDlgMonth(year:2022, month:12);
    }

    public void CreateBtnDlgMonth(int year, byte month)
    {
        int yearEnd = year;
        byte monthEnd = (byte)(month + 1);
        if (monthEnd == 13){ yearEnd++; monthEnd = 1;}
        
        var style = FindResource("ButtonDLGTemp") as Style;
        
        DateTime startDate = new DateTime(day: 1, month: month, year: year);
        DateTime endDate = new DateTime(day: 1, month: monthEnd, year: yearEnd).AddDays(-1);
        
        int firstWeeks = ISOWeek.GetWeekOfYear(startDate);
        int lastWeeks = ISOWeek.GetWeekOfYear(endDate);
        
        for (int i = 0; firstWeeks + i <= lastWeeks; i++)
        {
            foreach (DateTime date in Tasks.EachDay(from:Tasks.GetDayOfWeek(week:firstWeeks + i, year:year), to:Tasks.GetDayOfWeek(week:firstWeeks + i, year:year, DayOfWeek.Friday)))
            {
                string nameOfWeek = Tasks.FistLetterUpper(date.ToString("dddd", lang));
                int col = (int)date.DayOfWeek - 1;
                
                StackPanel stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                
                Brush foreground = dateNow == date ? Brushes.White : FindResource("RgbM2") as Brush;
                
                Label lbJourNom = makeLabel(content: nameOfWeek, foreground: foreground);
                Label lbNumJour = makeLabel(content: $"{date.Day}", foreground: foreground);
                
                Grid gridDlg = new Grid();
                int dlgRow = 0;
                
                NpgsqlDataReader cdReader = Sql.GetDlgByDate(date: date.ToString("yyyy-MM-dd"));
                while (cdReader.Read())
                {
                    Button button = Widget.MakeBtnDlg(cdReader: cdReader, style: style);
                    Widget.SetElementGrid(button, row: dlgRow);
                
                    gridDlg.RowDefinitions.Add(new RowDefinition());
                    gridDlg.Children.Add(button);
                    dlgRow++;
                }
                
                cdReader.Close();
                
                stackPanel.Children.Add(lbJourNom);
                stackPanel.Children.Add(lbNumJour);
                stackPanel.Children.Add(gridDlg);

                Widget.SetElementGrid(stackPanel, row:i, column:col);
                GridMonth.Children.Add(stackPanel);
            }

        }
        // foreach (DateTime date in Tasks.EachDay(from: startDate, to: endDate))
        // {
        //     string nameOfWeek = Tasks.FistLetterUpper(date.ToString("dddd", lang));
        //     if (semaine.Contains(nameOfWeek))
        //     {
        //         int col = semaine.IndexOf(nameOfWeek);
        //
        //         StackPanel stackPanel = new StackPanel()
        //         {
        //             Orientation = Orientation.Vertical,
        //             HorizontalAlignment = HorizontalAlignment.Center
        //         };
        //
        //         Brush foreground = dateNow == date ? Brushes.White : FindResource("RgbM2") as Brush;
        //
        //         // Label lbJourNom = new Label()
        //         // {
        //         //     Content = nameOfWeek,
        //         //     Width = Constants.LabelWidthSize,
        //         //     Height = Constants.LabelHeighSize,
        //         //     FontSize = Constants.LabelFontSize,
        //         //     Foreground = foreground,
        //         //     HorizontalAlignment = HorizontalAlignment.Center,
        //         // };
        //         // Label lbNumJour = new Label()
        //         // {
        //         //     Content = $"{date.Day}",
        //         //     Width = Constants.LabelWidthSize,
        //         //     Height = Constants.LabelHeighSize,
        //         //     FontSize = Constants.LabelFontSize,
        //         //     Foreground = foreground,
        //         //     HorizontalAlignment = HorizontalAlignment.Center,
        //         // };
        //
        //         Label lbJourNom = makeLabel(content: nameOfWeek, foreground: foreground);
        //         Label lbNumJour = makeLabel(content: $"{date.Day}", foreground: foreground);
        //         
        //         Grid gridDlg = new Grid();
        //         int dlgRow = 0;
        //
        //         NpgsqlDataReader cdReader = Sql.GetDlgByDate(date: date.ToString("yyyy-MM-dd"));
        //         while (cdReader.Read())
        //         {
        //             Button button = Widget.MakeBtnDlg(cdReader: cdReader, style: style);
        //             Widget.SetElementGrid(button, row: dlgRow);
        //
        //             gridDlg.RowDefinitions.Add(new RowDefinition());
        //             gridDlg.Children.Add(button);
        //             dlgRow++;
        //         }
        //
        //         cdReader.Close();
        //
        //         stackPanel.Children.Add(lbJourNom);
        //         stackPanel.Children.Add(lbNumJour);
        //         stackPanel.Children.Add(gridDlg);
        //         
        //         // Widget.SetElementGrid(lbJourNom, row:0);
        //         // Widget.SetElementGrid(lbNumJour, row:1);
        //         // Widget.SetElementGrid(gridDlg, row: 2);
        //         
        //         Widget.SetElementGrid(stackPanel, row:i, column:col);
        //         GridMonth.Children.Add(stackPanel);
        //     }
        // }
    }

    private Label makeLabel(string content, Brush foreground)
    {
        return new Label()
        {
            Content = content,
            Width = Constants.LabelWidthSize,
            Height = Constants.LabelHeighSize,
            FontSize = Constants.LabelFontSize,
            Foreground = foreground,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
    }
}