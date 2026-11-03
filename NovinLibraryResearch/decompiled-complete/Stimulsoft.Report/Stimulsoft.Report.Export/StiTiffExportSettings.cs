using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public sealed class StiTiffExportSettings : StiBitmapExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiImageType ImageType
	{
		get
		{
			return StiImageType.Tiff;
		}
		set
		{
		}
	}

	public StiTiffExportSettings()
		: base(StiImageType.Tiff)
	{
	}
}
