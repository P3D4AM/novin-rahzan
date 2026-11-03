using System.ComponentModel;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class CodeingHesab
{
	[AttHelpURL("New_Item41.htm")]
	[PersianNameAttribute.PersianName("کدینگ حسابها")]
	[DefaultValue(true)]
	public bool CodeingHesabMain { get; set; } = true;


	[PersianNameAttribute.PersianName("اضافه کردن کدینگ حسابها")]
	[DefaultValue(true)]
	public bool CodeingHesabInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش کدینگ حسابها")]
	[DefaultValue(true)]
	public bool CodeingHesabUpdate { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف از کدینگ حسابها")]
	[DefaultValue(true)]
	public bool CodeingHesabDelete { get; set; } = true;

}
