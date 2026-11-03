using System.ComponentModel;

namespace PacketParser.Attendance.Access;

public class AgreedDeduction
{
	[PersianNameAttribute.PersianName("جدول کسورات توافقی")]
	[DefaultValue(true)]
	public bool AgreedDeductionMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد کسورات توافقی")]
	[DefaultValue(true)]
	public bool AgreedDeductionInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف کسورات توافقی")]
	[DefaultValue(true)]
	public bool AgreedDeductionDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش کسورات توافقی")]
	[DefaultValue(true)]
	public bool AgreedDeductionUpdate { get; set; } = true;

}
