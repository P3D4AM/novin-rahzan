using System.ComponentModel;

namespace PacketParser.Access;

public class ReportsAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری کارکرد روزانه")]
	public bool Report_Rozane { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش دفتر روزنامه")]
	public bool Report_Rozname { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش معین")]
	public bool Report_Moein { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تراز آزمایشی")]
	public bool Report_TarazAzmayeshi { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صورت سود و زیان")]
	public bool Report_SodoZian { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ترازنامه")]
	public bool Report_Tarazname { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تراز حسابها")]
	public bool Report_TarazeHesabha { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش اسناد")]
	public bool Report_Asnad { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دفتر کل")]
	public bool Report_Kol { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دفتر کل قانونی")]
	public bool Report_Kol_Sum { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش تفصیلی حساب ها")]
	public bool Report_Tafsil { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش کلی حساب ها")]
	public bool Report_HesabKoli { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش تفکیک یک معین به حساب های  تفصیلی")]
	public bool Report_Moein2Tafsils { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش تفکیک یک تفصیل به حساب های معین")]
	public bool Report_TajmieTafsili { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش تجمعی حساب های تفصیلی")]
	public bool Report_Tafsil2Moeins { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صورت معاملات فصلی")]
	public bool Report_MoamelatFasli { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش تفصیلی چند سطحی")]
	public bool Report_FloatingTafsil { get; set; } = true;


	[PersianNameAttribute.PersianName("گزارش کلی حسابها")]
	public bool Report_Kol_Moein_Tafsil { get; set; } = true;


	[PersianNameAttribute.PersianName("صورت معاملات فصلی")]
	public bool Report_TTMS { get; set; } = true;

}
