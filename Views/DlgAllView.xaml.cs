using System.Data.SQLite;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GeoTools.Model;


namespace GeoTools.Views;

public partial class DlgViewAll : UserControl
{
    private SQLiteConnection co = MainWindow.Connection;
    private User user = MainWindow.UserSession;

    public DlgViewAll()
    {
        InitializeComponent();

        // Admin = IsAdmin();
        CreateBtnDlg();

    }

    private void CreateBtnDlg()
    {
        var style = FindResource("ButtonDLGTemp") as Style;

        const string req = @"SELECT dlg.*,
                                (SELECT DISTINCT ex_et_ref FROM v_exports_en_cours WHERE ex_dl_id=dlg.dl_id) AS ex_et_ref,
                                (SELECT DISTINCT ex_et_nom FROM v_exports_en_cours WHERE ex_dl_id=dlg.dl_id) AS ex_et_nom,
                                (SELECT DISTINCT et_rgb FROM v_exports_en_cours WHERE ex_dl_id=dlg.dl_id) AS et_rgb
                            FROM v_dlg_tmp dlg;";
        
        const byte fontsize = 8;
        const byte heighsize = 19;
        const byte widthsize = 90;
        
        SQLiteCommand cd = new SQLiteCommand(req, co);
        SQLiteDataReader cdReader = cd.ExecuteReader();
        while (cdReader.Read())
        {
            
            Label lbZoMarche = new Label()
            {
                Content = $"RIP{cdReader["zo_marche"]}",
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontsize,
                Height = heighsize,
                Width = widthsize
            };

            Label lbDlInitDate = new Label()
            {
                Content = $"{cdReader["dl_init_date"]}",
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontsize,
                Height = heighsize,
                Width = widthsize
            };
            
            Label lbExEtNom = new Label()
            {
                Content = $"{cdReader["ex_et_nom"]}",
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontsize,
                Height = heighsize,
                Width = widthsize
            };

            // Create the Grid
            Grid grid = new Grid()
            {
                Width = 250,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true
            };

            // // Define the Columns
            for (byte i = 0; i < 2; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                if (i > 0)
                {
                    col.Width = new GridLength(widthsize);
                }
                grid.ColumnDefinitions.Add(col);
            }

            // Define the Rows
            for (byte i = 0; i < 3; i++)
            {
                RowDefinition row = new RowDefinition();
                grid.RowDefinitions.Add(row);
            }
            
            SetElementGrid(element:lbZoMarche, row:0, column:1);
            SetElementGrid(element:lbDlInitDate, row:1, column:1);
            SetElementGrid(element:lbExEtNom, row:2, column:1);

            grid.Children.Add(lbZoMarche);
            grid.Children.Add(lbDlInitDate);
            grid.Children.Add(lbExEtNom);

            Button button = new Button()
            {
                Content = grid,
                Name = $"dlg_{cdReader["dl_id"]}",
                Style = style,
                ToolTip = @$"{cdReader["dlg"]}
Etat : {cdReader["ex_et_nom"]} ({cdReader["ex_et_ref"]})
ID : {cdReader["dl_id"]}
admin={user.Admin}",
                Background = Brushes.RoyalBlue
            };
            
            
            button.Click += new RoutedEventHandler(button_Click);
            Panel.Children.Add(button);
        }
    }
    private static void SetElementGrid(UIElement element, int row=0, int column=0)
    {
        Grid.SetRow(element, row);
        Grid.SetColumn(element, column);
    }
    
    void button_Click(object sender, RoutedEventArgs e)
    {
        //MessageBox.Show($"You clicked on the {(sender as Button).Name}"string.Format("You clicked on the {0}. button.", (sender as Button).Name));
        
        string btnName = (sender as Button).Name;
        if (btnName is not null)
        {
            MessageBox.Show($"You clicked on the {btnName}");
        }
    }
}