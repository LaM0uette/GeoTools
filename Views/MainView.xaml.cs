using System.Windows;
using System.Windows.Controls;

namespace GeoTools.Views;

public partial class MainView
{
    private static TabItem _te = new ();
    private static TabItem _teb = new ();

    public MainView()
    {
        InitializeComponent();
        _te = TiExportGrace;
        _teb = TiMenu;
    }
    
    public static void Test()
    {
        _teb.IsSelected = true;
    }

    private void BtnTiMenu_OnClick(object sender, RoutedEventArgs e)
    {
        _te.IsSelected = true;
    }
}