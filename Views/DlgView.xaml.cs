using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GeoTools.Utils;

namespace GeoTools.Views;

public partial class DlgView
{
    
    private static TabItem _vTabItemDlgAll = new ();
    private static TabItem _vTabItemDlgMonth = new ();
    private static List<ToggleButton> _toggleButtons = new();
    
    public DlgView()
    {
        InitializeComponent();
        AddComboBoxData();
        SetTabItems();

        Tasks.SetSelectedTabItem(_vTabItemDlgAll);
        
        AddToggleButtonsInList();
        
        //ChangeWidth();
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.SizeChanged += OnSizeChanged;
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
    
    
    
    
    
    
    
    
    
    
    
    
    
    

    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        Tasks.SetSelectedTabItem(MainView.VTabItemMenu);
    }
    
    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        ChangeWidth();
        ChangeHeight();
    }

    private void ChangeHeight()
    {
        double maxHeigh = GetMaxHeight();
        
        //DlgViewMonth.GridMonth.MaxHeight = maxHeigh;
    }
    private void ChangeWidth()
    {
        double maxWith = GetMaxWidth();
        
        DlgViewAll.Width = maxWith;
        DlgViewMonth.Width = maxWith;
    }

    private double GetMaxHeight()
    {
        return Tasks.GetWindowHeight() - (ActualHeight != double.NaN ? ActualHeight : 0);
    }
    private double GetMaxWidth()
    {
        return Tasks.GetWindowWidth() - DlgLegend.ActualWidth - 
               (Tasks.GetWindowState() == WindowState.Maximized?Constants.MaximazeScrollBarWith:Constants.NormalScrollBarWith);
    }
    
    
    
    void Toggle_OnClick(object sender, RoutedEventArgs e)
    {
        string btnName = ((ToggleButton) sender).Name;
        string mode = $"{((ToggleButton) sender).Content}";

        DlgViewAll.InstanceDlgViewAll.CreateBtnDlgAll(mode:mode);

        foreach (ToggleButton btn in _toggleButtons)
        {
            btn.IsChecked = btnName == btn.Name;
        }
    }
}