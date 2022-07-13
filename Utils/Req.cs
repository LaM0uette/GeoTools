namespace GeoTools.Utils;

public static class Req
{
    public static string AddDlg(
        string proj, 
        string refcode3, 
        string dateInit, 
        string phase, 
        string typeExport, 
        int livraison, 
        int version) =>
        
        @$"SELECT * 
           FROM ""GeoTools"".""add_dlg""(
               '{proj}', '{refcode3}', '{dateInit}', '{phase}', '{typeExport}', {livraison}, {version})";
    
    public static string AllDlg() =>
        @"SELECT * 
          FROM ""GeoTools"".""v_dlg""";

    public static string AllDlgFiltered(int id) =>
        @$"SELECT * 
           FROM ""GeoTools"".""v_dlg"" 
           WHERE id_etat={id}";

    public static string DlgByDate(string date) => 
        @$"SELECT * 
           FROM ""GeoTools"".get_dlg_by_date('{date}')";

    public static string DlgByWeek(byte week, int year) =>
        @$"SELECT * 
           FROM ""GeoTools"".get_dlg_by_weeks({week}, {year})";
    
    public static string DlgFilteredByWeek(byte week, int year, int id) =>
        @$"SELECT * 
           FROM ""GeoTools"".get_dlg_by_weeks({week}, {year})
           WHERE id_etat = {id}";
    
    public static string UserInformation(string guid) =>
        @$"SELECT * 
           FROM ""GeoTools"".""t_users""
           WHERE us_guid='{guid}'";
}
