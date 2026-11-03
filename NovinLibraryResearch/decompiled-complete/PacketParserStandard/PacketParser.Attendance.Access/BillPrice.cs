using System.ComponentModel;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class BillPrice
{
	[PersianNameAttribute.PersianName("فیش حقوقی")]
	[AttHelpURL("New_Item62.htm")]
	[DefaultValue(true)]
	public bool BillPriceMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد فیش حقوقی")]
	[DefaultValue(true)]
	public bool BillPriceInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف فیش حقوقی")]
	[DefaultValue(true)]
	public bool BillPriceDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش فیش حقوقی")]
	[DefaultValue(true)]
	public bool BillPriceUpdate { get; set; } = true;


	[PersianNameAttribute.PersianName("نمایش فیش حقوقی")]
	[DefaultValue(true)]
	public bool BillPriceDisplay { get; set; } = true;


	[PersianNameAttribute.PersianName("گزارش فیش حقوقی")]
	[AttHelpURL("New_Item63.htm")]
	[DefaultValue(true)]
	public bool BillPriceReport { get; set; } = true;


	[PersianNameAttribute.PersianName("چاپ فیش حقوقی")]
	[DefaultValue(true)]
	public bool BillPricePrint { get; set; } = true;


	[PersianNameAttribute.PersianName("تغییر وضعیت فیش از دائم به موقت")]
	[DefaultValue(true)]
	public bool changeEditStateOfFish { get; set; } = true;


	public bool CanInsert(AccessLevelClass pAccessLevel)
	{
		return pAccessLevel.LevelHozor >= Level_Hozor.Level3;
	}
}
