namespace PacketParser.Entities;

public enum EnUserRole : short
{
	[PersianNameAttribute.PersianName("کاربر عادی")]
	RegularUser,
	[PersianNameAttribute.PersianName("مدیر واحد")]
	DepartmentAdmin,
	[PersianNameAttribute.PersianName("مدیر کل")]
	Manager
}
