using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class FilterFactorForReport
{
	[DefaultValue(true)]
	public bool DisplayDate { get; set; } = true;


	[DefaultValue(true)]
	public bool DisplayDescription { get; set; }

	[DefaultValue(true)]
	public bool DisplayHesab { get; set; }

	[DefaultValue(true)]
	public bool DisplayNumber { get; set; }

	[DefaultValue(true)]
	public bool DisplayPrice { get; set; }

	[DefaultValue(true)]
	public bool DisplayUser { get; set; }

	[DefaultValue(true)]
	public bool VisitorID { get; set; }

	[DefaultValue(true)]
	public bool VisitorPrice { get; set; }

	[DefaultValue(true)]
	public bool Maliat { get; set; }

	[DefaultValue(true)]
	public bool Discount { get; set; }

	[DefaultValue(true)]
	public bool Fair { get; set; }

	[DefaultValue(true)]
	public bool SelectedHesab { get; set; }

	[DefaultValue(true)]
	public bool Masir { get; set; }

	[DefaultValue(true)]
	public bool CustGroup { get; set; }

	[DefaultValue(true)]
	public bool Source { get; set; }

	[DefaultValue(true)]
	public bool CMSDate { get; set; } = true;


	[DefaultValue(true)]
	public bool CMSCode { get; set; } = true;


	[DefaultValue(false)]
	public bool CMSMoein { get; set; }

	[DefaultValue(false)]
	public bool CMSServerStatus { get; set; }

	[DefaultValue(false)]
	public bool CMSSanadAndKardexAffective { get; set; }

	[DefaultValue(false)]
	public bool CMSTaxResult { get; set; }
}
