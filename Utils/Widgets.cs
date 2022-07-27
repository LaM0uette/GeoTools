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

        public int CornerRadius { get => _cornerRadius ?? 1; set => _cornerRadius = value; }
        public (byte, byte, byte) Background { get => _background ?? (255, 255, 255); set => _background = value; }
        public (byte, byte, byte) BorderBrush { get => _borderBrush ?? (255, 255, 255); set => _borderBrush = value; }
        public int Width { get => _width ?? 65; set => _width = value; }
        public int Height { get => _height ?? 12; set => _height = value; }
        public Thickness Margin { get => _margin ?? new Thickness(3); set => _margin = value; }
        public HorizontalAlignment HorizontalAlignment { get => _horizontalAlignment ?? HorizontalAlignment.Right; set => _horizontalAlignment = value; }
        public VerticalAlignment VerticalAlignment { get => _verticalAlignment ?? VerticalAlignment.Center; set => _verticalAlignment = value; }
    }
        
    public static Border NewBorder(BorderStruct strct = new())
    {
        return new Border
        {
            CornerRadius = new CornerRadius(strct.CornerRadius),
            Background = new SolidColorBrush(Color.FromRgb(strct.Background.Item1, strct.Background.Item2, strct.Background.Item3)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(strct.BorderBrush.Item1, strct.BorderBrush.Item2, strct.BorderBrush.Item3)),
            Width = strct.Width,
            Height = strct.Height,
            Margin = strct.Margin,
            HorizontalAlignment = strct.HorizontalAlignment,
            VerticalAlignment = strct.VerticalAlignment,
        };
    }

    #endregion
    
    //
    
    #region Border

    public struct TextBlockStruct
    {
        private TextAlignment? _textAlignment;
        private TextWrapping? _textWrapping;
        private (byte, byte, byte)? _foreground;
        private int? _width;
        private int? _height;
        private Thickness? _margin;
        private HorizontalAlignment? _horizontalAlignment;
        private VerticalAlignment? _verticalAlignment;

        public TextAlignment TextAlignment { get => _textAlignment ?? TextAlignment.Center; set => _textAlignment = value; }
        public TextWrapping TextWrapping { get => _textWrapping ?? TextWrapping.Wrap; set => _textWrapping = value; }
        public (byte, byte, byte) Foreground { get => _foreground ?? (25, 25, 25); set => _foreground = value; }
        public int Width { get => _width ?? 65; set => _width = value; }
        public int Height { get => _height ?? 12; set => _height = value; }
        public Thickness Margin { get => _margin ?? new Thickness(0); set => _margin = value; }
        public HorizontalAlignment HorizontalAlignment { get => _horizontalAlignment ?? HorizontalAlignment.Center; set => _horizontalAlignment = value; }
        public VerticalAlignment VerticalAlignment { get => _verticalAlignment ?? VerticalAlignment.Center; set => _verticalAlignment = value; }
    }
        
    public static TextBlock NewTextBlock(string content,int fontSize = 10, TextBlockStruct strct = new())
    {
        return new TextBlock
        {
            Text = content,
            FontSize = fontSize,
            TextAlignment = strct.TextAlignment,
            TextWrapping = strct.TextWrapping,
            Foreground = new SolidColorBrush(Color.FromRgb(strct.Foreground.Item1, strct.Foreground.Item2, strct.Foreground.Item3)),
            Width = strct.Width,
            Height = strct.Height,
            Margin = strct.Margin,
            HorizontalAlignment = strct.HorizontalAlignment,
            VerticalAlignment = strct.VerticalAlignment,
        };
    }

    #endregion


}