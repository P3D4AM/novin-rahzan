namespace PacketParser.Entities;

public enum EnOrderBarcodeDetect : short
{
	[PersianNameAttribute.PersianName("بی اثر")]
	None,
	[PersianNameAttribute.PersianName("نمایش اطلاعات فاکتور")]
	LoadOrder,
	[PersianNameAttribute.PersianName("تغییر وضعیت فاکتور به وضعیت بعد")]
	ChangeStatus,
	[PersianNameAttribute.PersianName("نمایش سوال به صورت موردی")]
	AskQuestion,
	[PersianNameAttribute.PersianName("ارسال مستقیم به کارتخوان")]
	PcPoss
}
