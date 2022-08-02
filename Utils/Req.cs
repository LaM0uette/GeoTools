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
    
    public static string AllDlg() => """
    SELECT *
    FROM "GeoTools"."v_dlg"
    """;

    public static string AllDlgFiltered(int id) =>
        @$"SELECT * 
           FROM ""GeoTools"".""v_dlg"" 
           WHERE id_etat={id}";

    public static string DlgByDate(string date) => 
        @$"SELECT * 
           FROM ""GeoTools"".get_dlg_by_date('{date}')";

    public static string DlgByWeek(byte week, int year) =>
        @$"SELECT * 
           FROM ""GeoTools"".get_dlg_by_week({week}, {year})";
    
    public static string DlgByMonth(byte month, int year) =>
        @$"SELECT * 
           FROM ""GeoTools"".get_dlg_by_month({month}, {year})";
    
    public static string DlgFilteredByDay(string date, int id) =>
        @$"SELECT * 
           FROM ""GeoTools"".get_dlg_by_date({date})
           WHERE ""id_etat"" = {id}";
    
    public static string DlgFilteredByWeek(byte week, int year, int id) =>
        @$"SELECT * 
           FROM ""GeoTools"".get_dlg_by_week({week}, {year})
           WHERE ""id_etat"" = {id}";
    
    public static string DlgFilteredByMonth(byte month, int year, int id) =>
        @$"SELECT * 
           FROM ""GeoTools"".get_dlg_by_month({month}, {year})
           WHERE ""id_etat"" = {id}";

    public static string GetDlg(int id) =>
        @$"SELECT * FROM ""GeoTools"".get_dlg({id})";
    
    public static string Logs() => "SELECT * FROM \"GeoTools\".t_logs";
    
    public static string UserInformation(string guid) =>
        @$"SELECT * 
           FROM ""GeoTools"".""t_users""
           WHERE us_guid='{guid}'";
}
