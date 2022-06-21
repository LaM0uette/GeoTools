using System.Data.SQLite;
using System.Threading.Tasks;
using GeoTools.Model;
using GeoTools.Functions;

namespace GeoTools
{
    
    public partial class MainWindow
    {
        public static SQLiteConnection Connection = new ();
        public static User UserSession = new ();

        public MainWindow()
        {
            Connection = new SQLiteConnection("Data Source=T:\\- 4 Suivi Appuis\\25_BDD\\MyDLG\\bdd.sqlite");
            Connection.Open();
            
            SetUserParameters();
        }
        
        private static void SetUserParameters()
        {
            string req = $"SELECT * FROM t_users WHERE us_guid='{Tasks.GetUserSession()}'";

            SQLiteCommand command = new SQLiteCommand(req, Connection);
            SQLiteDataReader cdReader = command.ExecuteReader();
        
            while (cdReader.Read())
            {
                UserSession.Prenom = $"{cdReader["us_prenom"]}";
                UserSession.Nom = $"{cdReader["us_nom"]}";
                
                if (cdReader.GetByte(cdReader.GetOrdinal("us_admin")) == 1)
                {
                    UserSession.Admin = true;
                }
            }
        }
        
    }
}