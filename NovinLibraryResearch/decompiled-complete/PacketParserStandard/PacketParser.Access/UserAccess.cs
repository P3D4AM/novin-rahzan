using System.ComponentModel;

namespace PacketParser.Access;

public class UserAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست کاربران")]
	public bool User_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن کاربر جدید")]
	public bool User_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف کاربران")]
	public bool User_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش کاربران")]
	public bool User_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعیین سطح دسترسی کاربران")]
	public bool User_Access { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده کاربران حذف شده")]
	public bool Role_ShowDeleted { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن کاربران حذف شده")]
	public bool Role_Active { get; set; } = true;

}
