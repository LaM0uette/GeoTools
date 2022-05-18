using System;
using System.Windows;
using System.Windows.Controls;

namespace GeoTools.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    // private void MainView_OnLoaded(object sender, RoutedEventArgs e)
    // {
    //     Button btn = new Button();
    //     
    //     btn.Height = 30;
    //     btn.Content = "Click Me!!";
    //     btn.Width = 200;
    //     
    //     var style = FindResource("ButtonTxtInvB2") as Style;
    //     btn.Style = style;
    //     
    //     TestGrid.Children.Add(btn);
    // }
    private void BtnTiMenu_OnClick(object sender, RoutedEventArgs e)
    {
        TiExportGrace.IsSelected = true;
    }
    
    private void BtnTiExportGrace_OnClick(object sender, RoutedEventArgs e)
    {
        TiMenu.IsSelected = true;
    }
}