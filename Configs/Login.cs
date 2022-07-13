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

    public static SqlStruct GeoTools = new(
        host: "BORDEAUX04",
        user: "postgres",
        password: "INEO_Infracom_33",
        database: "sig",
        appName: "GeoTools"
    );

    public static SqlStruct GeoToolsNotif = GeoTools with { AppName = "GeoToolsNotif" };
}