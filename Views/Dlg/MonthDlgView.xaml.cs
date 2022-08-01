using System;
using System.Windows;
using System.Windows.Controls;
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
        var dlgs = Tasks.GetListOfDlgStructs(dlgCdReader);
        
        ClearMonthGrid();
        AddStackPanelOfDays();

        foreach (var dlg in dlgs)
        {
            var button = DlgButtons.GetButtonFromMonthDlg(dlg);
            button.Click += SetActionsOnBtnDlg_Click;
            
            var stackPanelName = FindName($"MonthDlgStackPanel{dlg.DateInit:ddMMyyyy}") as StackPanel;

            if (stackPanelName is not null)
                stackPanelName.Children.Add(button);
        }
    }

    private void ClearMonthGrid()
    {
        MonthGrid.Children.Clear();
        MonthGrid.RowDefinitions.Clear();
        MonthGrid.UpdateLayout();
    }

    private void AddStackPanelOfDays()
    {
        // TODO: A MODIFIER !
        const int month = 6;
        const int year = 2022;
        var lastWeek = 0;
        var weekInc = -1;
        
        for (var i = 1; i <= DateTime.DaysInMonth(year, month); i++)  // Boucle sur tous les jours du mois
        {
            var day = new DateTime(year, month, i);
            var weekOfTheDay = Tasks.GetWeekNumber(day);
            var sp = Widgets.NewMonthDlgStackPanel($"{i:D2}{month:D2}{year}");

            // Check if is new week
            if (weekOfTheDay > lastWeek)
            {
                lastWeek = weekOfTheDay;
                weekInc++;
                MonthGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Check and add RegisterName for StackPanel
            if(FindName(sp.Name)!=null)
                UnregisterName(sp.Name);
            RegisterName(sp.Name, sp);

            switch (day.ToString("dddd", Constants.LangFr).Capitalize())
            {
                case "Lundi":
                    SetGridItems(sp, 0, weekInc);
                    break;
                case "Mardi":
                    SetGridItems(sp, 1, weekInc);
                    break;
                case "Mercredi":
                    SetGridItems(sp, 2, weekInc);
                    break;
                case "Jeudi":
                    SetGridItems(sp, 3, weekInc);
                    break;
                case "Vendredi":
                    SetGridItems(sp, 4, weekInc);
                    break;
                default:
                    continue;
                    
            }

            MonthGrid.Children.Add(sp);
        }
    }
    
    private static void SetGridItems(StackPanel sp, int col, int row)
    {
        Grid.SetColumn(sp, col);
        Grid.SetRow(sp, row);
    }

    #endregion
}