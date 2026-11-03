using System.ComponentModel;

namespace PacketParser.Access;

public class ProductTaxAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست مالیات کالاها")]
	public bool ShowForm { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن مالیات کالا جدید")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف مالیات کالا موجود")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش مالیات کالا موجود")]
	public bool Update { get; set; } = true;

}
