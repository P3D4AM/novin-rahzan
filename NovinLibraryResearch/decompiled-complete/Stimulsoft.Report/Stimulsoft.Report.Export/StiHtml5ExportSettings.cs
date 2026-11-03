using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiHtml5ExportSettings : StiHtmlExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiHtmlType HtmlType
	{
		get
		{
			return StiHtmlType.Html5;
		}
		set
		{
		}
	}

	public StiHtml5ExportSettings()
		: base(StiHtmlType.Html5)
	{
	}
}
