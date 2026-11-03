using System.ComponentModel;

namespace PacketParser.Access;

public class HesabAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده چارت حسابها")]
	public bool Hesab_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف حساب")]
	public bool Hesab_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مسدود کردن حساب ")]
	public bool Hesab_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش حساب")]
	public bool Hesab_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده مانده موجودی")]
	public bool Hesab_SeeAccount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست حسابها")]
	public bool Hesab_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لیست حساب های کل")]
	public bool Hesab_Kol { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لیست حساب های معین")]
	public bool Hesab_Moein { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف حساب معین")]
	public bool Moein_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش حساب معین")]
	public bool Moein_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف حساب معین")]
	public bool Moein_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لیست حساب های تفصیلی")]
	public bool Hesab_Tafsil { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ایجاد حساب های تفصیلی")]
	public bool Tafsil_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش حساب های تفصیلی")]
	public bool Tafsil_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف حساب های تفصیلی")]
	public bool Tafsil_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده حساب های تفصیلی حذف شده")]
	public bool Role_ShowDeletedTafsil { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن حساب های تفصیلی حذف شده")]
	public bool Role_ActiveTafsil { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده حساب های معین حذف شده")]
	public bool Role_ShowDeletedMoein { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن حساب های معین حذف شده")]
	public bool Role_ActiveMoein { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده حساب های کل حذف شده")]
	public bool Role_ShowDeletedKol { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن حساب های کل حذف شده")]
	public bool Role_ActiveKol { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گروه های تفصیلی")]
	public bool Hesab_TafsilLevel { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لیست گروه حساب ها")]
	public bool ShowHesabGroupList { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف گروه حساب")]
	public bool InsertHesabGroup { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش گروه حساب")]
	public bool UpdateHesabGroup { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف گروه حساب")]
	public bool DeleteHesabGroup { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن گروه حساب حذف شده")]
	public bool ChangeStatusHesabGroup { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست گروه حساب ها")]
	public bool PrintHesabGroup { get; set; } = true;

}
