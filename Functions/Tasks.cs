using System;
namespace GeoTools.Functions;

public class Tasks
{
    private static string _guid ;

    public static string Guid
    {
        get => _guid;
        set => _guid = Environment.UserName;
    }
}

