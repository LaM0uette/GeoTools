using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace GeoTools.Views;

public partial class DlgViewAll : UserControl
{
    private readonly MyViewModel _viewModel;
    public DlgViewAll()
    {
        InitializeComponent();
        _viewModel = new MyViewModel();
    }
}

// INotifyPropertyChanged notifies the View of property changes, so that Bindings are updated.
sealed class MyViewModel : INotifyPropertyChanged
{
    
}