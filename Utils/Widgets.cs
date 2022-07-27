using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeoTools.Utils;

public static class Widgets
{
    #region Border

    public struct BorderStruct
    {
        private CornerRadius? _cornerRadius;

        public CornerRadius CornerRadius
        {
            get => _cornerRadius ?? new CornerRadius(3); 
            set => _cornerRadius = value;
        }
    }
        
    public static Border NewBorder(BorderStruct bd = new())
    {
        return new Border
        {
            CornerRadius = bd.CornerRadius,
            Background = Brushes.White,
            BorderBrush = Brushes.White,
            Height = 13,
            Width = 65,
            Margin = Constants.DlgMargin,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
        };
    }

    #endregion


}