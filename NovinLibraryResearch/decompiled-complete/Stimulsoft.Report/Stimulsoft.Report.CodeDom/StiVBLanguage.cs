using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.CodeDom;

[StiServiceBitmap(typeof(StiLanguage), "Stimulsoft.Report.Bmp.Languages.LanguageVB.bmp")]
public class StiVBLanguage : StiLanguage
{
	private StiVBCodeProvider provider;

	public override StiCodeDomProvider GetProvider()
	{
		return provider ?? (provider = new StiVBCodeProvider());
	}

	public override string ToString()
	{
		return "VB";
	}
}
