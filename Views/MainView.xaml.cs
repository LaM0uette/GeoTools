using System.Windows;
using System.Windows.Controls;

namespace GeoTools.Views;

public partial class MainView
{
    public static TabItem VTiExportGrace = new ();
    public static TabItem VTiMenu = new ();

    public MainView()
    {
        InitializeComponent();
        VTiExportGrace = TiExportGrace;
        VTiMenu = TiMenu;
    }
    
    public static void SetTabItem(TabItem t)
    {
        t.IsSelected = true;
    }

    private void BtnTiMenu_OnClick(object sender, RoutedEventArgs e)
    {
        SetTabItem(VTiExportGrace);
    }
}
