namespace PacketParser.Entities;

public enum ConnectionType : short
{
	[PersianNameAttribute.PersianName("انتخاب ترازو")]
	None,
	[PersianNameAttribute.PersianName("محک قبل سال 90")]
	Mahak_Rs232,
	[PersianNameAttribute.PersianName("محک بعد از سال 90")]
	Mahak_RS485,
	[PersianNameAttribute.PersianName("محک TCP")]
	Mahak_TCPIP,
	[PersianNameAttribute.PersianName("محک Serial")]
	Mahak_Serial,
	[PersianNameAttribute.PersianName("کارین Serial")]
	Karin_Serial,
	[PersianNameAttribute.PersianName("کارین Wifi دو فلش")]
	Karin_Wifi_2Flash,
	[PersianNameAttribute.PersianName("صدر LSG")]
	Sadr_LSG,
	[PersianNameAttribute.PersianName("رادین")]
	Radin,
	[PersianNameAttribute.PersianName("دیبال")]
	Dibal,
	[PersianNameAttribute.PersianName("پند TCP")]
	Pand_TcpClient,
	[PersianNameAttribute.PersianName("پند Serial")]
	Pand_Serial,
	[PersianNameAttribute.PersianName("رادین Serial")]
	Radin_Serial,
	[PersianNameAttribute.PersianName("تکین Serial")]
	Takin_Serial,
	[PersianNameAttribute.PersianName("صدر Serial ")]
	Sadr_Serial,
	[PersianNameAttribute.PersianName("رادین Label_Printer ")]
	Radin_Label_Printer,
	[PersianNameAttribute.PersianName("کارین Wifi تک فلش")]
	Karin_Wifi_takFlash,
	[PersianNameAttribute.PersianName("PosScale نوین پرداز")]
	NovinPardaz_PosScale,
	[PersianNameAttribute.PersianName("PosScale نوین پرداز شبکه")]
	NovinPardaz_PosScaleTcp,
	[PersianNameAttribute.PersianName("صدر NewSerial ")]
	Sadr_NewSerial
}
