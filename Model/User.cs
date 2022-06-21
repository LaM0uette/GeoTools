using System;
using GeoTools.Functions;

namespace GeoTools.Model;

public class User
{
    public string Guid = Tasks.Guid;
    public bool Admin { get; set; }
    public string Prenom { get; set; }
    public string Nom { get; set; }

}
