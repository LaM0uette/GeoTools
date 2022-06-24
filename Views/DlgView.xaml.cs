using System;
using System.Windows;
using GeoTools.Utils;
using Npgsql.Replication.TestDecoding;

namespace GeoTools.Views;

public partial class DlgView
{
    private const byte ScrollBarreWith = 10;
    public DlgView()
    {
        InitializeComponent();
        SetWith();
    }

    public void SetWith()
    {
        DlgViewAll.Width = Tasks.GetWindowSize() - DlgLegend.Width - ScrollBarreWith;
    }

    private void BtnDlgBackHome_OnClick(object sender, RoutedEventArgs e)
    {
        MainView.SetTabItem(MainView.VTiMenu);
    }
}
