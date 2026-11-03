namespace PacketParser.Entities;

public enum EnDataType : short
{
	Byte = 1,
	sByte,
	Short,
	uShort,
	[PersianNameAttribute.PersianName("عدد صحیح")]
	Int,
	uInt,
	Long,
	uLong,
	Float,
	[PersianNameAttribute.PersianName("عدد اعشاری")]
	Double,
	[PersianNameAttribute.PersianName("مقادیر پولی")]
	Decimal,
	Char,
	Bool,
	[PersianNameAttribute.PersianName("انتخابی")]
	Object,
	[PersianNameAttribute.PersianName("رشته")]
	String,
	[PersianNameAttribute.PersianName("تاریخ")]
	DateTime,
	UniqueIdentifier
}
