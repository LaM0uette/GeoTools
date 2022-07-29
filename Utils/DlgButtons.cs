using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Parser;

namespace GeoTools.Utils;

public static class DlgButtons
{
    #region Fonctions

    public static Button GetButtonFromDlg(Tasks.DlgStruct dlg)
    {
        var dlgBorder = Widgets.NewDlgBorder();
        var dlgGrid = Widgets.NewDlgGrid();
        var dlgName = NewDlgNameTextBlock(dlg);
        var dlgInfos = NewDlgInfos(dlg);

        Grid.SetColumn(dlgName, 0);
        Grid.SetColumn(dlgInfos, 2);

        dlgGrid.Children.Add(dlgName);
        dlgGrid.Children.Add(dlgInfos);

        dlgBorder.Child = dlgGrid;

        return new Button
        {
            Content = dlgBorder,
            Name = $"dlg_{dlg.Id}",
            Height = Constants.Dlg.Height,
            Width = Constants.Dlg.Width,
            Margin = Constants.Dlg.Margin,
            ToolTip = $"{dlg.Dlg}\n" +
                      $"Etat : {dlg.NomEtat} ({dlg.CodeEtat})\n" +
                      $"ID : {dlg.Id}\n" +
                      $"admin={MainWindow.UserSession.Admin}\n" +
                      $"prenom={MainWindow.UserSession.Prenom}", 
            Background = Tasks.HexBrush(hexColor: dlg.CouleurEtat)
        };
    }

    #endregion

    //

    #region NewLayouts

    private static Grid NewDlgInfos(Tasks.DlgStruct dlg)
    {
        var grd = Widgets.NewDlgInfosGrid();

        var dlgInfoRefcode1 = Widgets.NewDlgInfoTextBlock(dlg.Refcode1.ParseToString());
        var dlgInfoDateInit = Widgets.NewDlgInfoTextBlock($"{dlg.DateInit:MM/dd/yyyy}");
        var dlgInfoNomEtat = Widgets.NewDlgInfoTextBlock(dlg.NomEtat);
        var dlgSeparator = Widgets.NewDlgSeparator();

        Grid.SetColumn(dlgSeparator, 0);
        Grid.SetColumn(dlgInfoRefcode1, 1);
        Grid.SetColumn(dlgInfoDateInit, 1);
        Grid.SetColumn(dlgInfoNomEtat, 1);
        
        Grid.SetRow(dlgInfoRefcode1, 0);
        Grid.SetRow(dlgInfoDateInit, 1);
        Grid.SetRow(dlgInfoNomEtat, 2);
        Grid.SetRowSpan(dlgSeparator, 3);
        
        grd.Children.Add(dlgSeparator);
        grd.Children.Add(dlgInfoRefcode1);
        grd.Children.Add(dlgInfoDateInit);
        grd.Children.Add(dlgInfoNomEtat);
        
        return grd;
    }

    private static TextBlock NewDlgNameTextBlock(Tasks.DlgStruct dlg)
    {
        var dlgName = Widgets.NewDlgInfoTextBlock(dlg.DlgInfos.Replace("|", "\n"), 16);
        dlgName.Margin = new Thickness(10, 0, 0, 0);

        return dlgName;
    }

    #endregion

    //
    
    // TODO: A SUPPRIMER!
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
            Height = Constants.Dlg.Height,
            Width = Constants.Dlg.Width,
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