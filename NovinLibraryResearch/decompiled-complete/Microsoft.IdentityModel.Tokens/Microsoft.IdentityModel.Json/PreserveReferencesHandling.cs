using System;

namespace Microsoft.IdentityModel.Json;

[Flags]
internal enum PreserveReferencesHandling
{
	None = 0,
	Objects = 1,
	Arrays = 2,
	All = 3
}
