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
        var weekInc = 0;
        
        var lbLun = Widgets.NewDlgInfoTextBlock("Lundi", 18);
        var lbMar = Widgets.NewDlgInfoTextBlock("Mardi", 18);
        var lbMer = Widgets.NewDlgInfoTextBlock("Mercredi", 18);
        var lbJeu = Widgets.NewDlgInfoTextBlock("Jeudi", 18);
        var lbVen = Widgets.NewDlgInfoTextBlock("Vendredi", 18);
        SetGridItemsV(lbLun, 0, 0);
        SetGridItemsV(lbMar, 1, 0);
        SetGridItemsV(lbMer, 2, 0);
        SetGridItemsV(lbJeu, 3, 0);
        SetGridItemsV(lbVen, 4, 0);
        
        MonthGrid.RowDefinitions.Add(new RowDefinition());
        MonthGrid.Children.Add(lbLun);
        MonthGrid.Children.Add(lbMar);
        MonthGrid.Children.Add(lbMer);
        MonthGrid.Children.Add(lbJeu);
        MonthGrid.Children.Add(lbVen);
        
        
        for (var i = 1; i <= DateTime.DaysInMonth(year, month); i++)  // Boucle sur tous les jours du mois
        {
            var day = new DateTime(year, month, i);
            var weekOfTheDay = Tasks.GetWeekNumber(day);
            var bd = new Border { BorderThickness = new Thickness(2), 
                BorderBrush = Brushes.Gray,
                Margin = new Thickness(4),
                CornerRadius = new CornerRadius(3)
                
            };
            
            var sp = Widgets.NewMonthDlgStackPanel($"{i:D2}{month:D2}{year}");
            var dayName = Widgets.NewDlgInfoTextBlock(i.ParseToString(), 20);
            sp.MinWidth = Constants.Dlg.MonthWidth;
            dayName.HorizontalAlignment = HorizontalAlignment.Left;
            
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
                    SetGridItems(bd, 0, weekInc);
                    break;
                case "Mardi":
                    SetGridItems(bd, 1, weekInc);
                    break;
                case "Mercredi":
                    SetGridItems(bd, 2, weekInc);
                    break;
                case "Jeudi":
                    SetGridItems(bd, 3, weekInc);
                    break;
                case "Vendredi":
                    SetGridItems(bd, 4, weekInc);
                    break;
                default:
                    continue;
                    
            }

            sp.Children.Add(dayName);
            
            bd.Child = sp;

            MonthGrid.Children.Add(bd);
        }
    }
    
    private static void SetGridItems(Border sp, int col, int row)
    {
        Grid.SetColumn(sp, col);
        Grid.SetRow(sp, row);
    }
    
    private static void SetGridItemsV(TextBlock sp, int col, int row)
    {
        Grid.SetColumn(sp, col);
        Grid.SetRow(sp, row);
    }

    #endregion
}