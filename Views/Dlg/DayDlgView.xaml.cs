using GeoTools.Utils;
using Npgsql;


namespace GeoTools.Views.Dlg;

public partial class DayDlgView
{
    #region Statements

    public static DayDlgView Instance = new();

    public DayDlgView()
    {
        Instance = this;
        InitializeComponent();
    }

    #endregion

    //

    #region Fonctions

    public void CreateDlgButtons(NpgsqlDataReader dlgCdReader)
    {
        DayDlgWrapPanel.Children.Clear();

        var dlgStructs = Tasks.GetListOfDlgStructs(dlgCdReader);

        foreach (var dlgStruct in dlgStructs)
        {
            var button = DlgButtons.GetButtonFromDlg(dlgStruct);
            button.Click += DlgButtons.SetActionsOnBtnDlg_Click;
            DayDlgWrapPanel.Children.Add(button);
        }
    }

    #endregion
}