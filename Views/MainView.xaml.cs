using System.Windows;
using System.Windows.Controls;
using GeoTools.Model;

namespace GeoTools.Views;

public partial class MainView
{
    public static TabItem VTabItemExportGrace = new ();
    public static TabItem VTabItemMenu = new ();

    public MainView()
    {
        InitializeComponent();
        
        VTabItemExportGrace = TabItemExportGrace;
        VTabItemMenu = TabItemMenu;
    }
    
    public static void SetTabItem(TabItem tabItem)
    {
        tabItem.IsSelected = true;
    }

    private void BtnTiMenu_OnClick(object sender, RoutedEventArgs e)
    {
        SetTabItem(VTabItemExportGrace);
    }
}
