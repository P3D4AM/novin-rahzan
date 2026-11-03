using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public sealed class StiSvgExportSettings : StiImageExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiImageType ImageType
	{
		get
		{
			return StiImageType.Svg;
		}
		set
		{
		}
	}

	public StiSvgExportSettings()
		: base(StiImageType.Svg)
	{
	}
}
