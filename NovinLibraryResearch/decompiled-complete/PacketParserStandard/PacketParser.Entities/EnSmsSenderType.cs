namespace PacketParser.Entities;

public enum EnSmsSenderType : short
{
	[PersianNameAttribute.PersianName("سامانه اینترنتی")]
	Web,
	[PersianNameAttribute.PersianName("سیم کارت")]
	Gsm,
	[PersianNameAttribute.PersianName("SmsBan")]
	SmsBan
}
