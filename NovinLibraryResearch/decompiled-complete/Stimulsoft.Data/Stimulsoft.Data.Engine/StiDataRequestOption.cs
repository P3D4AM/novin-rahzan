using System;

namespace Stimulsoft.Data.Engine;

[Flags]
public enum StiDataRequestOption
{
	None = 0,
	All = 3,
	AllowOpenConnections = 1,
	AllowDataSort = 2,
	DisallowTransform = 4
}
