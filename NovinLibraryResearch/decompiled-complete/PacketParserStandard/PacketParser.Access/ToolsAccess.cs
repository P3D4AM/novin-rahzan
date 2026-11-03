using System.ComponentModel;

namespace PacketParser.Access;

public class ToolsAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("طراح گزارشات")]
	public bool Tools_ReportDesign { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ماشین حساب")]
	public bool Tools_Calculator { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دفترچه یادداشت")]
	public bool Tools_Notebook { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("کتابچه راهنما ")]
	public bool Tools_Rahnama { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشخصات نرم افزار")]
	public bool Tools_Software { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("راس گیری")]
	public bool Tools_RasGiri { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("درباره ما ...")]
	public bool Tolls_About { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال سازی")]
	public bool Tools_Activate { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تنظیمات ویژه اصناف")]
	public bool Tools_TanzimatevijeAsnaf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر رمز مدیریت")]
	public bool Tools_AdminPass { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("انتقال سال مالی")]
	public bool Tools_MaliYearTransfer { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مرتب سازی شماره اسناد براساس تاریخ")]
	public bool Tools_SanadSort { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لاگ گیری عملکرد کاربران")]
	public bool Tools_Log { get; set; } = true;

}
