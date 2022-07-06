using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GeoTools.Utils;

namespace GeoTools.Views;

public partial class DlgView
{
    private static TabItem _vTabItemDlgAll = new();
    private static TabItem _vTabItemDlgMonth = new();
    private static List<ToggleButton> _toggleButtons = new();

    public DlgView()
    {
        InitializeComponent();
        AddComboBoxData();
        SetTabItems();

        Tasks.SetSelectedTabItem(_vTabItemDlgAll);

        AddToggleButtonsInList();

        // Detecte le changement de taille de la mainWindow
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.SizeChanged += OnWindowSizeChanged;
    }

    //
    // Functions
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
        _toggleButtons.Add(BtnDlgAll);
        _toggleButtons.Add(BtnDlgATraiter);
        _toggleButtons.Add(BtnDlgFait);
    }

    private void SetTabItems()
    {
        _vTabItemDlgAll = TabItemDlgAll;
        _vTabItemDlgMonth = TabItemDlgMonth;
    }

    //
    // Actions


    //
    // Event
    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
        //var maxHeigh = Tasks.GetWindowHeight() - (ActualHeight != double.NaN ? ActualHeight : 0);
        var maxWith = Tasks.GetWindowWidth() - DlgLegend.ActualWidth - (
            Tasks.GetWindowState() == WindowState.Maximized
            ? Constants.MaximazeScrollBarWith
            : Constants.NormalScrollBarWith
            );

        //DlgViewMonth.GridMonth.MaxHeight = maxHeigh;
        DlgViewAll.Width = maxWith;
        DlgViewMonth.Width = maxWith;
    }


    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        Tasks.SetSelectedTabItem(MainView.VTabItemMenu);
    }

    void Toggle_OnClick(object sender, RoutedEventArgs e)
    {
        string btnName = ((ToggleButton) sender).Name;
        string mode = $"{((ToggleButton) sender).Content}";

        DlgViewAll.InstanceDlgViewAll.CreateBtnDlgAll(mode: mode);

        foreach (ToggleButton btn in _toggleButtons)
        {
            btn.IsChecked = btnName == btn.Name;
        }
    }
}