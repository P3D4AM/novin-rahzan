using System.ComponentModel;

namespace PacketParser.Access;

public class CouponAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صدور کوپن تخفیف")]
	public bool Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف کوپن تخفیف")]
	public bool Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش کوپن تخفیف")]
	public bool Update { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ کوپن تخفیف")]
	public bool Print { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری کوپن تخفیف")]
	public bool Report { get; set; } = true;

}
