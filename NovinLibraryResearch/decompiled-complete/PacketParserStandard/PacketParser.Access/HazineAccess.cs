using System;
using System.ComponentModel;

namespace PacketParser.Access;

public class HazineAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست هزینه ها")]
	public bool Hazine_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن هزینه جدید")]
	public bool Hazine_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف هزینه های موجود")]
	public bool Hazine_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش هزینه های موجود")]
	public bool Hazine_Update { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده مانده حساب هزینه")]
	public bool Hazine_SeeAccount { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گزارش گردش هزینه")]
	public bool Hazine_Report { get; set; } = true;

}
