namespace PacketParser.Entities;

public enum EnTaxInvoiceTemplate
{
	[PersianNameAttribute.PersianName("انتخاب نشده")]
	None,
	[PersianNameAttribute.PersianName("الگوی فروش")]
	Sell,
	[PersianNameAttribute.PersianName("الگوی فروش ارزی")]
	DecimalSell,
	[PersianNameAttribute.PersianName("صورتحساب طلا، جواهر و پلاتین")]
	Jewelry,
	[PersianNameAttribute.PersianName("الگوی 4 پیمانکاری")]
	Contracting,
	[PersianNameAttribute.PersianName("قبوض خدماتی")]
	UtilityBills,
	[PersianNameAttribute.PersianName("بلیط هواپیما")]
	Airfare,
	[PersianNameAttribute.PersianName("الگوی صادرات")]
	Export
}
