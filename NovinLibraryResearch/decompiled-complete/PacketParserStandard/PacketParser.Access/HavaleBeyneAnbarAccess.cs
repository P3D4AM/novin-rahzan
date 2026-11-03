using System.ComponentModel;

namespace PacketParser.Access;

public class HavaleBeyneAnbarAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور حواله بین انبارها")]
	public bool HavaleBeyneAnbar_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف حواله بین انبارها (کلیه کاربران) ")]
	public bool HavaleBeyneAnbar_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش حواله بین انبارها (کلیه کاربران) ")]
	public bool HavaleBeyneAnbar_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش حواله بین انبارها(فقط خودش) ")]
	public bool HavaleBeyneAnbar_Update_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف حواله بین انبارها(فقط خودش) ")]
	public bool HavaleBeyneAnbar_Delete_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ حواله بین انبارها (کلیه کاربران) ")]
	public bool HavaleBeyneAnbar_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ حواله بین انبارها(فقط خودش) ")]
	public bool HavaleBeyneAnbar_Print_Self { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری حواله بین انبارها (کلیه کاربران) ")]
	public bool HavaleBeyneAnbar_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری حواله بین انبارها(فقط خودش) ")]
	public bool HavaleBeyneAnbar_ReportSelf { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده فی کاردکس در حواله بین انبارها ")]
	public bool HavaleBeyneAnbar_KardexPrice { get; set; } = true;


	[DefaultValue(false)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام حواله بین انبارها")]
	public bool EditPrice { get; set; }

	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند حواله بین انبار ها")]
	public bool HavaleBeyneAnbar_ChangeStatusSanad { get; set; } = true;

}
