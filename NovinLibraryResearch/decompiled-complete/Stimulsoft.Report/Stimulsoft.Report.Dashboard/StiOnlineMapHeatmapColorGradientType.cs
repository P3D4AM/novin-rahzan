using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiOnlineMapHeatmapColorGradientType
{
	BlackAquaWhite,
	BlueRed,
	DeepSea,
	ColorSpectrum,
	Incandescent,
	HeatedMetal,
	Sunrise,
	SteppedColors,
	VisibleSpectrum,
	Custom
}
