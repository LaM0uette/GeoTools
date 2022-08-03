using System;
using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;
using Npgsql;
using Parser;

namespace GeoTools.Views.Dlg;

public partial class WeekDlgView
{
    #region Statements

    public static WeekDlgView Instance = new();
    
    public WeekDlgView()
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
        
        ClearWeekGrid();
        AddDaysInWeekGrid();
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

    private void ClearWeekGrid()
    {
        WeekGrid.Children.Clear();
        WeekGrid.RowDefinitions.Clear();
        WeekGrid.UpdateLayout();
    }

    private void AddDaysInWeekGrid()
    {
        WeekGrid.RowDefinitions.Add(new RowDefinition());
        
        var lbLun = Widgets.NewDlgInfoTextBlock("Lundi", 20);
        var lbMar = Widgets.NewDlgInfoTextBlock("Mardi", 20);
        var lbMer = Widgets.NewDlgInfoTextBlock("Mercredi", 20);
        var lbJeu = Widgets.NewDlgInfoTextBlock("Jeudi", 20);
        var lbVen = Widgets.NewDlgInfoTextBlock("Vendredi", 20);
        
        SetItemsInGrid(lbLun, 0, 0);
        SetItemsInGrid(lbMar, 1, 0);
        SetItemsInGrid(lbMer, 2, 0);
        SetItemsInGrid(lbJeu, 3, 0);
        SetItemsInGrid(lbVen, 4, 0);

        WeekGrid.Children.Add(lbLun);
        WeekGrid.Children.Add(lbMar);
        WeekGrid.Children.Add(lbMer);
        WeekGrid.Children.Add(lbJeu);
        WeekGrid.Children.Add(lbVen);
    }

    private void AddStackPanelOfDays()
    {
        var month = Tasks.GetMonthOfWeek(Constants.Week, Constants.Year);

        var currentDay = DateTime.Now;

        for (var i = 1; i <= DateTime.DaysInMonth(Constants.Year, month); i++)  // Boucle sur tous les jours du mois
        {
            var day = new DateTime(Constants.Year, month, i);
            var weekOfTheDay = Tasks.GetWeekNumber(day);

            if (!weekOfTheDay.Equals(Constants.Week)) continue;
            
            var border = Widgets.NewMonthDlgBorder();
            var stackPanel = Widgets.NewMonthDlgStackPanel($"{i:D2}{month:D2}{Constants.Year}");
            
            var dayName = Widgets.NewDlgInfoTextBlock(i.ParseToString(), 20);
            dayName.HorizontalAlignment = HorizontalAlignment.Left;
            dayName.Margin = new Thickness(0, 0, 0, 20);
            
            if (currentDay.Year.Equals(Constants.Year) && currentDay.Month.Equals(month) && currentDay.Day.Equals(day.Day))
                dayName.Foreground = Constants.Colors.Red;

            WeekGrid.RowDefinitions.Add(new RowDefinition());

            // Check and add RegisterName for StackPanel
            if(FindName(stackPanel.Name)!=null)
                UnregisterName(stackPanel.Name);
            RegisterName(stackPanel.Name, stackPanel);

            switch (day.ToString("dddd", Constants.LangFr).Capitalize())
            {
                case "Lundi":
                    SetItemsInGrid(border, 0, 1);
                    break;
                case "Mardi":
                    SetItemsInGrid(border, 1, 1);
                    break;
                case "Mercredi":
                    SetItemsInGrid(border, 2, 1);
                    break;
                case "Jeudi":
                    SetItemsInGrid(border, 3, 1);
                    break;
                case "Vendredi":
                    SetItemsInGrid(border, 4, 1);
                    break;
                default:
                    continue;
                    
            }

            stackPanel.Children.Add(dayName);
            border.Child = stackPanel;
            WeekGrid.Children.Add(border);
        }
    }
    
    private static void SetItemsInGrid(UIElement uiElement, int col, int row)
    {
        Grid.SetColumn(uiElement, col);
        Grid.SetRow(uiElement, row);
    }

    #endregion
}