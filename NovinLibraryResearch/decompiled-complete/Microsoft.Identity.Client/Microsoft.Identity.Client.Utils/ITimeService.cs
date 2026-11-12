using System;

namespace Microsoft.Identity.Client.Utils;

internal interface ITimeService
{
	DateTime GetUtcNow();
}
