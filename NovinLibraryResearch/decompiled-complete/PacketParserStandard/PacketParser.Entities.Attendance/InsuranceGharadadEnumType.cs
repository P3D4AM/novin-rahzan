namespace PacketParser.Entities.Attendance;

public enum InsuranceGharadadEnumType : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None = -1,
	[PersianNameAttribute.PersianName("در این قرارداد بیمه محاسبه نمیشود")]
	NotInsured,
	[PersianNameAttribute.PersianName("براساس حداقل قانون کار محاسبه شود")]
	LeastLawLabor,
	[PersianNameAttribute.PersianName("براساس محاسبات فیش حقوقی محاسبه شود")]
	PayloadCalculations
}
