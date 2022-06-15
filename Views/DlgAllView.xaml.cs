using System;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        var style = FindResource("ButtonTxtInvB2") as Style;
        string connectionString = "Data Source=T:\\- 4 Suivi Appuis\\25_BDD\\MyDLG\\bdd.sqlite";

        SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        
        SQLiteCommand command = new SQLiteCommand("SELECT * FROM v_dlg", connection);
        SQLiteDataReader sqlReader = command.ExecuteReader();
        while (sqlReader.Read())
        {

            Grid grid = new Grid()
            {
                Width = 200,
                Height = 150,
            };
            grid.RowDefinitions.Count = 2;
            
            
            Button button = new Button()
            {
                Name = $"dlg_{sqlReader["dl_id"]}"
            };
            
            
            grid.

            button.Click += new RoutedEventHandler(button_Click);
            Panel.Children.Add(button);
        }
        
        connection.Close();
    }
    
    void button_Click(object sender, RoutedEventArgs e)
    {
        //MessageBox.Show($"You clicked on the {(sender as Button).Name}"string.Format("You clicked on the {0}. button.", (sender as Button).Name));
        MessageBox.Show($"You clicked on the {(sender as Button).Name}");
    }
}