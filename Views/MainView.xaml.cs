using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;

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

    private void BtnTiMenu_OnClick(object sender, RoutedEventArgs e)
    {
        Tasks.SetTabItem(VTabItemExportGrace);
    }
}
