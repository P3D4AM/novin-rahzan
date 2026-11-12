using System;

namespace Microsoft.Identity.Client.Utils;

internal class GuidFactory : IGuidFactory
{
	public Guid NewGuid()
	{
		return Guid.NewGuid();
	}
}
