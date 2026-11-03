namespace PacketParser.Entities;

public enum EnrolType : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None = 100,
	[PersianNameAttribute.PersianName("اثر انگشت")]
	FingerPrint = 0,
	[PersianNameAttribute.PersianName("رمز")]
	Password = 1,
	[PersianNameAttribute.PersianName("کارت")]
	Card = 2,
	[PersianNameAttribute.PersianName("صورت")]
	Face = 3,
	[PersianNameAttribute.PersianName("کف دست")]
	Palm = 4
}
