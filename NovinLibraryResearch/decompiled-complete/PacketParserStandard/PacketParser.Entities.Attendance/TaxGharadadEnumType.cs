namespace PacketParser.Entities.Attendance;

public enum TaxGharadadEnumType : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None = -1,
	[PersianNameAttribute.PersianName("در این قرارداد مالیات محاسبه نمیشود")]
	NotTax,
	[PersianNameAttribute.PersianName("در این قرارداد مالیات محاسبه میشود")]
	IncudedTax
}
