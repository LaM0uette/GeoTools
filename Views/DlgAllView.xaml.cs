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
    // private readonly MyViewModel _viewModel;
    public ICommand Command {get;set;}
    public string DisplayName {get;set;}

    public DlgViewAll()
    {
        InitializeComponent();

        // _MyViewModel = new MyViewModel();
        // DataContext = _viewModel;

        var style = FindResource("ButtonTxtInvB2") as Style;
        string connectionString = "Data Source=T:\\- 4 Suivi Appuis\\25_BDD\\MyDLG\\bdd.sqlite";

        SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();


        SQLiteCommand command = new SQLiteCommand("SELECT * FROM v_dlg", connection);
        SQLiteDataReader sqlReader = command.ExecuteReader();
        while (sqlReader.Read())
        {
            Button button = new Button()
            {
                Content = sqlReader["dlg"].ToString(),
                Height = 50,
                Name = $"btn_{sqlReader["dl_id"]}",
                Style = style,
            };

            button.Click += new RoutedEventHandler(button_Click);
            Panel.Children.Add(button);
        }
        
        connection.Close();
        

        // DataTable dataTable = new DataTable();
        // dataTable.Load(command.ExecuteReader());
        // connection.Close();
        //
        //
        //
        // for (int i = 0; i < 100; i++)
        // {
        //     Button button = new Button()
        //     {
        //         Content = $"Button for {i}",
        //         Height = 50,
        //         Name = $"jeSuisUnTest{i}",
        //         Style = style,
        //     };
        //
        //     button.Click += new RoutedEventHandler(button_Click);
        //     Panel.Children.Add(button);
        // }
        //
        // foreach (var i in dataTable.Rows)
        // {
        //     
        // }

        // dataGrid.DataContext = dataTable;
        
    }
    
    
    void button_Click(object sender, RoutedEventArgs e)
    {
        Console.WriteLine(string.Format("You clicked on the {0}. button.", (sender as Button).Tag));
        MessageBox.Show(string.Format("You clicked on the {0}. button.", (sender as Button).Name));
    }
}