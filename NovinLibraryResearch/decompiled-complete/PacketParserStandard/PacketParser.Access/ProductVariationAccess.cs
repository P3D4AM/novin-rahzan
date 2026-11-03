using System.ComponentModel;

namespace PacketParser.Access;

public class ProductVariationAccess
{
	[PersianNameAttribute.PersianName("مشاهده لیست تنوع ها و ویژگی های کالا")]
	public bool ProductVariation_ShowForm { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف تنوع و ویژگی کالا")]
	public bool ProductVariation_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش تنوع و ویژگی کالا")]
	public bool ProductVariation_Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف تنوع و ویژگی کالا")]
	public bool ProductVariation_Delete { get; set; } = true;

}
