using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GeoTools.Utils;
using Npgsql;

namespace GeoTools.Views;

public partial class DlgView
{
    #region Statements

    private static TabItem _vTabItemDlgAll = new();
    private static TabItem _vTabItemDlgMonth = new();
    private static List<ToggleButton> _toggleButtons = new();
    
    public DlgView()
    {
        InitializeComponent();
        
        AddComboBoxData();
        AddToggleButtonsInList();
        SetTabItems();

        Tasks.SetCurrentTabItem(_vTabItemDlgAll);
        Dlg.AllDlgView.Instance.CreateDlgButtons(GetReaderAllDlgByMode());
        Dlg.MonthDlgView.Instance.CreateDlgButtons(GetReaderMonthDlgMode(6, 2022));
        
        ComboBoxTypeView.SelectionChanged += OnViewChanged;  // Detecte le changement d'item du combobox
    }

    #endregion

    //

    #region Actions

    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        Tasks.SetCurrentTabItem(MainView.VTabItemMenu);
    }

    private void TogBtnDlg_OnClick(object sender, RoutedEventArgs e)
    {
        var btnName = ((ToggleButton) sender).Name;

        Dlg.AllDlgView.Instance.CreateDlgButtons(GetReaderAllDlgByMode(btnName));
        Dlg.MonthDlgView.Instance.CreateDlgButtons(GetReaderMonthDlgMode(6, 2022, btnName));

        foreach (var btn in _toggleButtons)
            btn.IsChecked = btnName == btn.Name;
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

    private static NpgsqlDataReader GetReaderWeekDlgMode(string btnName = "")
    {
        // TODO: A MODIFIER !
        return btnName switch
        {
            "TogBtnDlgAFaire" => Sql.Get(Req.AllDlgFiltered(1)),
            "TogBtnDlgFait" => Sql.Get(Req.AllDlgFiltered(2)),
            _ => Sql.Get(Req.AllDlg())
        };
    }
    
    private static NpgsqlDataReader GetReaderMonthDlgMode(byte month, int year, string btnName = "")
    {
        return btnName switch
        {
            "TogBtnDlgAFaire" => Sql.Get(Req.DlgFilteredByMonth(month, year, 1)),
            "TogBtnDlgFait" => Sql.Get(Req.DlgFilteredByMonth(month, year, 2)),
            _ => Sql.Get(Req.DlgByMonth(month, year)) // TogBtnDlgTout
        };
    }
    
    private static NpgsqlDataReader GetReaderAllDlgByMode(string btnName = "")
    {
        return btnName switch
        {
            "TogBtnDlgAFaire" => Sql.Get(Req.AllDlgFiltered(1)),
            "TogBtnDlgFait" => Sql.Get(Req.AllDlgFiltered(2)),
            _ => Sql.Get(Req.AllDlg())
        };
    }

    private void SetTabItems()
    {
        _vTabItemDlgAll = TabItemAllDlg;
        _vTabItemDlgMonth = TabItemMonthDlg;
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
                break;
            case 3:
                break;
            case 4:
                Tasks.SetCurrentTabItem(_vTabItemDlgMonth);
                break;
        }
    }

    #endregion
}