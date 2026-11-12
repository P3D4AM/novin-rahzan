using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client.Utils;

internal static class RetryOperationHelper
{
	public static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> func, int maxAttempts, TimeSpan? retryInterval = null, Action<int, Exception> onAttemptFailed = null, ISet<Type> allowedExceptions = null)
	{
		if (func == null)
		{
			throw new ArgumentNullException("func");
		}
		if (maxAttempts < 1)
		{
			throw new ArgumentOutOfRangeException("maxAttempts", maxAttempts, "The maximum number of attempts must not be less than 1.");
		}
		int attempt = 0;
		while (true)
		{
			if (attempt > 0 && retryInterval.HasValue)
			{
				await Task.Delay(retryInterval.Value).ConfigureAwait(continueOnCapturedContext: true);
			}
			try
			{
				return await func().ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				if (allowedExceptions != null && !allowedExceptions.Contains(ex.GetType()))
				{
					throw;
				}
				attempt++;
				onAttemptFailed?.Invoke(attempt, ex);
				if (attempt >= maxAttempts)
				{
					throw;
				}
			}
		}
	}

	public static async Task ExecuteWithRetryAsync(Func<Task> func, int maxAttempts, TimeSpan? retryInterval = null, Action<int, Exception> onAttemptFailed = null, ISet<Type> allowedExceptions = null)
	{
		if (func == null)
		{
			throw new ArgumentNullException("func");
		}
		await ExecuteWithRetryAsync(async delegate
		{
			await func().ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}, maxAttempts, retryInterval, onAttemptFailed, allowedExceptions).ConfigureAwait(continueOnCapturedContext: true);
	}
}
