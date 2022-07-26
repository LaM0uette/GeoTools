using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GeoTools.Utils;

namespace GeoTools.Views;

public partial class DlgView
{
    #region Statements

    private static TabItem _vTabItemDlgAll = new();
    private static TabItem _vTabItemDlgMonth = new();
    private static List<ToggleButton> _toggleButtons = new();

    #endregion
    
    //

    #region Fonctions

    public DlgView()
    {
        InitializeComponent();
        
        AddComboBoxData();
        AddToggleButtonsInList();
        SetTabItems();

        Tasks.SetCurrentTabItem(_vTabItemDlgAll);
        
        // Detecte le changement d'item du combobox
        ComboBoxTypeView.SelectionChanged += OnViewChanged;

        // Detecte le changement de taille de la mainWindow
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.SizeChanged += OnWindowSizeChanged;
    }

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

    private void SetTabItems()
    {
        _vTabItemDlgAll = TabItemDlgAll;
        _vTabItemDlgMonth = TabItemDlgMonth;
    }

    #endregion

    //

    #region Actions

    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e) =>
        Tasks.SetCurrentTabItem(MainView.VTabItemMenu);

    private void TogBtnDlg_OnClick(object sender, RoutedEventArgs e)
    {
        var btnName = ((ToggleButton) sender).Name;

        Dlg.AllDlgView.Instance.CreateBtnDlgAll(btnName);
        DlgMonthView.InstanceDlgMonthView?.CreateBtnDlgMonth(year: 2022, month: 6, mode: btnName);

        foreach (var btn in _toggleButtons)
            btn.IsChecked = btnName == btn.Name;
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

    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
        static double GetWindowHeight() => Application.Current.MainWindow!.ActualHeight;
        static double GetWindowWidth() => Application.Current.MainWindow!.ActualWidth;

        var maxWindowHeight = GetWindowHeight();
        var scrollSize = Tasks.GetWindowState() == WindowState.Maximized
            ? Constants.MaxScrollBarWith
            : Constants.ScrollBarWith;
        var maxWindowWith = GetWindowWidth() - 300 - scrollSize;

        AllDlgView.DlgAllPanel.Height = maxWindowHeight;
        AllDlgView.Width = maxWindowWith;
        DlgViewMonth.Width = maxWindowWith;
    }

    #endregion
}