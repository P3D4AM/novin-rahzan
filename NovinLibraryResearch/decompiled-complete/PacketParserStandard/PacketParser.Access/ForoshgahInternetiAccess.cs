using System.ComponentModel;

namespace PacketParser.Access;

public class ForoshgahInternetiAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صفحه درباره ما")]
	public bool ForoshgahInterneti_DarbareMa { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("صفحه تماس با ما")]
	public bool ForoshgahInterneti_TamasBaMA { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت گروه های کالایی ویژه")]
	public bool ForoshgahInterneti_KalahayeVizhe { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت اخبار")]
	public bool ForoshgahInterneti_Akhbar { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت مقالات")]
	public bool ForoshgahInterneti_Maghalat { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("اطلاعیه عمومی")]
	public bool ForoshgahInterneti_Etelayie { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت تصاویر،لوگو،بنر و الیست اسلایدرها")]
	public bool ForoshgahInterneti_Tasavir { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت پیوندها")]
	public bool ForoshgahInterneti_PeyvandHa { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت تماس با ما")]
	public bool ForoshgahInterneti_TamasBaMA2 { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت درخواست های نمایندگی")]
	public bool ForoshgahInterneti_Namayandegi { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت درخواست های استخدامی")]
	public bool ForoshgahInterneti_Estekhdami { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مدیریت شکایات")]
	public bool ForoshgahInterneti_Shekayat { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال مجدد تمامی داده ها به هاست")]
	public bool ForoshgahInterneti_ErsalMojadad { get; set; } = true;


	[PersianNameAttribute.PersianName("سفارش گیر")]
	public bool ForoshgahInterneti_Sefareshgir { get; set; } = true;

}
