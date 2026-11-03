using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiElementStyleIdent
{
	Auto = 1,
	Blue,
	Orange,
	Green,
	Turquoise,
	SlateGray,
	DarkBlue,
	DarkGray,
	DarkGreen,
	DarkTurquoise,
	Silver,
	AliceBlue,
	Sienna,
	Custom
}
