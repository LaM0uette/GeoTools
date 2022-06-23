using Npgsql;

namespace GeoTools.Utils;

public class Sql
{
    
    public struct SqlCommand
    {
        public NpgsqlConnection Connection;
        public NpgsqlTransaction Transaction;
    }
    
    public static SqlCommand Connect() 
    {
        var connexionString = "HOST=BORDEAUX04;Username=postgres;Password=INEO_Infracom_33;Database=sig";
        var connexion = new NpgsqlConnection(connexionString);
        connexion.Open();
        
        var transaction = connexion.BeginTransaction();
        
        return new SqlCommand{Connection = connexion, Transaction = transaction};
    }
}