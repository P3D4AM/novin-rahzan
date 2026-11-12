using System;

namespace Microsoft.Extensions.Internal;

public class SystemClock : ISystemClock
{
	public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
