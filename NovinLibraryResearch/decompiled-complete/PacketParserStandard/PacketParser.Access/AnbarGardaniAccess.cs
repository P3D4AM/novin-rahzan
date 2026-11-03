using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess;

namespace PacketParser.Access;

public class AnbarGardaniAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست انبارگردانی ها")]
	public bool AnbarGardani_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن انبارگردانی جدید")]
	public bool AnbarGardani_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف انبارگردانی موجود")]
	public bool AnbarGardani_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش انبارگردانی موجود")]
	public bool AnbarGardani_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ انبارگردان")]
	public bool AnbarGardani_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده موجودی فعلی")]
	public bool AnbarGardani_SeeMojodi { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده فی کاردکس")]
	public bool AnbarGardani_SeeKardex { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت اقلام")]
	public bool EditPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند انبار گردانی")]
	public bool AnbarGardani_ChangeStatusSanad { get; set; } = true;


	public List<AnbarGardaniSanadStatusAccess> SanadStatusAccess { get; set; } = new List<AnbarGardaniSanadStatusAccess>();


	public AnbarGardaniDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new AnbarGardaniDefaultSanadStatusAccess();

}
