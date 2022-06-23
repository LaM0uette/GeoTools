using Npgsql;

namespace GeoTools.Utils;

public class Sql
{
    
    public struct SqlCommand
    {
        public NpgsqlConnection Connection;
        public NpgsqlTransaction Transaction;
    }
    
    public SqlCommand Connect() 
    {
        var ConnexionString = "HOST=BORDEAUX04;Username=postgres;Password=INEO_Infracom_33;Database=sig";
        var connexion = new NpgsqlConnection(ConnexionString);
        connexion.Open();
        
        var transaction = connexion.BeginTransaction();
        
        return new SqlCommand{Connection = connexion, Transaction = transaction};
    }
}