using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeoTools.Utils;

public class Tasks
{
    private static BrushConverter converter = new();
    public static Brush hexBrush(string hexColor)
    {
        return (Brush)converter.ConvertFromString(hexColor);
    }
    public static void SetElementGrid(UIElement element, int row=0, int column=0)
    {
        Grid.SetRow(element, row);
        Grid.SetColumn(element, column);
    }
    public static string GetUserSession()
    {
        return Environment.UserName;
    }

}

