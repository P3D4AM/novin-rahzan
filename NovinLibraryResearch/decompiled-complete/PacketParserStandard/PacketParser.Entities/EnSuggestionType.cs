namespace PacketParser.Entities;

public enum EnSuggestionType
{
	[PersianNameAttribute.PersianName("پر کردن کدهای خالی")]
	FillGaps = 1,
	[PersianNameAttribute.PersianName("بزرگترین عدد بعدی")]
	NextBiggest,
	[PersianNameAttribute.PersianName("عدم پیشنهاد")]
	NotSuggestion
}
