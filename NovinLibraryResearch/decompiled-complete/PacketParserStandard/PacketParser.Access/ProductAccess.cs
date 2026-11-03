using System.ComponentModel;

namespace PacketParser.Access;

public class ProductAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده لیست کالاها")]
	public bool Product_See { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن کالای جدید")]
	public bool Product_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن واحد کالای جدید")]
	public bool ProductVahed_Insert { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف کالاهای موجود")]
	public bool Product_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("حذف واحد کالا")]
	public bool ProductVahed_Delete { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش کالاهای موجود")]
	public bool Product_Upadte { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ویرایش واحد کالاهای موجود")]
	public bool ProductVahed_Upadte { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست قیمت")]
	public bool Product_PrintPriceList { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ لیست قیمت همراه با تصویر")]
	public bool Product_PrintPriceListPic { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ کاتالوگ")]
	public bool Product_PrintCatalog { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ بارکد")]
	public bool Product_PrintBarcode { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده کالاهای زیر نقطه سفارش")]
	public bool Product_SeePointOrder { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("چاپ کالاهای زیر نقطه سفارش")]
	public bool Product_PrintPointOrder { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده گزارش کاردکس کالا")]
	public bool Product_Cardex { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده و چاپ سود و زیان یک کالا")]
	public bool Product_SodoZian { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده و چاپ گزارش کالا و طرف حساب")]
	public bool Product_ProductCustomer { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("بازسازی کدینگ کالاها")]
	public bool Product_BazsaziCode { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ثبت اتوماتیک و افزایش قیمت کالاها")]
	public bool Product_SabtPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده موجودی کالا")]
	public bool Product_SeeMojoodi { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دسترسی به کالاهای خاص")]
	public bool Product_DastrasiBeKalahayeKhas { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("اصلاح موجودی ابتدای دوره")]
	public bool Product_UpdateMojodiFirst { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارشات ادواری")]
	public bool Product_RPT_Factors { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارشات ادواری تولید")]
	public bool Product_RPT_Tolid { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("سایر گزارشات مربوط به کالاها")]
	public bool Product_RPT_OtherReports { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارشات تجمیعی کالاها")]
	public bool Product_RPT_SumReports { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ثبت تخفیفات سطری کالاها")]
	public bool Product_Takhfif { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال و دریافت داده ها از طریق فایل اکسل")]
	public bool Product_Exel { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لیست کالاهای انتخابی")]
	public bool Product_SelectionList { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("سریال کالاهای ابتدای دوره")]
	public bool Product_SerialA2re { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("سریال کالاها")]
	public bool Product_SerialList { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده آخرین فی یا بهای تمام شده کالا")]
	public bool SeeLastPrice { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده کالاهای حذف شده")]
	public bool Role_ShowDeletedPrd { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن کالاهای حذف شده")]
	public bool Role_ActivePrd { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("مشاهده واحدهای حذف شده کالا")]
	public bool Role_ShowDeletedPrdUnit { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("فعال کردن واحدهای حذف شده کالا")]
	public bool Role_ActiveUnit { get; set; } = true;

}
