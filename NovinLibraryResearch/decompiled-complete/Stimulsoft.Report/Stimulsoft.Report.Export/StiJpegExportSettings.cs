using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public sealed class StiJpegExportSettings : StiBitmapExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiImageType ImageType
	{
		get
		{
			return StiImageType.Jpeg;
		}
		set
		{
		}
	}

	public StiJpegExportSettings()
		: base(StiImageType.Jpeg)
	{
	}
}
