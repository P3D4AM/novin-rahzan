using System.ComponentModel;

namespace PacketParser.Access;

public class ServicesAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست خدمات")]
	public bool Service_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن خدمت جدید")]
	public bool Service_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف خدمات موجود")]
	public bool Service_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش خدمات موجود")]
	public bool Service_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست خدمات")]
	public bool Service_Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده خدمات حذف شده")]
	public bool Role_ShowDeleted { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن خدمات حذف شده")]
	public bool Role_Active { get; set; } = true;

}
