using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public sealed class StiBmpExportSettings : StiBitmapExportSettings
{
	[JsonConverter(typeof(StringEnumConverter))]
	public override StiImageType ImageType
	{
		get
		{
			return StiImageType.Bmp;
		}
		set
		{
		}
	}

	public StiBmpExportSettings()
		: base(StiImageType.Bmp)
	{
	}
}
