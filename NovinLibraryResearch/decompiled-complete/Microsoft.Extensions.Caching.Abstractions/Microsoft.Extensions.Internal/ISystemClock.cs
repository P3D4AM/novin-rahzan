using System;

namespace Microsoft.Extensions.Internal;

public interface ISystemClock
{
	DateTimeOffset UtcNow { get; }
}
