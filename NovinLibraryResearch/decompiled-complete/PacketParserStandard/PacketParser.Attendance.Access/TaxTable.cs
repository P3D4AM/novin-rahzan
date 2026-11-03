using System.ComponentModel;

namespace PacketParser.Attendance.Access;

public class TaxTable
{
	[PersianNameAttribute.PersianName("جدول مالیات")]
	[DefaultValue(true)]
	public bool TaxtableMain { get; set; } = true;


	[PersianNameAttribute.PersianName("ایجاد جدول مالیات")]
	[DefaultValue(true)]
	public bool TaxtableInsert { get; set; } = true;


	[PersianNameAttribute.PersianName("حذف جدول مالیات")]
	[DefaultValue(true)]
	public bool TaxtableDelete { get; set; } = true;


	[PersianNameAttribute.PersianName("ویرایش جدول مالیات")]
	[DefaultValue(true)]
	public bool TaxtableUpdate { get; set; } = true;

}
