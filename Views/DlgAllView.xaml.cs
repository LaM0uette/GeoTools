using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace GeoTools.Views;

public partial class DlgViewAll : UserControl
{
    private readonly MyViewModel _viewModel;
    
    public string Dlg { get; set; }
    public DlgViewAll()
    {
        InitializeComponent();

        _MyViewModel = new MyViewModel();
    }
}