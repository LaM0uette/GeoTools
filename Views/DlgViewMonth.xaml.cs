using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;

namespace GeoTools.Views;

public partial class DlgViewMonth : UserControl
{
    public static DlgViewMonth InstanceDlgViewMonth;

    private static List<string> Jour = new List<string>(){"Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi"};
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
        byte monthEnd = month;
        if (month == 13){ yearEnd++; monthEnd = 1;}

        byte row = 0;
        byte nbWeeks = Tasks.WeekInMonth(year: year, month: month);


        for (DateTime date = new DateTime(day: 1, month: month, year: year);
             date < new DateTime(day: 1, month: monthEnd, year: yearEnd);
             date.AddDays(1))
        {
            string nameOfWeek = date.ToString("dddd", lang);
        }

        //MessageBox.Show($"{nbWeeks}");
        // for (byte row = 0; row < nbWeeks; row++)
        // {
        //     
        // }
    }
}