namespace PacketParser.Entities;

public enum EnKardexType
{
	[PersianNameAttribute.PersianName("کلیه کالاها")]
	AllProduct,
	[PersianNameAttribute.PersianName("کالاهای مانده دار")]
	ProductWithBallance,
	[PersianNameAttribute.PersianName("کالا با مانده غیرمنفی")]
	ProductWithPositiveBalance,
	[PersianNameAttribute.PersianName("کالاهای زیرنقطه سفارش")]
	ProductWithLessThanOrderPoint,
	[PersianNameAttribute.PersianName("کالاها با مانده منفی")]
	ProductWithNegativeBallance,
	[PersianNameAttribute.PersianName("کالاهایی که در گردش دارای مانده منفی هستند")]
	ProductWithNegativeBallanceInTour,
	[PersianNameAttribute.PersianName("کالاهایی که ورود به انبار ندارند")]
	ProductsThatsNotEnteredInDepot,
	[PersianNameAttribute.PersianName("کالاهایی که خروج از انبار ندارند")]
	ProductThatsNotExitedFromDepot
}
