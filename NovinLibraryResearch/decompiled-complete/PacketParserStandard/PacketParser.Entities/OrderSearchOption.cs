namespace PacketParser.Entities;

public enum OrderSearchOption
{
	[PersianNameAttribute.PersianName("فاکتوری که صادر می شود در کاردکس موثر باشد")]
	KardexAffect,
	[PersianNameAttribute.PersianName("زمان صدور هر فاکتور، سند حسابداری آن هم صادر شود")]
	SanadAffect
}
