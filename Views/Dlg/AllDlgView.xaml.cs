using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;
using Npgsql;


namespace GeoTools.Views.Dlg;

public partial class AllDlgView
{
    #region Statements

    public static AllDlgView Instance = new();

    public AllDlgView()
    {
        Instance = this;
        InitializeComponent();
    }

    #endregion

    //

    #region Fonctions

    public void CreateDlgButtons(NpgsqlDataReader dlgCdReader)
    {
        AllDlgWrapPanel.Children.Clear();

        var dlgs = Tasks.GetListOfDlgStructs(dlgCdReader);

        foreach (var button in dlgs.Select(DlgButtons.GetButtonFromDlg))
        {
            button.Click += DlgButtons.SetActionsOnBtnDlg_Click;
            AllDlgWrapPanel.Children.Add(button);
        }
    }

    #endregion
}