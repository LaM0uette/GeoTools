using System.Windows;
using System.Windows.Controls;

namespace GeoTools.Views;

public partial class MainView
{
    private static TabItem Te;
    private static TabItem Teb;
    
    public MainView()
    {
        InitializeComponent();
        
        Te = TiExportGrace;
        Teb = TiMenu;
    }
    
    public static void Test()
    {
        Te.IsSelected = true;
    }

    private void BtnTiMenu_OnClick(object sender, RoutedEventArgs e)
    {
        Teb.IsSelected = true;
    }
}