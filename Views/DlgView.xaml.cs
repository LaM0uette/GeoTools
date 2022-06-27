using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;

namespace GeoTools.Views;

public partial class DlgView
{
    
    public static TabItem VTabItemDlgAll = new ();
    public static TabItem VTabItemDlgMonth = new ();
    
    public DlgView()
    {
        InitializeComponent();
        
        VTabItemDlgAll = TabItemDlgAll;
        VTabItemDlgMonth = TabItemDlgMonth;
        
        SetTabItem(VTabItemDlgMonth);
        
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.SizeChanged += OnSizeChanged;
    }
    
    public static void SetTabItem(TabItem tabItem)
    {
        tabItem.IsSelected = true;
    }
    
    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        MainView.SetTabItem(MainView.VTabItemMenu);
    }
    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        double maxWith = GetMaxWidth();
        
        DlgViewAll.Width = maxWith;
        //DlgViewMonth.Width = maxWith;
    }

    private double GetMaxWidth()
    {
        return Tasks.GetWindowSize() - DlgLegend.Width - 
               (Tasks.GetWindowState() == WindowState.Maximized?Constants.MaximazeScrollBarWith:Constants.NormalScrollBarWith);
    }
}