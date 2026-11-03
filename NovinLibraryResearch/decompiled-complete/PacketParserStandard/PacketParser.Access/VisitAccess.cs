using System.ComponentModel;

namespace PacketParser.Access;

public class VisitAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف تور ویزیت")]
	public bool Visit_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لیست  تور ویزیت")]
	public bool Visit_List { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لیست توزیع")]
	public bool ListTozie { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دوره ویزیت مشتریان")]
	public bool VisitPriod { get; set; } = true;

}
