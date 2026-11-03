using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Access.SanadStatusAccess.DefautAccess.Other;

namespace PacketParser.Access;

public class SarNakhAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست سرنخ ها")]
	public bool SarNakh_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن سرنخ جدید")]
	public bool SarNakh_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف سرنخ موجود")]
	public bool SarNakh_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش سرنخ موجود")]
	public bool SarNakh_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تبدیل سرنخ به مشتری")]
	public bool SarNakh_ChangeToCustomer { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سرنخ فروش")]
	public bool SarNakh_ChangeStatusSanad { get; set; } = true;


	public List<SarNakhSanadStatusAccess> SanadStatusAccess { get; set; } = new List<SarNakhSanadStatusAccess>();


	public SarNakhDefaultSanadStatusAccess SanadStatusDefaultAccess { get; set; } = new SarNakhDefaultSanadStatusAccess();

}
