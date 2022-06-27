using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using GeoTools.Utils;

namespace GeoTools.Views;

public partial class DlgViewMonth : UserControl
{
    public static DlgViewMonth InstanceDlgViewMonth;

    private static List<string> jour = new List<string>(){"Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi"};
    private static readonly CultureInfo lang = CultureInfo.CreateSpecificCulture("fr-FR");

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

        byte row = 0;
        byte nbWeeks = Tasks.WeekInMonth(year: year, month: month);
        DateTime startDate = new DateTime(day: 1, month: month, year: year);
        DateTime endDate = new DateTime(day: 1, month: monthEnd, year: yearEnd).AddDays(-1);

        foreach (DateTime date in Tasks.EachDay(from:startDate, to:endDate))
        {
            string nameOfWeek = Tasks.FistLetterUpper(date.ToString("dddd", lang));
        }
    }
}