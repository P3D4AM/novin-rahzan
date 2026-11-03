using System.ComponentModel;

namespace PacketParser.Access;

public class DiscountAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور تخفیف حجمی")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف تخفیف حجمی")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش تخفیف حجمی")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ تخفیف حجمی")]
	public bool Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری تخفیفات حجمی")]
	public bool Report { get; set; } = true;

}
