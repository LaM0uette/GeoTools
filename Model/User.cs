using CommonTasks;
using GeoTools.Utils;

namespace GeoTools.Model;

public class User
{

    public string Guid { get; set; } = Windows.GetGuid();
    
    public int Refcode1 { get; set; }
    
    public string? Nom { get; set; }

    public string? Prenom { get; set; }
    
    public int Role { get; set; }
    
    public bool Admin { get; set; }
}
