using System.Windows;
using GeoTools.Model;
using GeoTools.Utils;
using Npgsql;

namespace GeoTools
{
    public partial class MainWindow
    {
        public static User UserSession = new();

        public MainWindow()
        {
            SizeChanged += onsenfou;
            SetUserParameters();
        }

        private static void SetUserParameters()
        {

            NpgsqlDataReader cdReader = Sql.GetUserInformation(guid: Tasks.GetUserSession());
            
            while (cdReader.Read())
            {
                
                UserSession.Prenom = $"{cdReader["us_prenom"]}";
                UserSession.Nom = $"{cdReader["us_nom"]}";

                if (cdReader.GetByte(cdReader.GetOrdinal("us_admin")) == 1)
                {
                    UserSession.Admin = true;
                }
            }
            cdReader.Close();
        }

        private void onsenfou(object sender, SizeChangedEventArgs e)
        {
            //Views.DlgView.SetWith();
        }
    }
}