namespace Stimulsoft.Report.Helpers;

public class StiIsoCountry
{
	public string[] Names { get; set; }

	public string[] RuNames { get; set; }

	public string[] FrNames { get; set; }

	public string Alpha2 { get; set; }

	public string Alpha3 { get; set; }

	internal StiIsoCountry Ru(params string[] names)
	{
		RuNames = names;
		return this;
	}

	internal StiIsoCountry Fr(params string[] names)
	{
		FrNames = names;
		return this;
	}

	internal StiIsoCountry Iso(string alpha2, string alpha3 = null)
	{
		Alpha2 = alpha2;
		Alpha3 = alpha3;
		return this;
	}

	public StiIsoCountry(params string[] names)
	{
		Names = names;
	}
}
