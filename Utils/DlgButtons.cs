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
        var dlgCounterBorder = Widgets.NewDlgCounterBorder();
        var dlgBorder = Widgets.NewDlgBorder();
        var dlgGrid = Widgets.NewDlgGrid();
        var dlgName = NewDlgNameTextBlock(dlg);
        var dlgInfos = NewDlgInfos(dlg);

        Grid.SetColumn(dlgName, 0);
        Grid.SetColumn(dlgInfos, 2);

        dlgGrid.Children.Add(dlgName);
        dlgGrid.Children.Add(dlgInfos);

        dlgBorder.Child = dlgGrid;
        dlgCounterBorder.Child = dlgBorder;

        return new Button
        {
            Content = dlgCounterBorder,
            Name = $"dlg_{dlg.Id}",
            Height = Constants.Dlg.Height,
            Width = Constants.Dlg.Width,
            Margin = Constants.Dlg.Margin,
            ToolTip = $"{dlg.Dlg}\n" +
                      $"Etat : {dlg.NomEtat} ({dlg.CodeEtat})\n" +
                      $"ID : {dlg.Id}\n" +
                      $"admin={MainWindow.UserSession.Admin}\n" +
                      $"prenom={MainWindow.UserSession.Prenom}" +
                      $"date={dlg.DateInit}", 
            Background = Tasks.HexBrush(hexColor: dlg.CouleurEtat)
        };
    }
    
    public static Button GetButtonFromMonthDlg(Tasks.DlgStruct dlg)
    {
        var dlgName = Widgets.NewDlgInfoTextBlock(dlg.Dlg);

        return new Button
        {
            Content = dlgName,
            Name = $"dlg_{dlg.Id}",
            Margin = Constants.Dlg.Margin,
            ToolTip = $"{dlg.Dlg}\n" +
                      $"Etat : {dlg.NomEtat} ({dlg.CodeEtat})\n" +
                      $"ID : {dlg.Id}\n" +
                      $"admin={MainWindow.UserSession.Admin}\n" +
                      $"prenom={MainWindow.UserSession.Prenom}" +
                      $"date={dlg.DateInit}", 
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
        var dlgInfoDateInit = Widgets.NewDlgInfoTextBlock($"{dlg.DateInit:dd/MM/yyyy}");
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
}