using System.Windows;
using System.Windows.Controls;

namespace GeoTools.Views;

public partial class MainView
{
    public static TabItem Te = new ();
    public static TabItem Teb = new ();

    public MainView()
    {
        InitializeComponent();
        Te = TiExportGrace;
        Teb = TiMenu;
    }
    
    public static void Test(TabItem t)
    {
        t.IsSelected = true;
    }

    private void BtnTiMenu_OnClick(object sender, RoutedEventArgs e)
    {
        Te.IsSelected = true;
    }
}