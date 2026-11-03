using System;
using System.ComponentModel;

namespace PacketParser.Access;

public class SandoghAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست صندوق ها")]
	public bool Sandogh_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن صندوق جدید")]
	public bool Sandogh_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف صندوق های موجود")]
	public bool Sandogh_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش صندوق های موجود")]
	public bool Sandogh_Update { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گزارش گردش عملکرد صندوق")]
	public bool Sandogh_SeeGardesh { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ گزارش گردش عملکرد صندوق")]
	public bool Sandogh_PrintGardesh { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده موجودی صندوق ها")]
	public bool Sandogh_SeeMojodi { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده صندوق های حذف شده")]
	public bool Role_ShowDeleted { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن صندوق های حذف شده")]
	public bool Role_Active { get; set; } = true;

}
