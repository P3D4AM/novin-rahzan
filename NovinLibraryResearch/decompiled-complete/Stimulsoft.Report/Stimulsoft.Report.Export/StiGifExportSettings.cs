using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public sealed class StiGifExportSettings : StiBitmapExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiImageType ImageType
	{
		get
		{
			return StiImageType.Gif;
		}
		set
		{
		}
	}

	public StiGifExportSettings()
		: base(StiImageType.Gif)
	{
	}
}
