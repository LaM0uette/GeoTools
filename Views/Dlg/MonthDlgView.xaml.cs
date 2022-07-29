using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Accessibility;
using GeoTools.Utils;
using Npgsql;

namespace GeoTools.Views.Dlg;

public partial class MonthDlgView
{
    #region Statements

    public static MonthDlgView Instance = new();
    
    public MonthDlgView()
    {
        Instance = this;
        InitializeComponent();
    }

    #endregion

    //
    
    #region Actions

    // TODO: A MODIFIER !
    private static void SetActionsOnBtnDlg_Click(object sender, RoutedEventArgs e)
    {
        var btnName = ((Button) sender).Name;
        
        if (btnName is not null)
        {
            MessageBox.Show($"Tu as cliqué sur le bouton : {btnName}");
        }
    }

    #endregion
    
    //

    #region Fonctions

    public void CreateDlgButtons(NpgsqlDataReader dlgCdReader)
    {
        MonthGrid.Children.Clear();

        var dlgStructs = Tasks.GetListOfDlgStructs(dlgCdReader);
        
        // TODO: A MODIFIER !
        const int month = 6;
        const int year = 2022;
        var week = 0;
        var weekI = -1;
        
        for (var i = 1; i <= DateTime.DaysInMonth(year, month); i++)  // Boucle sur tous les jours du mois
        {
            var dt = new DateTime(year, month, i);

            if (Tasks.GetWeekNumber(dt) > week)
            {
                week = Tasks.GetWeekNumber(dt);
                weekI++;
                MonthGrid.RowDefinitions.Add(new RowDefinition());
            }
            
            var sp = Widgets.NewMonthDlgStackPanel($"{i:D2}{month:D2}{year}");
            var name = Widgets.NewDlgInfoTextBlock($"{i:D2}{month:D2}{year}");
            
            //Console.WriteLine(sp.Name);

            switch (dt.ToString("dddd", Constants.LangFr).Capitalize())
            {
                case "Lundi":
                    Grid.SetColumn(sp, 0);
                    Grid.SetRow(sp, weekI);
                    break;
                case "Mardi":
                    Grid.SetColumn(sp, 1);
                    Grid.SetRow(sp, weekI);
                    break;
                case "Mercredi":
                    Grid.SetColumn(sp, 2);
                    Grid.SetRow(sp, weekI);
                    break;
                case "Jeudi":
                    Grid.SetColumn(sp, 3);
                    Grid.SetRow(sp, weekI);
                    break;
                case "Vendredi":
                    Grid.SetColumn(sp, 4);
                    Grid.SetRow(sp, weekI);
                    break;
                default:
                    continue;
                    
            }

            MonthGrid.Children.Add(sp);
        }

        foreach (var dlgStruct in dlgStructs)
        {
            var button = DlgButtons.GetButtonFromDlg(dlgStruct);
            button.Click += SetActionsOnBtnDlg_Click;

            var lb = new Label{Content = "Salut", Background = Brushes.Blue, Width = 50, Height = 50};

            try
            {
                var myControl1 = FindName($"MonthDlgStackPanel{dlgStruct.DateInit:ddMMyyyy}") as StackPanel;

                Console.WriteLine("");
                Console.WriteLine("*******************");
                Console.WriteLine(myControl1.Name);
                Console.WriteLine($"MonthDlgStackPanel{dlgStruct.DateInit:ddMMyyyy}");
                Console.WriteLine("*******************");
                
                myControl1.Children.Add(lb);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine($"ERREUR -- MonthDlgStackPanel{dlgStruct.DateInit:ddMMyyyy}");
            }

            //AllDlgPanel.Children.Add(button);
        }
    }

    #endregion
}