using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GeoTools.Utils;
using GeoTools.Model;
using Npgsql;


namespace GeoTools.Views;

public partial class DlgViewAll : UserControl
{
    private User user = MainWindow.UserSession;

    public DlgViewAll()
    {
        InitializeComponent();
        
        CreateBtnDlg();

    }

    private void CreateBtnDlg()
    {
        var style = FindResource("ButtonDLGTemp") as Style;

        const byte fontsize = 8;
        const byte heighsize = 19;
        const byte widthsize = 90;

        NpgsqlDataReader cdReader = Sql.GetAllDlg();
        
        while (cdReader.Read())
        {
            
            Label lbZoMarche = new Label()
            {
                Content = $"RIP{cdReader["refcode1"]}",
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontsize,
                Height = heighsize,
                Width = widthsize
            };

            Label lbDlInitDate = new Label()
            {
                Content = $"{cdReader["date_initial"]}",
                Background = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = fontsize,
                Height = heighsize,
                Width = widthsize
            };
            
            Label lbExEtNom = new Label()
            {
                Content = $"{cdReader["nom_etat"]}",
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
            
            Tasks.SetElementGrid(element:lbZoMarche, row:0, column:1);
            Tasks.SetElementGrid(element:lbDlInitDate, row:1, column:1);
            Tasks.SetElementGrid(element:lbExEtNom, row:2, column:1);

            grid.Children.Add(lbZoMarche);
            grid.Children.Add(lbDlInitDate);
            grid.Children.Add(lbExEtNom);

            Button button = new Button()
            {
                Content = grid,
                Name = $"dlg_{cdReader["id"]}",
                Style = style,
                ToolTip = //$"{cdReader["dlg"]}" +
                          $"Etat : {cdReader["nom_etat"]} ({cdReader["code_etat"]})\n" +
                          $"ID : {cdReader["id"]}\n" +
                          $"admin={user.Admin}\n" +
                          $"prenom={user.Prenom}",
                Background = Tasks.hexBrush(hexColor:cdReader["couleur_etat"].ToString())
            };

            button.Click += button_Click;
            Panel.Children.Add(button);
        }
        cdReader.Close();
    }

    static void button_Click(object sender, RoutedEventArgs e)
    {

        string btnName = ((Button)sender).Name;
        if (btnName is not null)
        {
            MessageBox.Show($"You clicked on the {btnName}");
        }
    }
}