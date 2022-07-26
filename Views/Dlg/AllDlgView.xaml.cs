using System;
using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;
using System.Runtime.InteropServices;
using Npgsql;


namespace GeoTools.Views.Dlg;

public partial class AllDlgView
{
    public static AllDlgView Instance = new();
    
    public AllDlgView()
    {
        Instance = this;
        
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
    public static void BtnDlgAll_Click(object sender, RoutedEventArgs e)
    {
        var btnName = ((Button) sender).Name;
        if (btnName is not null)
        {
            MessageBox.Show($"Tu as cliqué sur le bouton : {btnName}");
        }
    }
}