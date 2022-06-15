using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace GeoTools.Views;

public partial class DlgViewAll : UserControl
{

    public DlgViewAll()
    {
        InitializeComponent();
        CreateBtnDlg();

    }

    private void CreateBtnDlg()
    {
        var style = FindResource("ButtonDLGTemp") as Style;
        string connectionString = "Data Source=T:\\- 4 Suivi Appuis\\25_BDD\\MyDLG\\bdd.sqlite";

        SQLiteConnection connection = new SQLiteConnection(connectionString);
        connection.Open();
        
        SQLiteCommand command = new SQLiteCommand("SELECT * FROM v_dlg_tmp", connection);
        SQLiteDataReader sqlReader = command.ExecuteReader();
        while (sqlReader.Read())
        {

            Label lb_ext_id = new Label(){Content = $"{sqlReader["zo_ext_id"]}"};
            Label lb_phase_td = new Label(){Content = $"{sqlReader["dl_phase"]}-{sqlReader["dl_td"]}"};
            Label lb_nLivr_nVer = new Label(){Content = $"{sqlReader["dl_no_livraison"]}-V{sqlReader["dl_no_version"]}"};


            Button button = new Button()
            {
                Content = lb_ext_id,
                Name = $"dlg_{sqlReader["dl_id"]}" + lb_phase_td + lb_nLivr_nVer,
                Style = style,
            };
            
            
            
            // Button button = new Button()
            // {
            //     Content = $"{sqlReader["zo_ext_id"]}" +
            //               $"\n{sqlReader["dl_phase"]}-{sqlReader["dl_td"]}" +
            //               $"\n{sqlReader["dl_no_livraison"]}-V{sqlReader["dl_no_version"]}",
            //     Name = $"dlg_{sqlReader["dl_id"]}",
            //     Style = style,
            // };

            button.Click += new RoutedEventHandler(button_Click);
            Panel.Children.Add(button);
        }
        
        connection.Close();
    }
    
    void button_Click(object sender, RoutedEventArgs e)
    {
        //MessageBox.Show($"You clicked on the {(sender as Button).Name}"string.Format("You clicked on the {0}. button.", (sender as Button).Name));
        MessageBox.Show($"You clicked on the {(sender as Button).Name}");
    }
}