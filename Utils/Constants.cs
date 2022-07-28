using System.Globalization;
using System.Windows;

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
        public const byte Height = 70;
        
        public const byte DlgNameWidth = 120;
        public const byte DlgInfosWidth = 120;
    }
    
    public static class Border
    {
        public const byte Width = 100;
        public const byte Height = 18;
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
