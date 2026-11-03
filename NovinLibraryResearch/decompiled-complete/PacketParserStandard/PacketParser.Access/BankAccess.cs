using System;
using System.ComponentModel;

namespace PacketParser.Access;

public class BankAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست بانک ها")]
	public bool Bank_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن بانک جدید")]
	public bool Bank_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف بانک های موجود")]
	public bool Bank_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش بانک های موجود")]
	public bool Bank_Update { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گزارش گردش عملکرد بانک")]
	public bool Bank_SeeGardesh { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ گزارش گردش عملکرد بانک")]
	public bool Bank_PrintGardesh { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده موجودی بانک ها")]
	public bool Bank_SeeMojodi { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده بانک های حذف شده")]
	public bool Role_ShowDeleted { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن بانک های حذف شده")]
	public bool Role_Active { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش نقدینگی بانک ها")]
	public bool BankFinanceReport { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست بانک ها")]
	public bool Report { get; set; } = true;

}
