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
        string nameOfDay;
        
        for (byte i = 0; firstWeeks + i <= lastWeeks; i++)
        {
            Weeks weeks = sql2Struc(week: (byte)(firstWeeks + i), year:year);

            foreach (DateTime date in Tasks.EachDay(from:Tasks.GetDayOfWeek(week:firstWeeks + i, year:year), to:Tasks.GetDayOfWeek(week:firstWeeks + i, year:year, DayOfWeek.Friday)))
            {
                Brush? foreground = dateNow == DateOnly.FromDateTime(date) ? Brushes.White : FindResource("RgbM2") as Brush;
                int col = (int)date.DayOfWeek;
                
                nameOfDay = Tasks.FistLetterUpper(date.ToString("dddd", lang));
                List<Dictionary<string, object>> listDay = GetDay(weeks: weeks, day: nameOfDay);

                StackPanel stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                Label lbJourNom = makeLabel(content: nameOfDay, foreground: foreground);
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

                ScrollViewer scrollViewer = new ScrollViewer()
                {
                    Content = gridDlg,
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
    private List<Dictionary<string, object>> GetDay (Weeks weeks, string day)
    {
        List<Dictionary<string, object>> listDay = new ();
        switch (day)
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

        return listDay;
    }
    private Weeks sql2Struc(byte week, int year)
    {
        NpgsqlDataReader cdReader = Sql.GetDlgByWeek(week: (byte)(week), year:year);
        Weeks weeks = new();
        while (cdReader.Read())
        {
            Dictionary<string, object> dictionary = Tasks.sqlDict(cdReader);
                
            DateTime date = (DateTime)dictionary["date_initial"];
            
            switch (Tasks.FistLetterUpper(date.ToString("dddd", lang)))
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
        
        return weeks;
    }
}