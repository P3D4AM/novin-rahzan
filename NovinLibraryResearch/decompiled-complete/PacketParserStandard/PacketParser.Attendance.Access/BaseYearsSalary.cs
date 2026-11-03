using System.ComponentModel;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class BaseYearsSalary
{
	[PersianNameAttribute.PersianName("پایه سنواتی ")]
	[AttHelpURL("New_Item47.htm")]
	[DefaultValue(true)]
	public bool BaseYearsSalaryMain { get; set; } = true;

}
