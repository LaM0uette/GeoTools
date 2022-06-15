using System.Windows;

namespace GeoTools.Views;

public partial class DlgView
{
    public DlgView()
    {
        InitializeComponent();
        SetWith();
    }

    private void SetWith()
    {
        DlgViewAll.Width = Application.Current.MainWindow.Width - BorderTest.Width;
    }
    
    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        //MainView.TiMenu.IsSelected = true;
        MainView.Test();
    }
}
