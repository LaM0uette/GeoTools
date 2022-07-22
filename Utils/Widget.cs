using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeoTools.Utils;

public class Widget
{
    public static Button MakeBtnDlg(Dictionary<string, object> dictionary, Style style)
    {
        Thickness margin = new (2);

        var txtZoMarche = makeTextBlock(content: $"RIP{dictionary["refcode1"]}", fontSize:9);
        var txtDlInitDate = makeTextBlock(content: $"{DateTime.Parse(dictionary["date_initial"].ToString()!):MM/dd/yyyy}", fontSize:9);
        var txtExEtNom = makeTextBlock(content: $"{dictionary["nom_etat"]}", fontSize:9);

        var bdZoMarche = MakeBorderTxt(margin: margin);
        bdZoMarche.Child = txtZoMarche;

        var bdDlInitDate = MakeBorderTxt(margin: margin);
        bdDlInitDate.Child = txtDlInitDate;

        var bdExEtNom = MakeBorderTxt(margin: margin);
        bdExEtNom.Child = txtExEtNom;

        var stackLabel = new StackPanel()
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Right,
            Children = { bdZoMarche, bdDlInitDate, bdExEtNom },
            Margin = new Thickness(3, 0, 0, 0),
        };
        
        var dlgInfo = new TextBlock()
        {
            Text = dictionary["dlg_infos"].ToString()!.Replace("|", "\n"),
            FontSize = 11,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(0, 0, 3, 0),
            Foreground = Brushes.White,
        };

        Separator separator = new ()
        {
            Width = 16,
            Background = Brushes.Transparent
        };

        var stackAll = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Children = { dlgInfo, separator, stackLabel }
        };

        return new Button()
        {
            Content = stackAll,
            Height = Constants.DlgHeight,
            Width = Constants.DlgWith,
            Margin = new Thickness(5),
            Name = $"dlg_{dictionary["id"]}",
            //Style = style,
            ToolTip = $"{dictionary["dlg"]}\n" +
                      $"Etat : {dictionary["nom_etat"]} ({dictionary["code_etat"]})\n" +
                      $"ID : {dictionary["id"]}\n" +
                      $"admin={MainWindow.UserSession.Admin}\n" +
                      $"prenom={MainWindow.UserSession.Prenom}", 
            Background = Tasks.HexBrush(hexColor: dictionary["couleur_etat"].ToString()!)
        };
    }

    private static TextBlock makeTextBlock(string content, int fontSize)
    {
        return new TextBlock()
        {
            Text = content,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Foreground = Brushes.Black,
            FontSize = fontSize
        };
    }
    private static Border MakeBorderBtn()
    {
        return new Border()
        {
            CornerRadius = new CornerRadius(5),
            Background = Brushes.White,
            BorderBrush = Brushes.White,
            Height = Constants.LabelHeightSize,
            Width = Constants.LabelWidthSize,
            Margin = new Thickness(5, 0, 5, 0),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
        };
    }
    private static Border MakeBorderTxt(Thickness margin)
    {
        return new Border()
        {
            CornerRadius = new CornerRadius(3),
            Background = Brushes.White,
            BorderBrush = Brushes.White,
            Height = 13,
            Width = 65,
            Margin = margin,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
        };
    }
    public static void SetElementGrid(UIElement element, int row=0, int column=0)
    {
        Grid.SetRow(element, row);
        Grid.SetColumn(element, column);
    }
}