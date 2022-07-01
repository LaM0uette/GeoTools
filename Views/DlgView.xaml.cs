using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GeoTools.Utils;

namespace GeoTools.Views;

public partial class DlgView
{
    
    public static TabItem VTabItemDlgAll = new ();
    public static TabItem VTabItemDlgMonth = new ();
    private List<ToggleButton> _toggleButtons = new();
    
    public DlgView()
    {
        InitializeComponent();
        VTabItemDlgAll = TabItemDlgAll;
        VTabItemDlgMonth = TabItemDlgMonth;

        Tasks.SetTabItem(VTabItemDlgAll);
        
        FillListBtn();
        
        //ChangeWidth();
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.SizeChanged += OnSizeChanged;
    }

    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        Tasks.SetTabItem(MainView.VTabItemMenu);
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
    
    private void FillListBtn()
    {
        _toggleButtons.Add(BtnDlgATraiter);
        _toggleButtons.Add(BtnDlgAll);
        _toggleButtons.Add(BtnDlgFait);
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