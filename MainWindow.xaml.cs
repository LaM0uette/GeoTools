using System.Data.SQLite;
using GeoTools.Model;
using GeoTools.Utils;
using Npgsql;

namespace GeoTools
{
    public partial class MainWindow
    {
        public static SQLiteConnection Connection = new();
        public static User UserSession = new();

        public MainWindow()
        {
            // var sql = "get_dlg_by_date";
            // var sql = "SELECT * FROM \"GeoTools\".v_dlg";
            //var sq = "SELECT * FROM \"GeoTools\".get_dlg_by_date('2022-06-23')";
            Connection = new SQLiteConnection("Data Source=T:\\- 4 Suivi Appuis\\25_BDD\\MyDLG\\bdd.sqlite");
            Connection.Open();

            var sql = Sql.Connect();
            var req = "SELECT * FROM \"GeoTools\".add_dlg('XD5965', 'BIVO', CURRENT_DATE, 'EXE', 'TRANSPORT ET DISTRIBUTION', 4, 6)";
            
            new NpgsqlCommand(req, sql.Connection).ExecuteNonQuery();
            sql.Transaction.Commit();

            SetUserParameters();
        }

        private static void SetUserParameters()
        {
            string req = $"SELECT * FROM t_users WHERE us_guid='{Tasks.GetUserSession()}'";

            SQLiteDataReader cdReader = Tasks.GetData(cmd: req);

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