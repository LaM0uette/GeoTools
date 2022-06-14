using System.Windows;

namespace GeoTools.Views;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();
    }

    private void BtnTiMenu_OnClick(object sender, RoutedEventArgs e)
    {
        TiExportGrace.IsSelected = true;
    }
}