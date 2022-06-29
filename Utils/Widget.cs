using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GeoTools.Model;
using Npgsql;

namespace GeoTools.Utils;

public class Widget
{
    private static User _user = MainWindow.UserSession;
    public static Button MakeBtnDlg(Dictionary<string, object> dictionary, Style style)
    {
        Thickness margin = new Thickness(2);

        TextBlock txtZoMarche = makeTextBlock(content: $"RIP{dictionary["refcode1"]}", fontSize:9);
        TextBlock txtDlInitDate = makeTextBlock(content: $"{DateTime.Parse(dictionary["date_initial"].ToString()).ToString("MM/dd/yyyy")}", fontSize:9);
        TextBlock txtExEtNom = makeTextBlock(content: $"{dictionary["nom_etat"]}", fontSize:9);

        Border bdZoMarche = MakeBorderTxt(margin: margin);
        bdZoMarche.Child = txtZoMarche;

        Border bdDlInitDate = MakeBorderTxt(margin: margin);
        bdDlInitDate.Child = txtDlInitDate;

        Border bdExEtNom = MakeBorderTxt(margin: margin);
        bdExEtNom.Child = txtExEtNom;

        StackPanel stackLabel = new StackPanel()
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Right,
            Children = { bdZoMarche, bdDlInitDate, bdExEtNom },
            Margin = new Thickness(3, 0, 0, 0),
        };
        
        TextBlock dlgInfo = new TextBlock()
        {
            Text = dictionary["dlg_infos"].ToString().Replace("|", "\n"),
            FontSize = 11,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(0, 0, 3, 0),
        };

        StackPanel stackAll = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Children = { dlgInfo, stackLabel }
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
                      $"admin={_user.Admin}\n" +
                      $"prenom={_user.Prenom}", 
            Background = Tasks.HexBrush(hexColor: dictionary["couleur_etat"].ToString())
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
    // private static Label MakeLabel(string content)
    // {
    //     return new Label()
    //     {
    //         Content = content,
    //         Background = Brushes.Transparent,
    //         HorizontalAlignment = HorizontalAlignment.Center,
    //         VerticalAlignment = VerticalAlignment.Center,
    //         FontSize = Constants.LabelFontSize,
    //         HorizontalContentAlignment = HorizontalAlignment.Center,
    //         VerticalContentAlignment = VerticalAlignment.Center,
    //     };
    // }
    private static Border MakeBorderBtn()
    {
        return new Border()
        {
            CornerRadius = new CornerRadius(5),
            Background = Brushes.White,
            BorderBrush = Brushes.White,
            Height = Constants.LabelHeighSize,
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