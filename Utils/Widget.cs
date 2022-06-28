using System;
using System.Collections.Generic;
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
        Label lbZoMarche = MakeLabel(content: $"RIP{dictionary["refcode1"]}");
        Label lbDlInitDate = MakeLabel(content: $"{DateTime.Parse(dictionary["date_initial"].ToString()).ToString("MM/dd/yyyy")}");
        Label lbExEtNom = MakeLabel(content: $"{dictionary["nom_etat"]}");

        Border bdZoMarche = MakeBorder();
        bdZoMarche.Child = lbZoMarche;

        Border bdDlInitDate = MakeBorder();
        bdDlInitDate.Child = lbDlInitDate;

        Border bdExEtNom = MakeBorder();
        bdExEtNom.Child = lbExEtNom;

        // Create the Grid
        Grid grid = new Grid()
        {
            Width = Constants.DlgWith,
            Height = Constants.DlgHeight,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            ShowGridLines = true
        };

        Grid GridLabel = new Grid()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Center 
        };
        // Define the Rows
        for (byte i = 0; i < 3; i++)
        {
            GridLabel.RowDefinitions.Add(new RowDefinition());
        }

        SetElementGrid(element: bdZoMarche, row: 0);
        SetElementGrid(element: bdDlInitDate, row: 1);
        SetElementGrid(element: bdExEtNom, row: 2);

        GridLabel.Children.Add(bdZoMarche);
        GridLabel.Children.Add(bdDlInitDate);
        GridLabel.Children.Add(bdExEtNom);
            
        for (byte i = 0; i < 2; i++)
        {
            ColumnDefinition col = new ColumnDefinition();
            if (i > 0)
            {
                col.Width = new GridLength(Constants.DlgLargeColumnWidth);
            }

            grid.ColumnDefinitions.Add(col);
        }

        // Define the Rows
        for (byte i = 0; i < 1; i++) 
        {
            //RowDefinition row = new RowDefinition();
            grid.RowDefinitions.Add(new RowDefinition());
        }

        TextBlock dlgInfo = new TextBlock()
        {
            Text = dictionary["dlg_infos"].ToString().Replace("|", "\n"),
            FontSize = Constants.TextBlockFontSize,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };

        SetElementGrid(element: dlgInfo);
        SetElementGrid(element: GridLabel, column: 1);

        grid.Children.Add(GridLabel);
        grid.Children.Add(dlgInfo);

        return new Button()
        {
            Content = grid,
            Name = $"dlg_{dictionary["id"]}",
            Style = style,
            ToolTip = $"{dictionary["dlg"]}\n" +
                      $"Etat : {dictionary["nom_etat"]} ({dictionary["code_etat"]})\n" +
                      $"ID : {dictionary["id"]}\n" +
                      $"admin={_user.Admin}\n" +
                      $"prenom={_user.Prenom}", 
            Background = Tasks.HexBrush(hexColor: dictionary["couleur_etat"].ToString())
        };
    }
    private static Label MakeLabel(string content)
    {
        return new Label()
        {
            Content = content,
            Background = Brushes.Transparent,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = Constants.LabelFontSize,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
        };
    }
    private static Border MakeBorder()
    {
        return new Border()
        {
            CornerRadius = new CornerRadius(5),
            Background = Brushes.White,
            Height = Constants.LabelHeighSize,
            Width = Constants.LabelWidthSize,
            Margin = new Thickness(5, 0, 5, 0),
        };
    }
    public static void SetElementGrid(UIElement element, int row=0, int column=0)
    {
        Grid.SetRow(element, row);
        Grid.SetColumn(element, column);
    }
}