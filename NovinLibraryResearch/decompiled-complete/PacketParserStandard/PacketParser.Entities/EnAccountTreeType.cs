namespace PacketParser.Entities;

public enum EnAccountTreeType : short
{
	[PersianNameAttribute.PersianName("درختواره حساب ها")]
	KolMoeinTafsil = 1,
	[PersianNameAttribute.PersianName("درختواره حساب ها به تفکیک گروه تفصیلی")]
	KolMoeinTafsilGroupTafsil
}
