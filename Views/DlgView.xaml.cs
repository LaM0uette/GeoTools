using System.Windows;
using GeoTools.Utils;

namespace GeoTools.Views;

public partial class DlgView
{
    public DlgView()
    {
        InitializeComponent();
        SetDlgViewWith();
    }

    private void SetDlgViewWith()
    {
        DlgViewAll.Width = Tasks.GetWindowSize() - DlgLegend.Width - Constants.ScrollBarWith;
    }

    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        MainView.SetTabItem(MainView.VTabItemMenu);
    }
}