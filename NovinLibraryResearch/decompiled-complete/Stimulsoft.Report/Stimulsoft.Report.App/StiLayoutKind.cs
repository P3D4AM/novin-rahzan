using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.App;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiLayoutKind
{
	Canvas = 1,
	DockPanel,
	WrapPanel,
	HorizontalStackPanel,
	VerticalStackPanel
}
