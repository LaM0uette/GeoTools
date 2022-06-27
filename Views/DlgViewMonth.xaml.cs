using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GeoTools.Utils;
using Microsoft.VisualBasic;

namespace GeoTools.Views;

public partial class DlgViewMonth : UserControl
{
    public static DlgViewMonth InstanceDlgViewMonth;
    
    public DlgViewMonth()
    {
        InstanceDlgViewMonth = this;
        InitializeComponent();
        
        CreateBtnDlgMonth(year:2022, month:6);
    }

    public void CreateBtnDlgMonth(int year, byte month)
    {
        byte nbWeeks = Tasks.WeekInMonth(year: year, month: month);
        //MessageBox.Show($"{nbWeeks}");
    }
}