namespace PacketParser.Entities;

public enum EnDeviceMessageType : short
{
	[PersianNameAttribute.PersianName("AddOeUpdate")]
	AddOrUpdate,
	[PersianNameAttribute.PersianName("Delete")]
	Delete
}
