using System;

namespace Microsoft.AspNetCore.SignalR.Client;

public interface IRetryPolicy
{
	TimeSpan? NextRetryDelay(RetryContext retryContext);
}
