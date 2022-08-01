using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GeoTools.Utils;
using Npgsql;
using Parser;

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
        AddDaysInMonthGrid();
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

    private void AddDaysInMonthGrid()
    {
        MonthGrid.RowDefinitions.Add(new RowDefinition());
        
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

        MonthGrid.Children.Add(lbLun);
        MonthGrid.Children.Add(lbMar);
        MonthGrid.Children.Add(lbMer);
        MonthGrid.Children.Add(lbJeu);
        MonthGrid.Children.Add(lbVen);
    }

    private void AddStackPanelOfDays()
    {
        // TODO: A MODIFIER !
        const int month = 6;
        const int year = 2022;
        
        var currentDay = DateTime.Now;
        var lastWeek = 0;
        var weekInc = 0;

        for (var i = 1; i <= DateTime.DaysInMonth(year, month); i++)  // Boucle sur tous les jours du mois
        {
            var day = new DateTime(year, month, i);
            var weekOfTheDay = Tasks.GetWeekNumber(day);
            
            var border = Widgets.NewMonthDlgBorder();
            var stackPanel = Widgets.NewMonthDlgStackPanel($"{i:D2}{month:D2}{year}");
            var dayName = Widgets.NewDlgInfoTextBlock(i.ParseToString(), 20);
            dayName.HorizontalAlignment = HorizontalAlignment.Left;
            dayName.Margin = new Thickness(0, 0, 0, 20);
            
            if (currentDay.Year.Equals(year) && currentDay.Month.Equals(month) && currentDay.Day.Equals(day.Day))
                dayName.Foreground = Constants.Colors.Red;
            
            // Check if is new week
            if (weekOfTheDay > lastWeek)
            {
                lastWeek = weekOfTheDay;
                weekInc++;
                MonthGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Check and add RegisterName for StackPanel
            if(FindName(stackPanel.Name)!=null)
                UnregisterName(stackPanel.Name);
            RegisterName(stackPanel.Name, stackPanel);

            switch (day.ToString("dddd", Constants.LangFr).Capitalize())
            {
                case "Lundi":
                    SetItemsInGrid(border, 0, weekInc);
                    break;
                case "Mardi":
                    SetItemsInGrid(border, 1, weekInc);
                    break;
                case "Mercredi":
                    SetItemsInGrid(border, 2, weekInc);
                    break;
                case "Jeudi":
                    SetItemsInGrid(border, 3, weekInc);
                    break;
                case "Vendredi":
                    SetItemsInGrid(border, 4, weekInc);
                    break;
                default:
                    continue;
                    
            }

            stackPanel.Children.Add(dayName);
            border.Child = stackPanel;
            MonthGrid.Children.Add(border);
        }
    }
    
    private static void SetItemsInGrid(UIElement uiElement, int col, int row)
    {
        Grid.SetColumn(uiElement, col);
        Grid.SetRow(uiElement, row);
    }

    #endregion
}