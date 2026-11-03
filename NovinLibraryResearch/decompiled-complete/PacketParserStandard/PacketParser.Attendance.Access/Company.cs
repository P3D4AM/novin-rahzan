using System.ComponentModel;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Company
{
	[AttHelpURL("New_Item6.htm")]
	[PersianNameAttribute.PersianName("شرکت")]
	[DefaultValue(true)]
	public bool CompanyMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد اطلاعات یک شرکت")]
	[DefaultValue(true)]
	public bool CompanyInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف اطلاعات یک شرکت")]
	[DefaultValue(true)]
	public bool CompanyDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش اطلاعات یک شرکت")]
	[DefaultValue(true)]
	public bool CompanyUpdate { get; set; } = true;


	public bool CanInsert(AccessLevelClass pAccessLevel, int count)
	{
		bool result = true;
		if (pAccessLevel.LevelHozor <= Level_Hozor.Level2 && count > 1)
		{
			result = false;
		}
		else if (pAccessLevel.LevelHozor == Level_Hozor.Level3 && count > 2)
		{
			result = false;
		}
		else if (pAccessLevel.LevelHozor == Level_Hozor.Level4 && count > 5)
		{
			result = false;
		}
		return result;
	}
}
