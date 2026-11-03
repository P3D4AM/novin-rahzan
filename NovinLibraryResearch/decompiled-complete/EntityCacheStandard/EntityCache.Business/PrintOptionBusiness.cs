using PacketParser;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class PrintOptionBusiness : IHasContext
{
	[PersianNameAttribute.PersianName("شماره سطر")]
	public bool ShowRadif { get; set; } = true;


	[PersianNameAttribute.PersianName("شماره فاکتور")]
	public bool ShowNo { get; set; } = true;


	[PersianNameAttribute.PersianName("تاریخ")]
	public bool ShowDate { get; set; } = true;


	[PersianNameAttribute.PersianName("زمان")]
	public bool ShowTime { get; set; } = true;


	[PersianNameAttribute.PersianName("کد مشتری")]
	public bool ShowCodeCus { get; set; } = true;


	[PersianNameAttribute.PersianName("نام مشتری")]
	public bool ShowCusName { get; set; } = true;


	[PersianNameAttribute.PersianName("تلفن")]
	public bool ShowTell1 { get; set; } = true;


	[PersianNameAttribute.PersianName("تلفن دو")]
	public bool ShowTell2 { get; set; } = true;


	[PersianNameAttribute.PersianName("فکس")]
	public bool ShowFax { get; set; } = true;


	[PersianNameAttribute.PersianName("موبایل یک")]
	public bool ShowMobile1 { get; set; } = true;


	[PersianNameAttribute.PersianName("موبایل دو")]
	public bool ShowMobile2 { get; set; } = true;


	[PersianNameAttribute.PersianName("ادرس دو")]
	public bool ShowAdd1 { get; set; } = true;


	[PersianNameAttribute.PersianName("ادرس دو")]
	public bool ShowAdd2 { get; set; } = true;


	[PersianNameAttribute.PersianName("مانده پس از")]
	public bool ShowAfterAccount { get; set; } = true;


	[PersianNameAttribute.PersianName("مانده فاکتور")]
	public bool ShowBeforeAccount { get; set; } = true;


	[PersianNameAttribute.PersianName("مانده قبلی + جمع این فاکتور")]
	public bool ShowBeforAddFactorSum { get; set; } = true;


	[PersianNameAttribute.PersianName("کرایه")]
	public bool ShowKeraye { get; set; } = true;


	[PersianNameAttribute.PersianName("جمع کل")]
	public bool ShowTotalSum { get; set; } = true;


	[PersianNameAttribute.PersianName("جمع چک")]
	public bool ShowSum_Check { get; set; } = true;


	[PersianNameAttribute.PersianName("جمع حواله")]
	public bool ShowSum_Havale { get; set; } = true;


	[PersianNameAttribute.PersianName("جمع تخفیفات سطری")]
	public bool ShowDiscountSum { get; set; } = true;


	[PersianNameAttribute.PersianName("جمع خدمات")]
	public bool ShowSumKhadamat { get; set; } = true;


	[PersianNameAttribute.PersianName("جمع سطر")]
	public bool ShowRowSum { get; set; } = true;


	[PersianNameAttribute.PersianName("جمع دریافت")]
	public bool ShowReception_Sum { get; set; } = true;


	[PersianNameAttribute.PersianName("جمع کالا")]
	public bool ShowSumKala { get; set; } = true;


	[PersianNameAttribute.PersianName("درصد تخفیفات سطری")]
	public bool ShowRowDiscountPercent { get; set; } = true;


	[PersianNameAttribute.PersianName("شرح سطر")]
	public bool ShowRowDesc { get; set; } = true;


	[PersianNameAttribute.PersianName("تخفیف")]
	public bool ShowDiscount { get; set; } = true;


	[PersianNameAttribute.PersianName("مبلغ تخفیف سطر")]
	public bool ShowRowDiscount { get; set; } = true;


	[PersianNameAttribute.PersianName("کد کالا")]
	public bool ShowPRDCode { get; set; } = true;


	[PersianNameAttribute.PersianName("نام کالا")]
	public bool ShowPRDName { get; set; } = true;


	[PersianNameAttribute.PersianName("مقدار")]
	public bool ShowQT { get; set; } = true;


	[PersianNameAttribute.PersianName("مقدار واحد 2")]
	public bool ShowQT2 { get; set; } = true;


	[PersianNameAttribute.PersianName("ضریب واحد")]
	public bool ShowV2QT { get; set; } = true;


	[PersianNameAttribute.PersianName("فی واحد")]
	public bool ShowUnitPrice { get; set; } = true;


	[PersianNameAttribute.PersianName("نام واحد")]
	public bool ShowVahedName { get; set; } = true;


	[PersianNameAttribute.PersianName("نام واحد دو")]
	public bool ShowVahedName2 { get; set; } = true;


	[PersianNameAttribute.PersianName("مشخصات شزکت")]
	public bool CompanyInfo { get; set; } = true;


	[PersianNameAttribute.PersianName("توضیحات")]
	public bool Description { get; set; } = true;


	[PersianNameAttribute.PersianName("جمع واحد دوم")]
	public bool CollectItems { get; set; } = true;


	[PersianNameAttribute.PersianName("تعداد اقلام")]
	public bool RowCount { get; set; } = true;


	public string ContextGuid { get; set; }

	public PrintOptionBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
