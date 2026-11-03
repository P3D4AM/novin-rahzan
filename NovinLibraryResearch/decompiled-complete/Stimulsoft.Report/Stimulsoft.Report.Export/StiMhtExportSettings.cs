using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiMhtExportSettings : StiHtmlExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiHtmlType HtmlType
	{
		get
		{
			return StiHtmlType.Mht;
		}
		set
		{
		}
	}

	public StiMhtExportSettings()
		: base(StiHtmlType.Mht)
	{
	}
}
