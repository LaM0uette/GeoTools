using System;
using System.Threading.Tasks;
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
    private const byte fontsize = 13;
    private const byte heighsize = 26;
    private const byte widthsize = 125;

    public DlgViewAll()
    {
        InitializeComponent();
        
        CreateBtnDlg();

    }

    private void CreateBtnDlg()
    {
        var style = FindResource("ButtonDLGTemp") as Style;

        NpgsqlDataReader cdReader = Sql.GetAllDlg();
        
        while (cdReader.Read())
        {
            Label lbZoMarche = MakeLabel(content: $"RIP{cdReader["refcode1"]}");
            Label lbDlInitDate = MakeLabel(content: $"{DateTime.Parse(cdReader["date_initial"].ToString()).ToString("MM/dd/yyyy")}");
            Label lbExEtNom = MakeLabel(content: $"{cdReader["nom_etat"]}");

            Border bdtamere = new Border()
            {
                CornerRadius = new CornerRadius(5),
                Background = Brushes.White,
                Height = heighsize,
                Width = widthsize,
                Margin = new Thickness(5, 0, 5, 0),
                };
            bdtamere.Child = lbZoMarche;
            
            // Create the Grid
            Grid grid = new Grid()
            {
                Width = 300,
                Height = 120,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true
            };

            Grid panel = new Grid()
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Center
                
            };
            // Define the Rows
            for (byte i = 0; i < 3; i++)
            {
                RowDefinition row = new RowDefinition();
                panel.RowDefinitions.Add(row);
            }
            Tasks.SetElementGrid(element:lbZoMarche, row:0);
            Tasks.SetElementGrid(element:lbDlInitDate, row:1);
            Tasks.SetElementGrid(element:lbExEtNom, row:2);

            panel.Children.Add(bdtamere);
            panel.Children.Add(lbDlInitDate);
            panel.Children.Add(lbExEtNom);

            // // Define the Columns
            for (byte i = 0; i < 2; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                if (i > 0)
                {
                    col.Width = new GridLength(widthsize+20);
                }
                grid.ColumnDefinitions.Add(col);
            }

            // Define the Rows
            for (byte i = 0; i < 1; i++)
            {
                RowDefinition row = new RowDefinition();
                grid.RowDefinitions.Add(row);
            }

            TextBlock dlgInfo = new TextBlock()
            {
                Text = cdReader["dlg_infos"].ToString().Replace("|", "\n"),
                FontSize = fontsize + 8,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            
            Tasks.SetElementGrid(element:dlgInfo);
            Tasks.SetElementGrid(element:panel, column:1);
            
            // Tasks.SetElementGrid(element:lbZoMarche, row:0, column:1);
            // Tasks.SetElementGrid(element:lbDlInitDate, row:1, column:1);
            // Tasks.SetElementGrid(element:lbExEtNom, row:2, column:1);
            //
            // grid.Children.Add(lbZoMarche);
            // grid.Children.Add(lbDlInitDate);
            // grid.Children.Add(lbExEtNom);
            
            grid.Children.Add(panel);
            grid.Children.Add(dlgInfo);

            Button button = new Button()
            {
                Content = grid,
                Name = $"dlg_{cdReader["id"]}",
                Style = style,
                ToolTip = $"{cdReader["dlg"]}\n" +
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

    static Label MakeLabel(string content)
    {
        Label test = new Label()
        {
            Content = content,
            Background = Brushes.Transparent,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = fontsize,
            // Height = heighsize,
            // Width = widthsize,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            // Margin = new Thickness(5, 0, 5, 0),
        };
        return test;
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