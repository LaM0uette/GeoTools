using System;

namespace GeoTools.Model;

public class User
{
    public string Guid { get; } = Environment.UserName;
    public bool Admin { get; set; }
    public string Prenom { get; set; }
    public string Nom { get; set; }

}
