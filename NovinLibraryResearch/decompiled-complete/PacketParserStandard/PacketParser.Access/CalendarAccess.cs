using System.ComponentModel;

namespace PacketParser.Access;

public class CalendarAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست مناسبت ها")]
	public bool List { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن مناسبت جدید")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف مناسبت های موجود")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش مناسبت های موجود")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دریافت مناسبت ها از سرور")]
	public bool GetFromServer { get; set; } = true;

}
