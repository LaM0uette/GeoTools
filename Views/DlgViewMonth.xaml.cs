using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using GeoTools.Utils;
using Npgsql;
using Calendar = System.Globalization.Calendar;

namespace GeoTools.Views;

public partial class DlgViewMonth : UserControl
{
    public static DlgViewMonth InstanceDlgViewMonth;
    
    private static readonly CultureInfo lang = CultureInfo.CreateSpecificCulture("fr-FR");
    private static readonly DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
    
    private struct Weeks
    {
        public List<Dictionary<string, object>> Lundi = new ();
        public List<Dictionary<string, object>> Mardi = new();
        public List<Dictionary<string, object>> Mercredi = new();
        public List<Dictionary<string, object>> Jeudi = new();
        public List<Dictionary<string, object>> Vendredi = new();

        public Weeks()
        {
            this.Lundi = Lundi;
            this.Mardi = Mardi;
            this.Mercredi = Mercredi;
            this.Jeudi = Jeudi;
            this.Vendredi = Vendredi;
        }
    }

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
        var style = FindResource("ButtonDLGTemp") as Style;

        DateTime startDate = new DateTime(day: 1, month: month, year: year);
        DateTime endDate = new DateTime(day: 1, month: monthEnd, year: yearEnd).AddDays(-1);
        
        byte firstWeeks = (byte)ISOWeek.GetWeekOfYear(startDate);
        byte lastWeeks = (byte)ISOWeek.GetWeekOfYear(endDate);
        string nameOfWeek;
        
        for (byte i = 0; firstWeeks + i <= lastWeeks; i++)
        {
            NpgsqlDataReader cdReader = Sql.GetDlgByWeek(week: (byte)(firstWeeks + i), year:year);
            Weeks weeks = new();
            while (cdReader.Read())
            {
                Dictionary<string, object> dictionary = Tasks.sqlDict(cdReader);
                
                DateTime date = (DateTime)dictionary["date_initial"];
                
                nameOfWeek = Tasks.FistLetterUpper(date.ToString("dddd", lang));
                switch (nameOfWeek)
                {
                    case "Lundi":
                        weeks.Lundi.Add(dictionary);
                        break;
                    case "Mardi":
                        weeks.Mardi.Add(dictionary);
                        break;
                    case "Mercredi":
                        weeks.Mercredi.Add(dictionary);
                        break;
                    case "Jeudi":
                        weeks.Jeudi.Add(dictionary);
                        break;
                    case "Vendredi":
                        weeks.Vendredi.Add(dictionary);
                        break;
                }
            }
            cdReader.Close();
            
            foreach (DateTime date in Tasks.EachDay(from:Tasks.GetDayOfWeek(week:firstWeeks + i, year:year), to:Tasks.GetDayOfWeek(week:firstWeeks + i, year:year, DayOfWeek.Friday)))
            {
                Brush? foreground = dateNow == DateOnly.FromDateTime(date) ? Brushes.White : FindResource("RgbM2") as Brush;
                int col = (int)date.DayOfWeek;
                
                nameOfWeek = Tasks.FistLetterUpper(date.ToString("dddd", lang));
                List<Dictionary<string, object>> listDay = new ();
                switch (nameOfWeek)
                {
                    case "Lundi":
                        listDay = weeks.Lundi;
                        break;
                    case "Mardi":
                        listDay = weeks.Mardi;
                        break;
                    case "Mercredi":
                        listDay = weeks.Mercredi;
                        break;
                    case "Jeudi":
                        listDay = weeks.Jeudi;
                        break;
                    case "Vendredi":
                        listDay = weeks.Vendredi;
                        break;
                }

                StackPanel stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Label lbJourNom = makeLabel(content: nameOfWeek, foreground: foreground);
                Label lbNumJour = makeLabel(content: $"{date.Day}", foreground: foreground);
                
                Grid gridDlg = new Grid();

                int dlgRow = 0;

                foreach (var dlg in listDay)
                {
                    Button button = Widget.MakeBtnDlg(dictionary: dlg, style: style);
                    Widget.SetElementGrid(button, row: dlgRow);
                    
                    gridDlg.RowDefinitions.Add(new RowDefinition());
                    gridDlg.Children.Add(button);
                    dlgRow++;
                }

                // if (cdReader["date_initial"].ToString() == date.ToString())
                // {
                //     Button button = Widget.MakeBtnDlg(cdReader: cdReader, style: style);
                //     Widget.SetElementGrid(button, row: dlgRow);
                //
                //     gridDlg.RowDefinitions.Add(new RowDefinition());
                //     gridDlg.Children.Add(button);
                //     dlgRow++;
                // }

                ScrollViewer scrollViewer = new ScrollViewer()
                {
                    Content = gridDlg,
                    MaxHeight = 200,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
                };
        
                stackPanel.Children.Add(lbJourNom);
                stackPanel.Children.Add(lbNumJour);
                stackPanel.Children.Add(scrollViewer);

                Widget.SetElementGrid(stackPanel, row:i, column:col);
                GridMonth.Children.Add(stackPanel);

            }
        }
    }

    // private StackPanel MakePanel(DateTime date, NpgsqlDataReader cdReader)
    // {
    //     var style = FindResource("ButtonDLGTemp") as Style;
    //     string nameOfWeek = Tasks.FistLetterUpper(date.ToString("dddd", lang));
    //
    //     StackPanel stackPanel = new StackPanel()
    //     {
    //         Orientation = Orientation.Vertical,
    //         HorizontalAlignment = HorizontalAlignment.Center
    //     };
    //             
    //     Brush? foreground = dateNow == DateOnly.FromDateTime(date) ? Brushes.White : FindResource("RgbM2") as Brush;
    //             
    //     Label lbJourNom = makeLabel(content: nameOfWeek, foreground: foreground);
    //     Label lbNumJour = makeLabel(content: $"{date.Day}", foreground: foreground);
    //     
    //     Grid gridDlg = new Grid();
    //     int dlgRow = 0;
    //     
    //     while (cdReader.Read())
    //     {
    //         if (cdReader["date_initial"].ToString() == date.ToString())
    //         {
    //             Button button = Widget.MakeBtnDlg(dictionary: cdReader, style: style);
    //             Widget.SetElementGrid(button, row: dlgRow);
    //
    //             gridDlg.RowDefinitions.Add(new RowDefinition());
    //             gridDlg.Children.Add(button);
    //             dlgRow++;
    //         }
    //     }
    //
    //     // NpgsqlDataReader cdReader = Sql.GetDlgByDate(date: date.ToString("yyyy-MM-dd"));
    //     // while (cdReader.Read())
    //     // {
    //     //     Button button = Widget.MakeBtnDlg(cdReader: cdReader, style: style);
    //     //     Widget.SetElementGrid(button, row: dlgRow);
    //     //         
    //     //     gridDlg.RowDefinitions.Add(new RowDefinition());
    //     //     gridDlg.Children.Add(button);
    //     //     dlgRow++;
    //     // }
    //
    //     ScrollViewer scrollViewer = new ScrollViewer()
    //     {
    //         Content = gridDlg,
    //         MaxHeight = 200,
    //         VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
    //         HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
    //     };
    //
    //     stackPanel.Children.Add(lbJourNom);
    //     stackPanel.Children.Add(lbNumJour);
    //     stackPanel.Children.Add(scrollViewer);
    //
    //     return stackPanel;
    // }
    
    // private StackPanel MakePanel(DateTime date)
    // {
    //     var style = FindResource("ButtonDLGTemp") as Style;
    //     string nameOfWeek = Tasks.FistLetterUpper(date.ToString("dddd", lang));
    //
    //     StackPanel stackPanel = new StackPanel()
    //     {
    //         Orientation = Orientation.Vertical,
    //         HorizontalAlignment = HorizontalAlignment.Center
    //     };
    //             
    //     Brush foreground = dateNow == date ? Brushes.White : FindResource("RgbM2") as Brush;
    //             
    //     Label lbJourNom = makeLabel(content: nameOfWeek, foreground: foreground);
    //     Label lbNumJour = makeLabel(content: $"{date.Day}", foreground: foreground);
    //     
    //     Grid gridDlg = new Grid();
    //     int dlgRow = 0;
    //     
    //     NpgsqlDataReader cdReader = Sql.GetDlgByDate(date: date.ToString("yyyy-MM-dd"));
    //     while (cdReader.Read())
    //     {
    //         Button button = Widget.MakeBtnDlg(cdReader: cdReader, style: style);
    //         Widget.SetElementGrid(button, row: dlgRow);
    //             
    //         gridDlg.RowDefinitions.Add(new RowDefinition());
    //         gridDlg.Children.Add(button);
    //         dlgRow++;
    //     }
    //
    //     ScrollViewer scrollViewer = new ScrollViewer()
    //     {
    //         Content = gridDlg,
    //         MaxHeight = 200,
    //         VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
    //         HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
    //     };
    //
    //     // Border border = new Border()
    //     // {
    //     //     CornerRadius = new CornerRadius(5),
    //     //     Background = Brushes.White,
    //     //     Height = Constants.LabelHeighSize,
    //     //     Width = Constants.LabelWidthSize,
    //     //     Margin = new Thickness(5),
    //     // };
    //     // border.Child = border;
    //             
    //     cdReader.Close();
    //             
    //     stackPanel.Children.Add(lbJourNom);
    //     stackPanel.Children.Add(lbNumJour);
    //     stackPanel.Children.Add(scrollViewer);
    //
    //     return stackPanel;
    // }
    private Label makeLabel(string content, Brush? foreground)
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