using System.ComponentModel;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Vacation
{
	[PersianNameAttribute.PersianName("انواع مرخصی")]
	[AttHelpURL("New_Item58.htm")]
	[DefaultValue(true)]
	public bool VacationMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد انواع مرخصی")]
	[DefaultValue(true)]
	public bool VacationInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف انواع مرخصی")]
	[DefaultValue(true)]
	public bool VacationDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش انواع مرخصی")]
	[DefaultValue(true)]
	public bool VacationUpdate { get; set; } = true;


	[PersianNameAttribute.PersianName("نمایش انواع مرخصی")]
	[DefaultValue(true)]
	public bool VacationDisplay { get; set; } = true;


	[AttHelpURL("New_Item60.htm")]
	[PersianNameAttribute.PersianName("گزارش انواع مرخصی")]
	[DefaultValue(true)]
	public bool VacationReport { get; set; } = true;


	[PersianNameAttribute.PersianName("چاپ انواع مرخصی")]
	[DefaultValue(true)]
	public bool VacationPrint { get; set; } = true;


	[PersianNameAttribute.PersianName("مدیریت حقوق برای روزهای مرخصی")]
	[DefaultValue(true)]
	public bool VacationManagement { get; set; } = true;


	[PersianNameAttribute.PersianName("تعریف انواع مرخصی")]
	[AttHelpURL("New_Item37.htm")]
	[DefaultValue(true)]
	public bool VacationDefine { get; set; } = true;


	[AttHelpURL("New_Item7.htm")]
	[PersianNameAttribute.PersianName("اضافه، حذف، ویرایش  تعطیلات رسمی")]
	[DefaultValue(true)]
	public bool VacationHolidays { get; set; } = true;


	public bool CanVacationHolidays(AccessLevelClass pAccessLevel)
	{
		return pAccessLevel.LevelHozor >= Level_Hozor.Level1;
	}
}
