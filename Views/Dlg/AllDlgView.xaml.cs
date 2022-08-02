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

    public async Task CreateDlgButtons(NpgsqlDataReader dlgCdReader)
    {
        AllDlgWrapPanel.Children.Clear();

        var dlgStructs = Tasks.GetListOfDlgStructsB(dlgCdReader);

        await foreach (var dlgStruct in dlgStructs)
        {
            var button = DlgButtons.GetButtonFromDlg(dlgStruct);
            button.Click += DlgButtons.SetActionsOnBtnDlg_Click;
            AllDlgWrapPanel.Children.Add(button);
        }
    }

    #endregion
}