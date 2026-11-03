using System;
using System.ComponentModel;

namespace PacketParser.Access;

public class CustomerAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست اشخاص")]
	public bool Customer_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن شخص جدید")]
	public bool Customer_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف اشخاص موجود")]
	public bool Customer_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش اشخاص موجود")]
	public bool Customer_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده اشخاص حذف شده")]
	public bool Role_ShowDeleted { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن اشخاص حذف شده")]
	public bool Role_Active { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست اشخاص")]
	public bool Customer_Print { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گزارش گردش عملکرد اشخاص")]
	public bool Customer_SeeGardesh { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ گزارش گردش عملکرد اشخاص")]
	public bool Customer_PrintGardesh { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گزارش تفصیلی حساب اشخاص")]
	public bool Customer_SeeGardeshDetail { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ گزارش تفصیلی حساب اشخاص")]
	public bool Customer_PrintGardeshDetail { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به اشخاص بخصوص")]
	public bool Customer_DastrasiBeAshkhaseKhas { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("اصلاح مانده ابتدای دوره اشخاص")]
	public bool Customer_UpdateFirstAccount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لیست انتخابی اشخاص")]
	public bool Customer_SelectionList { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارشات تجمیعی فروش اشخاص")]
	public bool Customer_Sell_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارشات تجمیعی خرید اشخاص")]
	public bool Customer_Buy_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارشات تجمیعی برگشت از فروش اشخاص")]
	public bool Customer_BSell_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارشات تجمیعی برگشت از خرید اشخاص")]
	public bool Customer_BBuy_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارشات تجمیعی سود فروش اشخاص")]
	public bool Customer_Sood_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال و دریافت داده ها از طریق فایل اکسل")]
	public bool Customer_Last_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش آخرین فروش مشتریان")]
	public bool Customer_Bedehi_Generation { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش سن بدهی (پته بدهکاران)")]
	public bool Customer_Excel { get; set; } = true;


	[PersianNameAttribute.PersianName("لیست مسیر اشخاص")]
	public bool Customer_Road { get; set; } = true;


	public bool Customer_Card { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده سود فروش در گزارش گردش عملکرد اشخاص")]
	public bool Customer_SoodInGardesh { get; set; } = true;

}
