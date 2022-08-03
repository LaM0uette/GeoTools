using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using GeoTools.Utils;
using Npgsql;
using Parser;

namespace GeoTools.Views;

public partial class DlgView
{
    #region Statements

    private static TabItem _vTabItemDlgAll = new();
    private static TabItem _vTabItemDlgDay = new();
    private static TabItem _vTabItemDlgWeek = new();
    private static TabItem _vTabItemDlgMonth = new();
    private static List<ToggleButton> _toggleButtons = new();

    public DlgView()
    {
        InitializeComponent();

        AddComboBoxData();
        AddToggleButtonsInList();
        SetTabItems();
        SetCurrentWeek();
        SetCurrentMonth();
        LoadDefaultDlg();

        ComboBoxTypeView.SelectionChanged += OnViewChanged; // Detecte le changement d'item du combobox
    }

    #endregion

    //

    #region Actions

    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        Tasks.SetCurrentTabItem(MainView.VTabItemMenu);
    }

    private void SetTextBoxValue(object sender, RoutedEventArgs e)
    {
        var ctrlName = ((Button) sender).Name;
        var ctrl = new TextBox();

        if (ctrlName.Contains("Week"))
        {
            ctrl = TextBoxWeek;
        }
        else if (ctrlName.Contains("Month"))
        {
            ctrl = TextBoxMonth;
        }
        
        var inc = ctrlName.Contains("Left") ? -1 : 1;

        ctrl.Text = $"{ctrl.Text.ParseToInt() + inc}";
    }

    private void TogBtnDlg_OnClick(object sender, RoutedEventArgs e)
    {
        Mouse.OverrideCursor = Cursors.Wait;
        
        try
        {
            var btnName = ((ToggleButton) sender).Name;

            Constants.CurrentState = btnName switch
            {
                "TogBtnDlgAFaire" => Constants.TogBtn.AFaire,
                "TogBtnDlgFait" => Constants.TogBtn.Fait,
                _ => Constants.TogBtn.Tout
            };

            Dlg.AllDlgView.Instance.CreateDlgButtons(GetReaderAllDlgByMode());
            Dlg.DayDlgView.Instance.CreateDlgButtons(GetReaderDayDlgMode(new DateTime(2022, 6, 13)));
            Dlg.WeekDlgView.Instance.CreateDlgButtons(GetReaderWeekDlgMode(24, 2022));
            Dlg.MonthDlgView.Instance.CreateDlgButtons(GetReaderMonthDlgMode(6, 2022));

            foreach (var btn in _toggleButtons)
                btn.IsChecked = btnName == btn.Name;
        }
        finally
        {
            Mouse.OverrideCursor = null;
        }
    }
    
    private void BtnToday_OnClick(object sender, RoutedEventArgs e)
    {
        SetCurrentWeek();
        SetCurrentMonth();
    }
    
    private void TextBoxWeek_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var value = TextBoxWeek.Text.ParseToInt();

        TextBoxWeek.Text = value switch
        {
            <= 0 => $"{1}",
            > 60 => $"{60}",
            _ => $"{TextBoxWeek.Text}"
        };
    }
    
    private void TextBoxMonth_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var value = TextBoxMonth.Text.ParseToInt();

        TextBoxMonth.Text = value switch
        {
            <= 0 => $"{1}",
            > 12 => $"{12}",
            _ => $"{TextBoxMonth.Text}"
        };
    }

    #endregion

    //

    #region Fonctions

    private void AddComboBoxData()
    {
        ComboBoxTypeView.Items.Insert(0, "TOUT");
        ComboBoxTypeView.Items.Insert(1, new Separator());
        ComboBoxTypeView.Items.Insert(2, "JOUR");
        ComboBoxTypeView.Items.Insert(3, "SEMAINE");
        ComboBoxTypeView.Items.Insert(4, "MOIS");
        ComboBoxTypeView.SelectedItem = ComboBoxTypeView.Items.IndexOf("TOUT");
    }

    private void AddToggleButtonsInList()
    {
        _toggleButtons.Add(TogBtnDlgAll);
        _toggleButtons.Add(TogBtnDlgAFaire);
        _toggleButtons.Add(TogBtnDlgFait);
    }

    private static void LoadDefaultDlg()
    {
        Dlg.AllDlgView.Instance.CreateDlgButtons(GetReaderAllDlgByMode());
        Dlg.DayDlgView.Instance.CreateDlgButtons(GetReaderDayDlgMode(new DateTime(2022, 6, 13)));
        Dlg.WeekDlgView.Instance.CreateDlgButtons(GetReaderWeekDlgMode(24, 2022));
        Dlg.MonthDlgView.Instance.CreateDlgButtons(GetReaderMonthDlgMode(6, 2022));
    }

    private static NpgsqlDataReader GetReaderAllDlgByMode()
    {
        return Constants.CurrentState switch
        {
            Constants.TogBtn.AFaire => Sql.Get(Req.AllDlgFiltered(1)),
            Constants.TogBtn.Fait => Sql.Get(Req.AllDlgFiltered(2)),
            _ => Sql.Get(Req.AllDlg())
        };
    }

    private static NpgsqlDataReader GetReaderDayDlgMode(DateTime day)
    {
        var dt = day.ToString(CultureInfo.GetCultureInfo("fr-FR"));
        
        return Constants.CurrentState switch
        {
            Constants.TogBtn.AFaire => Sql.Get(Req.DlgFilteredByDay(dt, 1)),
            Constants.TogBtn.Fait => Sql.Get(Req.DlgFilteredByDay(dt, 2)),
            _ => Sql.Get(Req.DlgByDate(dt)) // TogBtnDlgTout
        };
    }

    private static NpgsqlDataReader GetReaderWeekDlgMode(byte week, int year)
    {
        return Constants.CurrentState switch
        {
            Constants.TogBtn.AFaire => Sql.Get(Req.DlgFilteredByWeek(week, year, 1)),
            Constants.TogBtn.Fait => Sql.Get(Req.DlgFilteredByWeek(week, year, 2)),
            _ => Sql.Get(Req.DlgByWeek(week, year)) // TogBtnDlgTout
        };
    }

    private static NpgsqlDataReader GetReaderMonthDlgMode(byte month, int year)
    {
        return Constants.CurrentState switch
        {
            Constants.TogBtn.AFaire => Sql.Get(Req.DlgFilteredByMonth(month, year, 1)),
            Constants.TogBtn.Fait => Sql.Get(Req.DlgFilteredByMonth(month, year, 2)),
            _ => Sql.Get(Req.DlgByMonth(month, year)) // TogBtnDlgTout
        };
    }

    private void SetCurrentWeek()
    {
        var currentWeek = Tasks.GetWeekNumber(DateTime.Now);
        TextBoxWeek.Text = currentWeek.ParseToString();
    }
    
    private void SetCurrentMonth()
    {
        var currentMonth = DateTime.Now.Month;
        TextBoxMonth.Text = currentMonth.ParseToString();
    }

    private void SetTabItems()
    {
        _vTabItemDlgAll = TabItemAllDlg;
        _vTabItemDlgDay = TabItemDayDlg;
        _vTabItemDlgWeek = TabItemWeekDlg;
        _vTabItemDlgMonth = TabItemMonthDlg;

        Tasks.SetCurrentTabItem(_vTabItemDlgAll);
    }

    #endregion

    //

    #region Events

    private void OnViewChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (ComboBoxTypeView.SelectedIndex)
        {
            case 0:
                Tasks.SetCurrentTabItem(_vTabItemDlgAll);
                break;
            case 2:
                Tasks.SetCurrentTabItem(_vTabItemDlgDay);
                break;
            case 3:
                Tasks.SetCurrentTabItem(_vTabItemDlgWeek);
                break;
            case 4:
                Tasks.SetCurrentTabItem(_vTabItemDlgMonth);
                break;
        }
    }

    #endregion
}