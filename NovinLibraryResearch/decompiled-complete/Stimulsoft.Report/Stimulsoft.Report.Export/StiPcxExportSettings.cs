using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public sealed class StiPcxExportSettings : StiBitmapExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiImageType ImageType
	{
		get
		{
			return StiImageType.Pcx;
		}
		set
		{
		}
	}

	public StiPcxExportSettings()
		: base(StiImageType.Pcx)
	{
	}
}
