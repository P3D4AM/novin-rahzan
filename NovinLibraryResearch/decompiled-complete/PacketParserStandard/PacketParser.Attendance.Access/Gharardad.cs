using System.ComponentModel;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Gharardad
{
	[PersianNameAttribute.PersianName("قرارداد کاری")]
	[AttHelpURL("New_Item42.htm")]
	[DefaultValue(true)]
	public bool GharardadMain { get; set; } = true;


	[PersianNameAttribute.PersianName("افزدون قرارداد جدید")]
	[DefaultValue(true)]
	public bool GharardadInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف یک قرارداد")]
	[DefaultValue(true)]
	public bool GharardadDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش یک قرارداد")]
	[DefaultValue(true)]
	public bool GharardadUpdate { get; set; } = true;


	[PersianNameAttribute.PersianName("چاپ لیست قراردادها")]
	[DefaultValue(true)]
	public bool GharardadPrintKol { get; set; } = true;


	[PersianNameAttribute.PersianName("چاپ قرارداد")]
	[DefaultValue(true)]
	public bool GharardadPrint { get; set; } = true;


	[PersianNameAttribute.PersianName("ثبت اطلاعات استخدامی کارکنان ")]
	[DefaultValue(true)]
	public bool GharardadEstekhdami { get; set; } = true;


	[PersianNameAttribute.PersianName("ثبت انواع مزایا و کسورات در قرارداد کاری ")]
	[DefaultValue(true)]
	public bool GharardadCURDMazayaKosorat { get; set; } = true;


	[PersianNameAttribute.PersianName("امکان ثبت پایه سنوات، حق مسکن، حق اولاد و حق خواروبار ")]
	[DefaultValue(true)]
	public bool GharardadHaghs { get; set; } = true;

}
