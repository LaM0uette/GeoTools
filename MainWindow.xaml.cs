using System.Data.SQLite;
using GeoTools.Model;

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
            
            UserSession = SetUserParameters(UserSession);
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