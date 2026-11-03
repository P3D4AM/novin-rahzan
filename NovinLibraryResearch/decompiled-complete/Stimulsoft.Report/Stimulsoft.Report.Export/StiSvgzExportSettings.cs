using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public sealed class StiSvgzExportSettings : StiImageExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiImageType ImageType
	{
		get
		{
			return StiImageType.Svgz;
		}
		set
		{
		}
	}

	public StiSvgzExportSettings()
		: base(StiImageType.Svgz)
	{
	}
}
