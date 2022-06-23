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
            Connection = new SQLiteConnection("Data Source=T:\\- 4 Suivi Appuis\\25_BDD\\MyDLG\\bdd.sqlite");
            Connection.Open();

            var cs = "HOST=BORDEAUX04;Username=postgres;Password=INEO_Infracom_33;Database=sig";
            var con = new NpgsqlConnection(cs);

            con.Open();
            var tr = con.BeginTransaction();

            // var sql = "get_dlg_by_date";

            // var sql = "SELECT * FROM \"GeoTools\".v_dlg";
            var sql = "SELECT * FROM \"GeoTools\".get_dlg_by_date('2022-06-23')";
            sql = "SELECT * FROM \"GeoTools\".add_dlg('XD5965', 'BIVO', CURRENT_DATE, 'EXE', 'TRANSPORT ET DISTRIBUTION', 2, 4)";
            var cmd = new NpgsqlCommand(sql, con);
            tr.Commit();

            NpgsqlDataReader pgreader = cmd.ExecuteReader();


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