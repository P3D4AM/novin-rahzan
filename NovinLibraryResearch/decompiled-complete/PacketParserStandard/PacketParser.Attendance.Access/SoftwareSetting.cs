using System.ComponentModel;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class SoftwareSetting
{
	[PersianNameAttribute.PersianName("تنظیمات نرم\u00adافزار")]
	[AttHelpURL("New_Item71.htm")]
	[DefaultValue(true)]
	public bool SoftwareSettingMain { get; set; } = true;


	[PersianNameAttribute.PersianName("تغییر رمز")]
	[AttHelpURL("New_Item70.htm")]
	[DefaultValue(true)]
	public bool ChangePassword { get; set; } = true;


	[PersianNameAttribute.PersianName("تنظیمات پیامک")]
	[DefaultValue(true)]
	public bool SoftwareSettingPayamak { get; set; } = true;


	[PersianNameAttribute.PersianName("اطلاعات شرکت")]
	[DefaultValue(true)]
	public bool SaleKari { get; set; } = true;


	[PersianNameAttribute.PersianName("اطلاعات پشتیبان گیری")]
	[DefaultValue(true)]
	public bool Backup { get; set; } = true;


	[PersianNameAttribute.PersianName(" تنظیم اطلاعات حقوقی")]
	[DefaultValue(true)]
	public bool SalaryInf { get; set; } = true;


	[PersianNameAttribute.PersianName("سند اتومات حقوق")]
	[DefaultValue(true)]
	public bool SanadInf { get; set; } = true;


	[PersianNameAttribute.PersianName("تنظیمات متفرقه")]
	[DefaultValue(true)]
	public bool ExtraSetting { get; set; } = true;

}
