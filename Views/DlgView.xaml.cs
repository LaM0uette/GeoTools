using System.Windows;

namespace GeoTools.Views;

public partial class DlgView
{
    public DlgView()
    {
        InitializeComponent();
        setWith();
    }

    private void setWith()
    {
        DlgViewAll.Width = Application.Current.MainWindow.Width - this.BorderTest.Width;
    }
    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        // MainView.TiMenu.IsSelected = true;
    }
}
