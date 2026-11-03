using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.CodeDom;

[StiServiceBitmap(typeof(StiLanguage), "Stimulsoft.Report.Bmp.Languages.LanguageCSharp.bmp")]
public class StiCSharpLanguage : StiLanguage
{
	private StiCSharpCodeProvider provider;

	public override StiCodeDomProvider GetProvider()
	{
		return provider ?? (provider = new StiCSharpCodeProvider());
	}

	public override string ToString()
	{
		return "CSharp";
	}
}
