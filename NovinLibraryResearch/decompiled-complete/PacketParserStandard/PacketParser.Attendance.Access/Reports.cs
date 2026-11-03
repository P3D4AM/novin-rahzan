using System.ComponentModel;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Reports
{
	[PersianNameAttribute.PersianName("گزارشات")]
	[DefaultValue(true)]
	public bool ReportsMain { get; set; } = true;


	[PersianNameAttribute.PersianName("گزارش ساز پویا")]
	[AttHelpURL("New_Item66.htm")]
	[DefaultValue(true)]
	public bool ReportsGozareshSazPoya { get; set; } = true;

}
