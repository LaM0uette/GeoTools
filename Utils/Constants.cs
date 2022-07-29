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
        public const byte Height = 80;
        public static Thickness Margin => new (5);
        
        public const byte DlgNameMaxWidth = Width-Height;
        public const byte DlgInfosMaxWidth = Height;
    }
    
    public static class Colors
    {
        public static SolidColorBrush White = new (Color.FromRgb(255, 255, 255));
    }

    #endregion

    public const byte LabelFontSize = 9;
    public const byte LabelHeightSize = 17;  //26;
    public const byte LabelWidthSize = 65;  //125;

    //...
    // Window
    public const byte MaximizeMarge = 15;
    public const byte ScrollBarWith = 10;
    public const byte MaxScrollBarWith = ScrollBarWith + MaximizeMarge;
}
