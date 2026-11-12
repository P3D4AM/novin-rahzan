using System;

namespace Stimulsoft.Base.Json;

[Flags]
public enum TypeNameHandling
{
	None = 0,
	Objects = 1,
	Arrays = 2,
	All = 3,
	Auto = 4
}
