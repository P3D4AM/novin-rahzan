using System;
using System.ComponentModel;

namespace PacketParser.Access;

public class BardashtAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست برداشت ها")]
	public bool Bardasht_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن برداشت جدید")]
	public bool Bardasht_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف برداشت های موجود")]
	public bool Bardasht_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش برداشت های موجود")]
	public bool Bardasht_Update { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده مانده حساب برداشت")]
	public bool Bardasht_SeeAccount { get; set; } = true;


	[Obsolete]
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گزارش گردش برداشت")]
	public bool Bardasht_Report { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده برداشت های حذف شده")]
	public bool Role_ShowDeleted { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن برداشت های حذف شده")]
	public bool Role_Active { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وضعیت سند برداشت")]
	public bool Bardasht_ChangeStatusSanad { get; set; } = true;

}
