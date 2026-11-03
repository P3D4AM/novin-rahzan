namespace PacketParser.Entities;

public enum DependencyType : short
{
	[PersianNameAttribute.PersianName("متصل")]
	Dependent,
	[PersianNameAttribute.PersianName("مستقل")]
	Independent,
	[PersianNameAttribute.PersianName("پیوست شده")]
	Attached
}
