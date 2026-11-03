namespace PacketParser.Settings.Accounting;

public class CompanySettings
{
	public string Fax { get; set; } = "";


	public string Address { get; set; } = "";


	public string WebSite { get; set; } = "";


	public string Telephone { get; set; } = "";


	public string Mobile { get; set; } = "";


	public string CompanyTitle { get; set; } = "عنوان دوره مالی";


	public string CompanyZip { get; set; } = "";


	public string CompanyEmail { get; set; } = "";


	public string CompanyInfo { get; set; } = "";


	public string CompanyMeli { get; set; } = "";


	public string Web_Pages_About { get; set; } = "";


	public string Web_Pages_ContactUS { get; set; } = "";


	public decimal Latitude { get; set; }

	public decimal Longitude { get; set; }

	public byte[] CompanyLogo { get; set; }

	public string CompanyState { get; set; } = "";


	public string CompanyCity { get; set; } = "";


	public short SanadType { get; set; }
}
