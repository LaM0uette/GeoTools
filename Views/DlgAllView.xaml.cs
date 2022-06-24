﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GeoTools.Utils;
using GeoTools.Model;
using Npgsql;


namespace GeoTools.Views;

public partial class DlgViewAll
{
    private User _user = MainWindow.UserSession;
    
    
    private const byte TextBlockFontSize = LabelFontSize + 8;

    private const byte LabelFontSize = 13;
    private const byte LabelHeighSize = 26;
    private const byte LabelWidthSize = 125;

    public DlgViewAll()
    {
        InitializeComponent();
        CreateBtnDlg();
    }

    private void CreateBtnDlg()
    {
        var style = FindResource("ButtonDLGTemp") as Style;

        NpgsqlDataReader cdReader = Sql.GetAllDlg();

        while (cdReader.Read())
        {
            Label lbZoMarche = MakeLabel(content: $"RIP{cdReader["refcode1"]}");
            Label lbDlInitDate =
                MakeLabel(content: $"{DateTime.Parse(cdReader["date_initial"].ToString()).ToString("MM/dd/yyyy")}");
            Label lbExEtNom = MakeLabel(content: $"{cdReader["nom_etat"]}");

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
                RowDefinition row = new RowDefinition();
                GridLabel.RowDefinitions.Add(row);
            }

            Tasks.SetElementGrid(element: bdZoMarche, row: 0);
            Tasks.SetElementGrid(element: bdDlInitDate, row: 1);
            Tasks.SetElementGrid(element: bdExEtNom, row: 2);

            GridLabel.Children.Add(bdZoMarche);
            GridLabel.Children.Add(bdDlInitDate);
            GridLabel.Children.Add(bdExEtNom);

            // // Define the Columns
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
                RowDefinition row = new RowDefinition();
                grid.RowDefinitions.Add(row);
            }

            TextBlock dlgInfo = new TextBlock()
            {
                Text = cdReader["dlg_infos"].ToString().Replace("|", "\n"),
                FontSize = TextBlockFontSize,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            Tasks.SetElementGrid(element: dlgInfo);
            Tasks.SetElementGrid(element: GridLabel, column: 1);

            grid.Children.Add(GridLabel);
            grid.Children.Add(dlgInfo);

            Button button = new Button()
            {
                Content = grid,
                Name = $"dlg_{cdReader["id"]}",
                Style = style,
                ToolTip = $"{cdReader["dlg"]}\n" +
                          $"Etat : {cdReader["nom_etat"]} ({cdReader["code_etat"]})\n" +
                          $"ID : {cdReader["id"]}\n" +
                          $"admin={_user.Admin}\n" +
                          $"prenom={_user.Prenom}",
                Background = Tasks.hexBrush(hexColor: cdReader["couleur_etat"].ToString())
            };

            button.Click += button_Click;
            Panel.Children.Add(button);
        }

        cdReader.Close();
    }

    static Border MakeBorder()
    {
        return new Border()
        {
            CornerRadius = new CornerRadius(5),
            Background = Brushes.White,
            Height = LabelHeighSize,
            Width = LabelWidthSize,
            Margin = new Thickness(5, 0, 5, 0),
        };
    }

    static Label MakeLabel(string content)
    {
        Label test = new Label()
        {
            Content = content,
            Background = Brushes.Transparent,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = LabelFontSize,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
        };
        return test;
    }

    static void button_Click(object sender, RoutedEventArgs e)
    {
        string btnName = ((Button) sender).Name;
        if (btnName is not null)
        {
            MessageBox.Show($"You clicked on the {btnName}");
        }
    }

    private void d(object sender, SizeChangedEventArgs e)
    {
    }
}