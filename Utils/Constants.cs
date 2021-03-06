using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace GeoTools.Utils;

public static class Constants
{
    #region Config

    public static readonly CultureInfo LangFr = CultureInfo.CreateSpecificCulture("fr-FR");

    #endregion
    
    //

    #region Enum
    
    public enum TogBtn
    {
        Tout,
        AFaire,
        Fait
    }

    public enum WeekDays
    {
        Lundi,
        Mardi,
        Mercredi,
        Jeudi,
        Vendredi,
    }
    

    #endregion
    
    //

    #region Structs

    public static class Dlg
    {
        public const byte Width = 240;
        public const byte Height = 90;
        public const byte MonthWidth = 220;
        public static Thickness Margin => new (5);
        
        public const byte DlgNameMaxWidth = Width-Height;
        public const byte DlgInfosMaxWidth = Height;
    }
    
    public static class Colors
    {
        public static SolidColorBrush White => new (Color.FromRgb(255, 255, 255));
        public static SolidColorBrush Gray => new (Color.FromRgb(143, 143, 163));
        public static SolidColorBrush Red => new (Color.FromRgb(229, 56, 67));
    }

    #endregion
    
    //

    #region Var

    public static TogBtn CurrentState = TogBtn.Tout;
    public static int Year { get; set; } = DateTime.Now.Year;
    public static byte Month { get; set; } = (byte)DateTime.Now.Month;
    public static byte Week { get; set; } = (byte)Tasks.GetWeekNumber(DateTime.Now);
    public static byte Day { get; set; } = (byte)DateTime.Now.Day;

    #endregion
}
