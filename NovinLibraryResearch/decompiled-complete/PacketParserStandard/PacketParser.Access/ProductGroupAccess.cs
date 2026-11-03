using System.ComponentModel;

namespace PacketParser.Access;

public class ProductGroupAccess
{
	[PersianNameAttribute.PersianName("مشاهده لیست گروه های کالا")]
	public bool Role_ShowForm { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف گروه جدید")]
	public bool Role_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش گروه")]
	public bool Role_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف گروه")]
	public bool Role_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گروه های حذف شده")]
	public bool Role_ShowDeleted { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن گروه های حذف شده")]
	public bool Role_Active { get; set; } = true;

}
