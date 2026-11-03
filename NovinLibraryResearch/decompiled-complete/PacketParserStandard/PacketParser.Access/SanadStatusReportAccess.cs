using System.ComponentModel;

namespace PacketParser.Access;

public class SanadStatusReportAccess
{
	[PersianNameAttribute.PersianName("مشاهده لیست وضعیت اسناد")]
	public bool SanadStatus_ShowForm { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف وضعیت جدید")]
	public bool SanadStatus_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش وضعیت سند")]
	public bool SanadStatus_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف وضعیت سند")]
	public bool SanadStatus_Delete { get; set; } = true;

}
