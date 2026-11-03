using System.ComponentModel;
using PacketParser.Entities.Attendance;

namespace PacketParser.Attendance.Access;

public class Accounting
{
	[PersianNameAttribute.PersianName("مشاهد، صدور و اصلاح اسناد حسابداری")]
	[AttHelpURL("New_Item65.htm")]
	[DefaultValue(true)]
	public bool AccountingMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد سند حسابداری")]
	[DefaultValue(true)]
	public bool AccountingInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف سند حسابداری")]
	[DefaultValue(true)]
	public bool AccountingDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش سند حسابداری")]
	[DefaultValue(true)]
	public bool AccountingUpdate { get; set; } = true;


	[PersianNameAttribute.PersianName("نمایش سند حسابداری")]
	[DefaultValue(true)]
	public bool AccountingDisplay { get; set; } = true;


	[PersianNameAttribute.PersianName("گزارش سند حسابداری")]
	[DefaultValue(true)]
	public bool AccountingReport { get; set; } = true;


	[PersianNameAttribute.PersianName("چاپ سند حسابداری")]
	[DefaultValue(true)]
	public bool AccountingPrint { get; set; } = true;


	[PersianNameAttribute.PersianName("انتقال سند به حسابداری")]
	[DefaultValue(true)]
	public bool AccountingMove { get; set; } = true;

}
