using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class CheckAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("عملیات چک های دریافتنی (وارده)")]
	public bool Check_Daryaftani { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("عملیات چک های پرداختنی (صادره)")]
	public bool Check_Pardakhtani { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش بدهی و طلب چک ها")]
	public bool Check_BedehiTalab { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چک پرینت")]
	public bool Check_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("طراح چک پرینت")]
	public bool Check_PrintDesign { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست چکها")]
	public bool Check_SeeList { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("عملیات اسناد دریافتنی ابتدای دوره")]
	public bool Check_VaredeA2reInsert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("عملیات اسناد پرداختنی ابتدای دوره")]
	public bool Check_SadereA2reInsert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("نمودار ماهیانه چکها")]
	public bool Check_NemodareMahiane { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("یاد آوری چک ها")]
	public bool Check_Yadavari { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("طراحی فرم واگذاری چک")]
	public bool Check_Vagozari_Form { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند چک ها")]
	public bool Check_ChangeStatusSanad { get; set; } = true;


	public List<ChequeSanadSanadStatusAccess> SanadStatusAccess { get; set; } = new List<ChequeSanadSanadStatusAccess>();


	public SanadChequeDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new SanadChequeDefaultSanadStatusAccess();

}
