using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;
using Npgsql;

namespace GeoTools.Views.Dlg;

public partial class MonthDlgView
{
    #region Statements

    public static MonthDlgView Instance = new();
    
    public MonthDlgView()
    {
        Instance = this;
        InitializeComponent();
    }

    #endregion

    //
    
    #region Actions

    // TODO: A MODIFIER !
    private static void SetActionsOnBtnDlg_Click(object sender, RoutedEventArgs e)
    {
        var btnName = ((Button) sender).Name;
        
        if (btnName is not null)
        {
            MessageBox.Show($"Tu as cliqué sur le bouton : {btnName}");
        }
    }

    #endregion
    
    //

    #region Fonctions

    public void CreateDlgButtons(NpgsqlDataReader dlgCdReader)
    {
        MonthGrid.Children.Clear();

        var dlgStructs = Tasks.GetListOfDlgStructs(dlgCdReader);

        foreach (var dlgStruct in dlgStructs)
        {
            var button = DlgButtons.GetButtonFromDlg(dlgStruct);
            button.Click += SetActionsOnBtnDlg_Click;
            AllDlgPanel.Children.Add(button);
        }
    }

    #endregion
}