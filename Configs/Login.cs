namespace GeoTools.Configs;

public static class Login
{
    public readonly struct SqlStruct
    {
        private string Host { get; }
        private string User { get; }
        private string Password { get; }
        private string Database { get; }
        public string AppName { get; init; }

        public SqlStruct(string host, string user, string password, string database, string appName)
        {
            Host = host;
            User = user;
            Password = password;
            Database = database;
            AppName = appName;
        }

        public override string ToString()
        {
            return @$"HOST={Host};
               Username={User};
               Password={Password};
               Database={Database};
               ApplicationName={AppName}";
        }
    }

    private const string PgHost = "BORDEAUX04";
    private const string PgUser = "postgres";
    private const string PgPassword = "INEO_Infracom_33";
    private const string PgDatabase = "sig";

    public static SqlStruct GeoTools = new(
        host: PgHost,
        user: PgUser,
        password: PgPassword,
        database: PgDatabase,
        appName: "GeoTools"
    );

    public static SqlStruct GeoToolsNotif = GeoTools with { AppName = "GeoToolsNotif"};
}