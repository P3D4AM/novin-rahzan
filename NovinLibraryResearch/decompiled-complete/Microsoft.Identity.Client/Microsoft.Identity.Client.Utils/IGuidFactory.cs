using System;

namespace Microsoft.Identity.Client.Utils;

internal interface IGuidFactory
{
	Guid NewGuid();
}
