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

        var cdReader = mode switch
        {
            "TogBtnDlgAll" => Sql.Get(Req.AllDlg()),
            "TogBtnDlgAFaire" => Sql.Get(Req.AllDlgFiltered(1)), 
            "TogBtnDlgFait" => Sql.Get(Req.AllDlgFiltered(2)),
            _ => Sql.Get(Req.AllDlg())
        };

        DlgAllPanel.Children.Clear();
        
        while (cdReader.Read())
        {
            if (style == null) continue;
            
            var button = Widget.MakeBtnDlg(dictionary: Tasks.SqlDict(cdReader), style:style);
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