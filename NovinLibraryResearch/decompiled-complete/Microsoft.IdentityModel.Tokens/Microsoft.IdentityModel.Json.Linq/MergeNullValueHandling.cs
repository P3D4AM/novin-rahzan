using System;

namespace Microsoft.IdentityModel.Json.Linq;

[Flags]
internal enum MergeNullValueHandling
{
	Ignore = 0,
	Merge = 1
}
