namespace PacketParser.Entities;

public enum ENHesabType : short
{
	None,
	[PersianNameAttribute.PersianName("دارایی")]
	Darayi,
	[PersianNameAttribute.PersianName("بدهی")]
	Bedehi,
	[PersianNameAttribute.PersianName("سرمایه")]
	Sarmaye,
	[PersianNameAttribute.PersianName("حسابهای سود و زیانی")]
	SodoZiani,
	[PersianNameAttribute.PersianName("حسابهای انتظامی")]
	Entezami
}
