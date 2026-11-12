using System;

namespace Microsoft.Identity.Client.Utils;

internal class TimeService : ITimeService
{
	public DateTime GetUtcNow()
	{
		return DateTime.UtcNow;
	}
}
