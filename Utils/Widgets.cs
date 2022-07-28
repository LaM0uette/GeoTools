using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeoTools.Utils;

public static class Widgets
{
    #region Border

    public static Border NewBorder()
    {
        return new Border
        {
            CornerRadius = new CornerRadius(4),
            Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            Width = Constants.Border.Width,
            Height = Constants.Border.Height,
            Margin = new Thickness(0, 0, 3, 0),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center
        };
    }

    #endregion
    
    //
    
    #region Border

    public static TextBlock NewTextBlock(string content,int fontSize = 10)
    {
        return new TextBlock
        {
            Text = content,
            FontSize = fontSize,
            TextAlignment = TextAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            Foreground = new SolidColorBrush(Color.FromRgb(25, 25, 25)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center
        };
    }

    #endregion
    
    //
    
    #region Layouts

    public static Grid NewDlgGrid()
    {
        var grd = new Grid{Width = Constants.Dlg.Width};

        grd.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(0, GridUnitType.Auto), MaxWidth = Constants.Dlg.DlgNameWidth});
        grd.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(1, GridUnitType.Star)});
        grd.ColumnDefinitions.Add(new ColumnDefinition{Width = new GridLength(Constants.Dlg.DlgInfosWidth)});

        return grd;
    }
    
    public static Grid NewDlgInfosGrid()
    {
        var grd = new Grid{Height = Constants.Dlg.Height};

        grd.RowDefinitions.Add(new RowDefinition{Height = new GridLength(1, GridUnitType.Star)});
        grd.RowDefinitions.Add(new RowDefinition{Height = new GridLength(1, GridUnitType.Star)});
        grd.RowDefinitions.Add(new RowDefinition{Height = new GridLength(1, GridUnitType.Star)});
        grd.RowDefinitions.Add(new RowDefinition{Height = new GridLength(5)});

        return grd;
    }

    #endregion


}