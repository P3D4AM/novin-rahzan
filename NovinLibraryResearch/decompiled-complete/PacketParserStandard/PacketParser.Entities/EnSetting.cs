namespace PacketParser.Entities;

public enum EnSetting : short
{
	[PersianNameAttribute.PersianName("تنظیمات قلم برنامه")]
	Font,
	[PersianNameAttribute.PersianName("مشخصات دوره مالی")]
	Company,
	[PersianNameAttribute.PersianName("تنظیمات اشخاص")]
	Customer,
	[PersianNameAttribute.PersianName("تنظیمات چاپ")]
	Print,
	[PersianNameAttribute.PersianName("تنظیمات پشتیبان گیری")]
	BackUp,
	[PersianNameAttribute.PersianName("تنظیمات چک پرینت")]
	ChequePrint,
	[PersianNameAttribute.PersianName("یادآوری چک ها و اقساط")]
	Reminder,
	[PersianNameAttribute.PersianName("تنظیمات کاردکس کالا")]
	Kardex,
	[PersianNameAttribute.PersianName("تنظیمات کدینگ و سند")]
	Coding,
	[PersianNameAttribute.PersianName("تنظیمات ترازوی دیجیتال")]
	Scale,
	[PersianNameAttribute.PersianName("تنظیمات فاکتور")]
	Order,
	[PersianNameAttribute.PersianName("تنظیمات فاکتور فروشگاهی")]
	InvoiceOrder,
	[PersianNameAttribute.PersianName("تنظیمات عمومی")]
	Public,
	[PersianNameAttribute.PersianName("تنظیمات کیوسک")]
	Kiosk,
	[PersianNameAttribute.PersianName("تنظیمات سامانه مودیان")]
	TaxService,
	[PersianNameAttribute.PersianName("تنظیمات کارتخوان")]
	PcPoss,
	[PersianNameAttribute.PersianName("تنظیمات انبار")]
	Depot,
	[PersianNameAttribute.PersianName("تنظیمات اسناد اتومات")]
	Document,
	[PersianNameAttribute.PersianName("تنظیمات شرح های اتومات")]
	Description,
	[PersianNameAttribute.PersianName("تنظیمات رسید تحویل کالا")]
	Receipt,
	[PersianNameAttribute.PersianName("همگام سازی با سرور")]
	Server,
	[PersianNameAttribute.PersianName("تنظیمات قرعه کشی")]
	Lottery,
	[PersianNameAttribute.PersianName("تنظیمات پیامک")]
	Sms,
	[PersianNameAttribute.PersianName("تنظیمات کالاها")]
	Product,
	[PersianNameAttribute.PersianName("تنظیمات بارکدخوان")]
	Barcode,
	[PersianNameAttribute.PersianName("تنظیمات وام و اقساط")]
	Installment,
	[PersianNameAttribute.PersianName("تنظیمات رزرواسیون")]
	Reservation,
	[PersianNameAttribute.PersianName("تاریخ و بازه های زمانی")]
	DateAndTime,
	[PersianNameAttribute.PersianName("تنظیمات سرویس قفل")]
	LockService
}
