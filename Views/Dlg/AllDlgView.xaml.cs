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
    private static void BtnDlgAll_Click(object sender, RoutedEventArgs e)
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

    public void CreateBtnDlgAll(NpgsqlDataReader cdReader)
    {
        var style = GetDlgTempStyle();
        if (style is null) return;
        
        DlgAllPanel.Children.Clear();

        while (cdReader.Read())
        {
            var button = Widget.MakeBtnDlg(dictionary: Tasks.SqlDict(cdReader), style: style);
            button.Click += BtnDlgAll_Click;
            DlgAllPanel.Children.Add(button);
        }

        cdReader.Close();
    }

    private Style? GetDlgTempStyle() => FindResource("ButtonDLGTemp") as Style;

    #endregion
}