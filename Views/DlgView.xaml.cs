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
        SetCurrentMwy();
        UpdateAllDlgMode();

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
        else if (ctrlName.Contains("Year"))
        {
            ctrl = TextBoxYear;
        }
        
        var inc = ctrlName.Contains("Left") ? -1 : 1;

        ctrl.Text = $"{ctrl.Text.ParseToByte() + inc}";
    }

    private void TogBtnDlg_OnClick(object sender, RoutedEventArgs e)
    {
        var btnName = ((ToggleButton) sender).Name;

        Constants.CurrentState = btnName switch
        {
            "TogBtnDlgAFaire" => Constants.TogBtn.AFaire,
            "TogBtnDlgFait" => Constants.TogBtn.Fait,
            _ => Constants.TogBtn.Tout
        };

        UpdateAllDlgMode();

        foreach (var btn in _toggleButtons)
            btn.IsChecked = btnName == btn.Name;
    }
    
    private void BtnToday_OnClick(object sender, RoutedEventArgs e)
    {
        SetCurrentMwy();
    }
    
    private void TextBoxWeek_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var value = TextBoxWeek.Text.ParseToByte();

        TextBoxWeek.Text = value switch
        {
            <= 0 => $"{1}",
            > 60 => $"{60}",
            _ => $"{TextBoxWeek.Text}"
        };

        UpdateAllDlgMode();
        
        var currentMonth = Tasks.GetMonthOfWeek(Constants.Week, Constants.Year);
        TextBoxMonth.Text = currentMonth.ParseToString();
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
        
        UpdateAllDlgMode();
    }
    
    private void TextBoxYear_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var value = TextBoxYear.Text.ParseToInt();

        TextBoxYear.Text = value switch
        {
            <= 2020 => $"{2020}",
            > 2060 => $"{2060}",
            _ => $"{TextBoxYear.Text}"
        };
        
        UpdateAllDlgMode();
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

    private void SetCurrentDay()
    {
        var currentDay = (byte)DateTime.Now.Day;
        Constants.Day = currentDay;
    }
    
    private void SetCurrentWeek()
    {
        var currentWeek = (byte)Tasks.GetWeekNumber(DateTime.Now);
        TextBoxWeek.Text = currentWeek.ParseToString();
        Constants.Week = currentWeek;
    }
    
    private void SetCurrentMonth()
    {
        var currentMonth = (byte)DateTime.Now.Month;
        TextBoxMonth.Text = currentMonth.ParseToString();
        Constants.Month = currentMonth;
    }
    
    private void SetCurrentYears()
    {
        var currentYears = DateTime.Now.Year;
        TextBoxYear.Text = currentYears.ParseToString();
        Constants.Year = currentYears;
    }
    
    private void SetCurrentMwy()
    {
        SetCurrentDay();
        SetCurrentWeek();
        SetCurrentMonth();
        SetCurrentYears();
    }

    private void SetTabItems()
    {
        _vTabItemDlgAll = TabItemAllDlg;
        _vTabItemDlgDay = TabItemDayDlg;
        _vTabItemDlgWeek = TabItemWeekDlg;
        _vTabItemDlgMonth = TabItemMonthDlg;

        Tasks.SetCurrentTabItem(_vTabItemDlgAll);
    }

    private void UpdateAllDate()
    {
        Constants.Year = TextBoxYear.Text.ParseToInt();
        Constants.Month = TextBoxMonth.Text.ParseToByte();
        Constants.Week = TextBoxWeek.Text.ParseToByte();
    }

    private void UpdateAllDlgMode()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        
        UpdateAllDate();

        try
        {
            if (Constants.Year.Equals(0))
            {
                SetCurrentYears();
            }
            
            if (Constants.Month.Equals(0))
            {
                SetCurrentMonth();
            }
            
            if (Constants.Day.Equals(0))
            {
                SetCurrentDay();
            }
            
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
            Dlg.AllDlgView.Instance.CreateDlgButtons(GetReaderAllDlgByMode());
            Dlg.DayDlgView.Instance.CreateDlgButtons(GetReaderDayDlgMode(dt));
            Dlg.WeekDlgView.Instance.CreateDlgButtons(GetReaderWeekDlgMode(Constants.Week, Constants.Year));
            Dlg.MonthDlgView.Instance.CreateDlgButtons(GetReaderMonthDlgMode(Constants.Month, Constants.Year));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            Mouse.OverrideCursor = null;
        }
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