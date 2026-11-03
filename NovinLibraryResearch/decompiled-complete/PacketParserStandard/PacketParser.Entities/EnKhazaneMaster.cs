namespace PacketParser.Entities;

public enum EnKhazaneMaster : short
{
	[PersianNameAttribute.PersianName("دریافت")]
	Reception = 1,
	[PersianNameAttribute.PersianName("پرداخت")]
	Pardakht
}
