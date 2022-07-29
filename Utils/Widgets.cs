using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeoTools.Utils;

public static class Widgets
{
    #region Border

    public static Border NewDlgBorder() => new()
    {
        BorderBrush = Constants.Colors.White,
        BorderThickness = new Thickness(0, 0, 0, 3)
    };

    #endregion

    //

    #region Border

    public static TextBlock NewDlgInfoTextBlock(string content, int fontSize = 12) => new ()
    {
        Text = content,
        FontSize = fontSize,
        TextAlignment = TextAlignment.Center,
        TextWrapping = TextWrapping.Wrap,
        Foreground = Constants.Colors.White,
        Margin = new Thickness(5, 0, 5, 0),
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Center
    };
    
    public static Border NewDlgSeparator() => new ()
    {
        Width = 2,
        CornerRadius = new CornerRadius(1),
        Height = Constants.Dlg.Height-15,
        Background = Constants.Colors.White
    };

    #endregion

    //

    #region Layouts

    public static Grid NewDlgGrid()
    {
        var grd = new Grid {Width = Constants.Dlg.Width};

        grd.ColumnDefinitions.Add(new ColumnDefinition
        {
            Width = new GridLength(0, GridUnitType.Auto), 
            MaxWidth = Constants.Dlg.DlgNameMaxWidth
        });
        
        grd.ColumnDefinitions.Add(new ColumnDefinition
        {
            Width = new GridLength(1, GridUnitType.Star)
        });
        
        grd.ColumnDefinitions.Add(new ColumnDefinition
        {
            Width = new GridLength(0, GridUnitType.Auto), 
            MaxWidth = Constants.Dlg.DlgInfosMaxWidth
        });

        return grd;
    }

    public static Grid NewDlgInfosGrid()
    {
        var grd = new Grid {Height = Constants.Dlg.Height};

        grd.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
        grd.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
        grd.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
        grd.RowDefinitions.Add(new RowDefinition {Height = new GridLength(5)});

        grd.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(0, GridUnitType.Auto), MaxWidth = 2});
        grd.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});

        return grd;
    }

    #endregion
}