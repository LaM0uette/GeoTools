using System;
using System.Threading.Tasks;
using CommonTasks;
using GeoTools.Model;
using GeoTools.Utils;
using GeoTools.Views;
using Parser;

namespace GeoTools
{
    public partial class MainWindow
    {
        public static User UserSession { get; } = new();
        public static Task PgSql { get; set; } = Sql.PgNotifierConnect();

        public MainWindow()
        {
            SetUserParameters();
        }
        
        private static void SetUserParameters()
        {
            var cdReader = Sql.Get(Req.UserInformation(TskWindows.GetGuid()));
            cdReader.Read();
            
            UserSession.Refcode1 = $"{cdReader["us_refcode1"]}".ParseToInt();
            UserSession.Nom = $"{cdReader["us_nom"]}";
            UserSession.Prenom = $"{cdReader["us_prenom"]}";
            UserSession.Role = $"{cdReader["us_role"]}".ParseToInt();
            UserSession.Admin = $"{cdReader["us_admin"]}".ParseToInt() == 1;

            cdReader.Close();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Sql.Close();
        }
    }
}