using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeoTools.Utils;

public static class Widgets
{
    #region Border

    public struct BorderStruct
    {
        private int? _cornerRadius;
        private (byte, byte, byte)? _background;
        private (byte, byte, byte)? _borderBrush;
        private int? _width;
        private int? _height;
        private Thickness? _margin;
        private HorizontalAlignment? _horizontalAlignment;
        private VerticalAlignment? _verticalAlignment;

        public int CornerRadius { get => _cornerRadius ?? 3; set => _cornerRadius = value; }
        public (byte, byte, byte) Background { get => _background ?? (10, 10, 10); set => _background = value; }
        public (byte, byte, byte) BorderBrush { get => _borderBrush ?? (10, 10, 10); set => _borderBrush = value; }
        public int Width { get => _width ?? 15; set => _width = value; }
        public int Height { get => _height ?? 65; set => _height = value; }
        public Thickness Margin { get => _margin ?? new Thickness(3); set => _margin = value; }
        public HorizontalAlignment HorizontalAlignment { get => _horizontalAlignment ?? HorizontalAlignment.Right; set => _horizontalAlignment = value; }
        public VerticalAlignment VerticalAlignment { get => _verticalAlignment ?? VerticalAlignment.Center; set => _verticalAlignment = value; }
    }
        
    public static Border NewBorder(BorderStruct bd = new())
    {
        return new Border
        {
            CornerRadius = new CornerRadius(bd.CornerRadius),
            Background = new SolidColorBrush(Color.FromRgb(bd.Background.Item1, bd.Background.Item2, bd.Background.Item3)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(bd.BorderBrush.Item1, bd.BorderBrush.Item2, bd.BorderBrush.Item3)),
            Height = bd.Height,
            Width = bd.Width,
            Margin = bd.Margin,
            HorizontalAlignment = bd.HorizontalAlignment,
            VerticalAlignment = bd.VerticalAlignment,
        };
    }

    #endregion
    
    //


}