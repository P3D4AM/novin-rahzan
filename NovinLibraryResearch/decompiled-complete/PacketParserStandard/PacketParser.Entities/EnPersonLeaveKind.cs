namespace PacketParser.Entities;

public enum EnPersonLeaveKind : short
{
	[PersianNameAttribute.PersianName("مشخص نشده")]
	None,
	[PersianNameAttribute.PersianName("استعلاجی")]
	Estelaji,
	[PersianNameAttribute.PersianName("استحقاقی")]
	Estehghaghi,
	[PersianNameAttribute.PersianName("تشویقی")]
	Tashvighi,
	[PersianNameAttribute.PersianName("بدون حقوق")]
	BiHoghogh,
	[PersianNameAttribute.PersianName("ماموریت")]
	Mission
}
