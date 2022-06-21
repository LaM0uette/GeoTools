using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeoTools.Model;
using GeoTools.Views;
using MahApps.Metro.Controls;

namespace GeoTools
{
    
    public partial class MainWindow
    {
        public static SQLiteConnection Connection = new ();
        public static User UserSession = SetUserParameters(new User());

        public MainWindow()
        {
            Connection = new SQLiteConnection("Data Source=T:\\- 4 Suivi Appuis\\25_BDD\\MyDLG\\bdd.sqlite");
            Connection.Open();
        }
        
        private static User SetUserParameters(User userSession)
        {
            string req = $"SELECT * FROM t_users WHERE us_guid='{userSession.Guid}'";

            SQLiteCommand command = new SQLiteCommand(req, Connection);
            SQLiteDataReader cdReader = command.ExecuteReader();
        
            while (cdReader.Read())
            {
                userSession.Prenom = $"{cdReader["us_prenom"]}";
                userSession.Nom = $"{cdReader["us_nom"]}";
                
                if (cdReader.GetByte(cdReader.GetOrdinal("us_admin")) == 1)
                {
                    userSession.Admin = true;
                }
            }
            return userSession;
        }
        
    }
}