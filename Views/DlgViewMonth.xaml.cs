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
        
        CreateBtnDlgMonth(year:2022, month:6);
    }

    public void CreateBtnDlgMonth(int year, byte month)
    {
        int yearEnd = year;
        byte monthEnd = (byte)(month + 1);
        if (monthEnd == 13){ yearEnd++; monthEnd = 1;}

        DateTime startDate = new DateTime(day: 1, month: month, year: year);
        DateTime endDate = new DateTime(day: 1, month: monthEnd, year: yearEnd).AddDays(-1);
        
        int firstWeeks = ISOWeek.GetWeekOfYear(startDate);
        int lastWeeks = ISOWeek.GetWeekOfYear(endDate);
        
        for (int i = 0; firstWeeks + i <= lastWeeks; i++)
        {
            foreach (DateTime date in Tasks.EachDay(from:Tasks.GetDayOfWeek(week:firstWeeks + i, year:year), to:Tasks.GetDayOfWeek(week:firstWeeks + i, year:year, DayOfWeek.Friday)))
            {
                int col = (int)date.DayOfWeek - 1;
                StackPanel stackPanel = makePanel(date: date);
                Widget.SetElementGrid(stackPanel, row:i, column:col);
                GridMonth.Children.Add(stackPanel);
            }
        }
    }

    private StackPanel makePanel(DateTime date)
    {
        var style = FindResource("ButtonDLGTemp") as Style;
        string nameOfWeek = Tasks.FistLetterUpper(date.ToString("dddd", lang));

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

        ScrollViewer scrollViewer = new ScrollViewer()
        {
            Content = gridDlg,
            MaxHeight = 200,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
        };

        // Border border = new Border()
        // {
        //     CornerRadius = new CornerRadius(5),
        //     Background = Brushes.White,
        //     Height = Constants.LabelHeighSize,
        //     Width = Constants.LabelWidthSize,
        //     Margin = new Thickness(5),
        // };
        // border.Child = border;
                
        cdReader.Close();
                
        stackPanel.Children.Add(lbJourNom);
        stackPanel.Children.Add(lbNumJour);
        stackPanel.Children.Add(scrollViewer);

        return stackPanel;
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
            VerticalAlignment = VerticalAlignment.Center,
        };
    }
}