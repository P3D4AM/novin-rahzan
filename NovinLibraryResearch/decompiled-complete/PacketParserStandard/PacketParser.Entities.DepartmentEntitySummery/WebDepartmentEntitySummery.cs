using PacketParser.Entities.DepartmentEntitySummery.Entities;

namespace PacketParser.Entities.DepartmentEntitySummery;

public class WebDepartmentEntitySummery
{
	public WebAghsatMEntitySummery AghsatM { get; set; } = new WebAghsatMEntitySummery();


	public WebAghsatShEntitySummery AghsatSh { get; set; } = new WebAghsatShEntitySummery();


	public WebChequeMEntitySummery ChequeM { get; set; } = new WebChequeMEntitySummery();


	public WebChequeShEntitySummery ChequeSh { get; set; } = new WebChequeShEntitySummery();


	public WebCustomerEntitySummery Customer { get; set; } = new WebCustomerEntitySummery();


	public WebCustomerGroupEntitySummery CustomerGroup { get; set; } = new WebCustomerGroupEntitySummery();


	public WebCustomerTitleEntitySummery CustomerTitle { get; set; } = new WebCustomerTitleEntitySummery();


	public WebKolEntitySummery Kol { get; set; } = new WebKolEntitySummery();


	public WebMoeinEntitySummery Moein { get; set; } = new WebMoeinEntitySummery();


	public WebTafsilEntitySummery Tafsil { get; set; } = new WebTafsilEntitySummery();


	public WebTafsilGroupEntitySummery TafsilGroup { get; set; } = new WebTafsilGroupEntitySummery();


	public WebLastPriceEntitySummery LastPrice { get; set; } = new WebLastPriceEntitySummery();


	public WebUserEntitySummery User { get; set; } = new WebUserEntitySummery();


	public WebProductEntitySummery Product { get; set; } = new WebProductEntitySummery();


	public WebProductGroupEntitySummery ProductGroup { get; set; } = new WebProductGroupEntitySummery();


	public WebProductVahedEntitySummery ProductVahed { get; set; } = new WebProductVahedEntitySummery();


	public WebFileInfoEntitySummery FileInfo { get; set; } = new WebFileInfoEntitySummery();


	public WebIntroductionMethodSummery IntroductionMethod { get; set; }
}
