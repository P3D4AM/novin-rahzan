using System.ComponentModel;

namespace PacketParser.Access;

public class WageAccess
{
	[PersianNameAttribute.PersianName("مشاهده لیست کارمزدهای بانکی")]
	public bool Wage_ShowForm { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف کارمزد جدید")]
	public bool Wage_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش کارمزد")]
	public bool Wage_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف کارمزد")]
	public bool Wage_Delete { get; set; } = true;

}
