using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;
using Npgsql;


namespace GeoTools.Views;

public partial class DlgAllView
{
    public static DlgAllView InstanceDlgAllView = new();
    
    public DlgAllView()
    {
        InstanceDlgAllView = this;
        
        InitializeComponent();
        CreateBtnDlgAll();
    }

    //
    // Functions
    public void CreateBtnDlgAll(string mode="TogBtnDlgAll")
    {
        var style = FindResource("ButtonDLGTemp") as Style;

        NpgsqlDataReader cdReader = mode switch
        {
            "TogBtnDlgAll" => Sql.GetAllDlg(),
            "TogBtnDlgAFaire" => Sql.GetAllDlgFiltered(1),
            "TogBtnDlgFait" => Sql.GetAllDlgFiltered(2),
            _ => Sql.GetAllDlg()
        };

        DlgAllPanel.Children.Clear();
        
        while (cdReader.Read())
        {
            if (style == null) continue;
            Button button = Widget.MakeBtnDlg(dictionary: Tasks.sqlDict(cdReader: cdReader), style:style);
            button.Click += BtnDlgAll_Click;
            DlgAllPanel.Children.Add(button);
        }

        cdReader.Close();
    }
    
    //
    // Actions
    private static void BtnDlgAll_Click(object sender, RoutedEventArgs e)
    {
        var btnName = ((Button) sender).Name;
        if (btnName is not null)
        {
            MessageBox.Show($"Tu as cliqué sur le bouton : {btnName}");
        }
    }
}