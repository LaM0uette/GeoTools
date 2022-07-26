using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;

namespace GeoTools.Views;

public partial class MainView
{
    #region Statements

    public static TabItem VTabItemExportGrace { get; set; } = new ();
    public static TabItem VTabItemMenu { get; set; } = new ();

    #endregion

    //

    #region Fonctions

    public MainView()
    {
        InitializeComponent();
        SetTabItems();
    }

    private void SetTabItems()
    {
        VTabItemExportGrace = TabItemExportGrace;
        VTabItemMenu = TabItemMenu;
    }

    #endregion

    //

    #region Actions

    private void BtnTiMenu_OnClick(object sender, RoutedEventArgs e)
    {
        Tasks.SetCurrentTabItem(VTabItemExportGrace);
    }

    #endregion
}
