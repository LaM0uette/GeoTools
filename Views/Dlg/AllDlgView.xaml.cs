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

    public void CreateAllDlgButtons(NpgsqlDataReader dlgCdReader)
    {
        var style = GetDlgTempStyle();
        if (style is null) return;
        
        AllDlgPanel.Children.Clear();

        var dlgStructs = Tasks.GetListOfDlgStructs(dlgCdReader);

        foreach (var dlgStruct in dlgStructs)
        {
            var button = Widget.GetButtonFromDlg();
            button.Click += SetActionsOnBtnDlg_Click;
            AllDlgPanel.Children.Add(button);
        }

        // while (dlgCdReader.Read())
        // {
        //     var button = Widget.MakeBtnDlg(dictionary: Tasks.SqlDict(dlgCdReader), style: style);
        //     button.Click += SetActionsOnBtnDlg_Click;
        //     AllDlgPanel.Children.Add(button);
        // }
        //
        // dlgCdReader.Close();
    }

    private Style? GetDlgTempStyle() => FindResource("ButtonDLGTemp") as Style;

    #endregion
}