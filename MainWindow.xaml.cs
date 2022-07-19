using System;
using System.Threading.Tasks;
using CommonTasks;
using GeoTools.Model;
using GeoTools.Utils;

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
            
            UserSession.Refcode1 = int.Parse($"{cdReader["us_refcode1"]}");
            UserSession.Nom = $"{cdReader["us_nom"]}";
            UserSession.Prenom = $"{cdReader["us_prenom"]}";
            UserSession.Role = int.Parse($"{cdReader["us_role"]}");
            UserSession.Admin = int.Parse($"{cdReader["us_admin"]}") == 1;

            cdReader.Close();
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Sql.Close();
        }
    }
}