using System.ComponentModel;

namespace PacketParser.Access;

public class ProductPriceLevelAccess
{
	[PersianNameAttribute.PersianName("مشاهده لیست سطوح قیمت کالا")]
	public bool ShowForm { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تعریف سطح قیمت جدید")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش سطح قیمت")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف سطح قیمت")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر ردیف سطح قیمت")]
	public bool ReOrdinal { get; set; } = true;

}
