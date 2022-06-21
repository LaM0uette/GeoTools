using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GeoTools.Views;

public partial class DlgViewAll : UserControl
{

    public DlgViewAll()
    {
        InitializeComponent();
        CreateBtnDlg();

    }

    private void CreateBtnDlg()
    {
        var style = FindResource("ButtonDLGTemp") as Style;
        const string connectionString = "Data Source=T:\\- 4 Suivi Appuis\\25_BDD\\MyDLG\\bdd.sqlite";
        
        const byte fontsize = 8;
        const byte heighsize = 19;
        const byte widthsize = 90;

        SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        
        SQLiteCommand command = new SQLiteCommand("SELECT * FROM v_dlg_tmp", connection);
        SQLiteDataReader sqlReader = command.ExecuteReader();
        while (sqlReader.Read())
        {
            
            Label lbZoMarche = new Label()
            {
                Content = $"RIP{sqlReader["zo_marche"]}",
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontsize,
                Height = heighsize,
                Width = widthsize
            };

            Label lbDlInitDate = new Label()
            {
                Content = $"{sqlReader["dl_init_date"]}",
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontsize,
                Height = heighsize,
                Width = widthsize
            };

            // Create the Grid
            Grid grid = new Grid()
            {
                Width = 250,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true
            };

            // // Define the Columns
            for (byte i = 0; i < 2; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                if (i > 0)
                {
                    col.Width = new GridLength(widthsize);
                }
                grid.ColumnDefinitions.Add(col);
            }

            // Define the Rows
            for (byte i = 0; i < 3; i++)
            {
                RowDefinition row = new RowDefinition();
                grid.RowDefinitions.Add(row);
            }
            

            SetElementGrid(element:lbZoMarche, row:0, column:1);
            SetElementGrid(element:lbDlInitDate, row:1, column:1);
            // SetElementGrid(element:lb_nLivr_nVer, row:2, column:1);

            grid.Children.Add(lbZoMarche);
            grid.Children.Add(lbDlInitDate);
            // grid.Children.Add(lb_nLivr_nVer);

            Button button = new Button()
            {
                Content = grid,
                Name = $"dlg_{sqlReader["dl_id"]}",
                Style = style,
            };

            button.Click += new RoutedEventHandler(button_Click);
            Panel.Children.Add(button);
        }
        
        connection.Close();
    }

    private static UIElement SetLabel(string content, byte fontsize=5, byte height=15, byte width=30,HorizontalAlignment horizontalAlignment=HorizontalAlignment.Center, VerticalAlignment verticalAlignment=VerticalAlignment.Center)
    {
        return new Label()
        {
            Content = content,
            HorizontalAlignment = horizontalAlignment,
            VerticalAlignment = verticalAlignment,
            FontSize = fontsize,
            Height = height,
            Width = width
        };
    }
    
    private static void SetElementGrid(UIElement element, int row=0, int column=0)
    {
        Grid.SetRow(element, row);
        Grid.SetColumn(element, column);
    }
    
    void button_Click(object sender, RoutedEventArgs e)
    {
        //MessageBox.Show($"You clicked on the {(sender as Button).Name}"string.Format("You clicked on the {0}. button.", (sender as Button).Name));
        
        string btnName = (sender as Button).Name;
        if (btnName is not null)
        {
            MessageBox.Show($"You clicked on the {btnName}");
        }
    }
}