namespace PacketParser.Entities;

public enum SMSSendType
{
	[PersianNameAttribute.PersianName("فاقد سرویس ویژه")]
	none,
	[PersianNameAttribute.PersianName("استعلام قیمت /ثبت سفارش پیامکی")]
	MojodoKala_SaveOrder,
	[PersianNameAttribute.PersianName("")]
	Temp,
	[PersianNameAttribute.PersianName("سریال و کد گارانتی")]
	SerialKala
}
