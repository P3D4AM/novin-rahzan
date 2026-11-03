namespace PacketParser.Entities;

public enum EnGroup : short
{
	[PersianNameAttribute.PersianName("None")]
	None = 1,
	[PersianNameAttribute.PersianName("Connection")]
	Connection = 0,
	[PersianNameAttribute.PersianName("Order")]
	Order = 2,
	[PersianNameAttribute.PersianName("CalleId")]
	CalleId = 3,
	[PersianNameAttribute.PersianName("Gps")]
	Gps = 4,
	[PersianNameAttribute.PersianName("Product")]
	Product = 5,
	[PersianNameAttribute.PersianName("Lk")]
	Lk = 6,
	[PersianNameAttribute.PersianName("Backup")]
	Backup = 7,
	[PersianNameAttribute.PersianName("Exception")]
	Exception = 8,
	[PersianNameAttribute.PersianName("InternetUpdater")]
	InternetUpdater = 9,
	[PersianNameAttribute.PersianName("Sell")]
	Sell = 10,
	[PersianNameAttribute.PersianName("Sell")]
	Customer = 11,
	[PersianNameAttribute.PersianName("scaleTcp")]
	scaleTcp = 12,
	[PersianNameAttribute.PersianName("File")]
	File = 13,
	[PersianNameAttribute.PersianName("SqlContextMonitor")]
	SqlContextMonitor = 14,
	[PersianNameAttribute.PersianName("Trace")]
	Trace = 15,
	[PersianNameAttribute.PersianName("Debug")]
	Debug = 16,
	[PersianNameAttribute.PersianName("Warning")]
	Warning = 17,
	[PersianNameAttribute.PersianName("Information")]
	Information = 18,
	[PersianNameAttribute.PersianName("Reception")]
	Reception = 19
}
