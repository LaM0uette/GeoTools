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
        public const byte Width = 200;
        public const byte Height = 80;
        
        public const byte LeftWidth = 60;
        public const byte RightWidth = 100;
    }

    #endregion
    
    // Dlg created
    public const byte DlgWith = 200;  //300;
    public const byte DlgHeight = 60;  //120;
    public const byte DlgLargeColumnWidth = 140;
    public const byte LabelFontSize = 9;
    public const byte LabelHeightSize = 17;  //26;
    public const byte LabelWidthSize = 65;  //125;
    public const byte TextBlockFontSize = 11;  //LabelFontSize + 8;
    
    public static Thickness DlgMargin = new (2);

    //...
    // Window
    public const byte MaximizeMarge = 15;
    public const byte ScrollBarWith = 10;
    public const byte MaxScrollBarWith = ScrollBarWith + MaximizeMarge;
}
