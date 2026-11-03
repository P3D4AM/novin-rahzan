namespace PacketParser.Entities;

public enum EnConnectionState
{
	[PersianNameAttribute.PersianName("در حال اتصال")]
	Connecting,
	[PersianNameAttribute.PersianName("متصل")]
	Connected,
	[PersianNameAttribute.PersianName("اتصال مجدد")]
	Reconnecting,
	[PersianNameAttribute.PersianName("عدم اتصال")]
	Disconnected,
	[PersianNameAttribute.PersianName("از بین رفته")]
	Disposed
}
