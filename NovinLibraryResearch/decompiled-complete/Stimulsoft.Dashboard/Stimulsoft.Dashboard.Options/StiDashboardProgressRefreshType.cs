using System;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Dashboard.Options;

[Flags]
[JsonConverter(typeof(StringEnumConverter))]
public enum StiDashboardProgressRefreshType
{
	DoEvents = 0,
	Invalidate = 1
}
