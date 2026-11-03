using System.ComponentModel;

namespace PacketParser.Access;

public class DepotAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست انبارها")]
	public bool Depot_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن انبار جدید")]
	public bool Depot_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف انبارهای موجود")]
	public bool Depot_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش انبارهای موجود")]
	public bool Depot_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده انبارهای حذف شده")]
	public bool Role_ShowDeleted { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن انبارهای حذف شده")]
	public bool Role_Active { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند انبار ")]
	public bool Depot_ChangeStatusSanad { get; set; } = true;

}
