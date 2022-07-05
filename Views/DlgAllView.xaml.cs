using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;
using Npgsql;


namespace GeoTools.Views;

public partial class DlgViewAll
{
    public static DlgViewAll InstanceDlgViewAll;
    public static string YoLeBolos = "TrouDuCul";
    
    public DlgViewAll()
    {
        InstanceDlgViewAll = this;
        InitializeComponent();

        CreateBtnDlgAll();
    }

    public void CreateBtnDlgAll(string mode="TOUT")
    {
        var style = FindResource("ButtonDLGTemp") as Style;

        NpgsqlDataReader cdReader = mode switch
        {
            "A TRAITER" => Sql.GetAllDlgFiltered(1),
            "TOUT" => Sql.GetAllDlg(),
            "FAIT" => Sql.GetAllDlgFiltered(2),
            _ => Sql.GetAllDlg()
        };

        DlgAllPanel.Children.Clear();
        
        while (cdReader.Read())
        {
            if (style == null) continue;
            Button button = Widget.MakeBtnDlg(dictionary: Tasks.sqlDict(cdReader: cdReader), style:style);
            button.Click += btnDlgAll_Click;
            DlgAllPanel.Children.Add(button);
        }

        cdReader.Close();
    }
    static void btnDlgAll_Click(object sender, RoutedEventArgs e)
    {
        string btnName = ((Button) sender).Name;
        if (btnName is not null)
        {
            MessageBox.Show($"You clicked on the {btnName}");
        }
    }
}