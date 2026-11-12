using System;

namespace Microsoft.Identity.Json.Linq;

[Flags]
internal enum MergeNullValueHandling
{
	Ignore = 0,
	Merge = 1
}
