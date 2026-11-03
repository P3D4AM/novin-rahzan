using System.ComponentModel;

namespace PacketParser.Entities.WordPress;

public class WooSyncBaseProductSettings
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر ابعاد")]
	public bool Dimensions { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر قیمت")]
	public bool ChangePrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر کلاس مالیاتی")]
	public bool ChangeTaxClass { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تخفیف")]
	public bool ChangeDiscount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تاریخ تخفیف")]
	public bool ChangeDiscountDate { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر تصویر")]
	public bool ChangePicture { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر محتوی کامل")]
	public bool ChangeFullContent { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر محتوی مختصر")]
	public bool ChangeQuickText { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر مشخصات فنی")]
	public bool ChangeTechInfo { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر وزن")]
	public bool ChangeWeight { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر SKU")]
	public bool ChangeSku { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر SLUG")]
	public bool ChangeSlug { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر واحد و ویژگی ها")]
	public bool ChangeAttributes { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر مقدار موجودی")]
	public bool ChangeAmount { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("تغییر مدیریت موجودی")]
	public bool ChangeManageStock { get; set; } = true;

}
