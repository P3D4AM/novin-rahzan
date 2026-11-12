using System;
using System.Collections.Generic;

namespace Microsoft.Data.SqlClient;

public sealed class SqlRetryingEventArgs : EventArgs
{
	public int RetryCount { get; private set; }

	public TimeSpan Delay { get; private set; }

	public bool Cancel { get; set; }

	public IList<Exception> Exceptions { get; private set; }

	public SqlRetryingEventArgs(int retryCount, TimeSpan delay, IList<Exception> exceptions)
	{
		RetryCount = retryCount;
		Delay = delay;
		Exceptions = exceptions;
	}
}
