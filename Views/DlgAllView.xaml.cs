using System;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace GeoTools.Views;

public partial class DlgViewAll : UserControl
{
    // private readonly MyViewModel _viewModel;
    

    public DlgViewAll()
    {
        InitializeComponent();

        // _MyViewModel = new MyViewModel();
        // DataContext = _viewModel;

        string connectionString = "Data Source=T:\\- 4 Suivi Appuis\\25_BDD\\MyDLG\\bdd.sqlite";


    }
}