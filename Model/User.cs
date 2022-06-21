using System;
using GeoTools.Functions;

namespace GeoTools.Model;

public class User
{
    public string Guid { get; set; } = Tasks.GetUserSession();
    public bool Admin { get; set; }
    public string Prenom { get; set; }
    public string Nom { get; set; }

}
