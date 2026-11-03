namespace PacketParser.Scale;

public enum ConnectionStatus : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("برقراری ارتباط...")]
	Connecting,
	[PersianNameAttribute.PersianName("آنلاین")]
	Connected,
	[PersianNameAttribute.PersianName("آفلاین")]
	Disconnected
}
