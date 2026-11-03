using System.ComponentModel;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Personnel
{
	[AttHelpURL("New_Item38.htm")]
	[PersianNameAttribute.PersianName("کارمندان")]
	[DefaultValue(true)]
	public bool PersonnelMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد اطلاعات یک کارمند")]
	[DefaultValue(true)]
	public bool PersonnelInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف اطلاعات یک کارمند")]
	[DefaultValue(true)]
	public bool PersonnelDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش اطلاعات یک کارمند")]
	[DefaultValue(true)]
	public bool PersonnelUpdate { get; set; } = true;


	[PersianNameAttribute.PersianName("چاپ اطلاعات یک کارمند")]
	[DefaultValue(true)]
	public bool PersonnelPrint { get; set; } = true;


	[AttHelpURL("New_Item39.htm")]
	[PersianNameAttribute.PersianName("افزودن یا ویرایش کد سخت افزاری")]
	[DefaultValue(true)]
	public bool PersonnelHardWareCodeCust { get; set; } = true;


	[PersianNameAttribute.PersianName("خواندن اطلاعات اثر انگشت")]
	[DefaultValue(true)]
	public bool PersonnelSaveEnrollData { get; set; } = true;


	[PersianNameAttribute.PersianName("اطلاعات گروه کارکنان")]
	[DefaultValue(true)]
	public bool PersonnelGroup { get; set; } = true;


	public bool CanInsert(AccessLevelClass pAccessLevel, int count)
	{
		return true;
	}

	public bool CanInsertPersonGroup(AccessLevelClass pAccessLevel, int count)
	{
		return pAccessLevel.LevelHozor >= Level_Hozor.Level2;
	}
}
