using System.ComponentModel;

namespace PacketParser.Attendance.Access;

public class Insurance
{
	[PersianNameAttribute.PersianName("تهیه لیست بیمه")]
	[DefaultValue(true)]
	public bool InsuranceMain { get; set; } = true;


	[PersianNameAttribute.PersianName("تهیه فایل خروجی لیست بیمه")]
	[DefaultValue(true)]
	public bool PreparedDiskett { get; set; } = true;


	[PersianNameAttribute.PersianName("چاپ لیست بیمه")]
	[DefaultValue(true)]
	public bool InsurancePrint { get; set; } = true;

}
