using System.Globalization;

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
    
    //...
    // Dlg created
    public const byte DlgWith = 177;  //300;
    public const byte DlgHeight = 50;  //120;
    public const byte DlgLargeColumnWidth = 140;
    public const byte LabelFontSize = 9;
    public const byte LabelHeightSize = 17;  //26;
    public const byte LabelWidthSize = 65;  //125;
    public const byte TextBlockFontSize = 11;  //LabelFontSize + 8;

    //...
    // Window
    public const byte MaximizeMarge = 15;
    public const byte ScrollBarWith = 10;
    public const byte MaxScrollBarWith = ScrollBarWith + MaximizeMarge;
}
