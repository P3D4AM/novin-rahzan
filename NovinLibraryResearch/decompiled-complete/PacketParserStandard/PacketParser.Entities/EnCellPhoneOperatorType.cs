namespace PacketParser.Entities;

public enum EnCellPhoneOperatorType : byte
{
	[PersianNameAttribute.PersianName("نا مشخص")]
	None,
	[PersianNameAttribute.PersianName("ایرانسل")]
	IranCell,
	[PersianNameAttribute.PersianName("همراه اول")]
	HamraheAval,
	[PersianNameAttribute.PersianName("رایتل")]
	Rightel
}
