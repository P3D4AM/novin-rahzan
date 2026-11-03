using System.ComponentModel;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Device
{
	[PersianNameAttribute.PersianName("تعداد دستگاه های قابل تعریف")]
	[AttHelpURL("New_Item5.htm")]
	[DefaultValue(true)]
	public bool DeviceMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد اطلاعات یک دستگاه")]
	[DefaultValue(true)]
	public bool DeviceInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف اطلاعات یک دستگاه")]
	[DefaultValue(true)]
	public bool DeviceDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش اطلاعات یک دستگاه")]
	[DefaultValue(true)]
	public bool DeviceUpdate { get; set; } = true;


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
		else if (pAccessLevel.LevelHozor == Level_Hozor.Level4 && count > 3)
		{
			result = false;
		}
		return result;
	}
}
