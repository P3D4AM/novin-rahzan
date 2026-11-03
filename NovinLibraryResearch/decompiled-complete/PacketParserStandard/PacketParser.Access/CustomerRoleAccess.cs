using System.ComponentModel;

namespace PacketParser.Access;

public class CustomerRoleAccess
{
	[PersianNameAttribute.PersianName("مشاهده لیست نقش های اشخاص")]
	public bool Role_ShowForm { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف نقش جدید")]
	public bool Role_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش نقش")]
	public bool Role_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف نقش")]
	public bool Role_Delete { get; set; } = true;

}
