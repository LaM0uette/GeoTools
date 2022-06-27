using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GeoTools.Views;

public partial class DlgViewSelect : UserControl
{
    private List<ToggleButton> toggleButtons = new();
    
    public DlgViewSelect()
    {
        InitializeComponent();
        FillListBtn();
    }

    private void FillListBtn()
    {
        toggleButtons.Add(BtnDlgTraiter);
        toggleButtons.Add(BtnDlgAll);
        toggleButtons.Add(BtnDlgFait);
    }
    
    void Toggle_OnClick(object sender, RoutedEventArgs e)
    {
        string btnName = ((ToggleButton) sender).Name;
        string mode = $"{((ToggleButton) sender).Content}";

        DlgViewAll.InstanceDlgViewAll.CreateBtnDlgAll(mode:mode);

        foreach (ToggleButton btn in toggleButtons)
        {
            if (btnName != btn.Name)
            {
                btn.IsChecked = false;
            }
            else
            {
                btn.IsChecked = true;
            }
        }
    }
}