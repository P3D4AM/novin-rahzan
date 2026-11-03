namespace PacketParser.Entities;

public enum EnCardType : short
{
	[PersianNameAttribute.PersianName("رفتن به منوی خاص")]
	GoToMenu,
	[PersianNameAttribute.PersianName("افزودن کالا به فاکتور")]
	IncreaseProduct,
	[PersianNameAttribute.PersianName("Action")]
	Action,
	[PersianNameAttribute.PersianName("حذف کالا از فاکتور")]
	RemoveProduct,
	[PersianNameAttribute.PersianName("ذخیره نقدی")]
	SaveCash,
	[PersianNameAttribute.PersianName("ذخیره نقدی و چاپ")]
	SaveCashAndPrint,
	[PersianNameAttribute.PersianName("ذخیره")]
	Save,
	[PersianNameAttribute.PersianName("ذخیره و چاپ")]
	SaveAndPrint,
	[PersianNameAttribute.PersianName("ذخیره کارتخوان 1")]
	Poss1,
	[PersianNameAttribute.PersianName("دخیره کارتخوان 1 و چاپ")]
	Poss1AndPrint,
	[PersianNameAttribute.PersianName("ذخیره کارتخوان 2")]
	Poss2,
	[PersianNameAttribute.PersianName("ذخیره کارتخوان 2 و چاپ")]
	Poss2AndPrint,
	[PersianNameAttribute.PersianName("ذخیره کارتخوان 3")]
	Poss3,
	[PersianNameAttribute.PersianName("ذخیره کارتخوان 3 و چاپ")]
	Poss3AndPrint,
	[PersianNameAttribute.PersianName("ذخیره کارتخوان 4")]
	Poss4,
	[PersianNameAttribute.PersianName("ذخیره کارتخوان 4 و چاپ")]
	Poss4AndPrint,
	[PersianNameAttribute.PersianName("ثبت فاکتور جدید")]
	NewOrder,
	[PersianNameAttribute.PersianName("فاکتور قبل")]
	PreviewsOrder,
	[PersianNameAttribute.PersianName("فاکتور بعد")]
	NextOrder,
	[PersianNameAttribute.PersianName("اولین فاکتور امروز")]
	FirstTodayOrder,
	[PersianNameAttribute.PersianName("آخرین فاکتور امروز")]
	LastTodayOrder,
	[PersianNameAttribute.PersianName("ابطال فاکتور")]
	RemoveOrder,
	[PersianNameAttribute.PersianName("گروه کالا")]
	DynamicGroups,
	[PersianNameAttribute.PersianName("میزها")]
	Placements,
	[PersianNameAttribute.PersianName("مدیریت میز")]
	PlacementManager,
	[PersianNameAttribute.PersianName("اتصال میز به فاکتور")]
	PlacementOrderRelation,
	[PersianNameAttribute.PersianName("تسویه محل پذیرایی")]
	PlacementSettlement
}
