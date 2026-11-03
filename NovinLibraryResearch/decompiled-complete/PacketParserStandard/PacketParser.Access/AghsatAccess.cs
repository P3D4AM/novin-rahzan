using System;
using System.ComponentModel;

namespace PacketParser.Access;

public class AghsatAccess
{
	[Obsolete("", true)]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("عملیات اقساط دریافتنی")]
	public bool Aghsat_M_Amaliat { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور قسط دریافتنی")]
	public bool Aghsat_M_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش اسناد دریافتنی")]
	public bool Aghsat_M_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور گروهی اقساط دریافتنی")]
	public bool Aghsat_M_Group { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("اعلام دریافت اقساط دریافتنی")]
	public bool Aghsat_M_Reception { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف اقساط دریافتنی")]
	public bool Aghsat_M_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش اقساط دریافتنی")]
	public bool Aghsat_M_Edit { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست اقساط دریافتنی")]
	public bool Aghsat_M_PrintList { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ دفترچه اقساط دریافتنی")]
	public bool Aghsat_M_PrintNote { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده سند دریافت اقساط دریافتنی")]
	public bool Aghsat_M_ShowSanad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تجمیع اقساط دریافتنی")]
	public bool Aghsat_M_Aggregate { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال پیامک اقساط دریافتنی")]
	public bool Aghsat_M_SendSms { get; set; } = true;


	[Obsolete("", true)]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("عملیات اقساط پرداختنی")]
	public bool Aghsat_SH_Amaliat { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور قسط پرداختنی")]
	public bool Aghsat_SH_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش اسناد پرداختنی")]
	public bool Aghsat_SH_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور گروهی اقساط پرداختنی")]
	public bool Aghsat_SH_Group { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("اعلام پرداخت اقساط پرداختنی")]
	public bool Aghsat_SH_Pardakht { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف اقساط پرداختنی")]
	public bool Aghsat_SH_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش اقساط پرداختنی")]
	public bool Aghsat_SH_Edit { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده سند پرداخت اقساط پرداختنی")]
	public bool Aghsat_SH_ShowSanad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ اقساط پرداختنی")]
	public bool Aghsat_SH_Print { get; set; } = true;


	[PersianNameAttribute.PersianName("دفتر وام")]
	public bool Vam_Report { get; set; } = true;

}
