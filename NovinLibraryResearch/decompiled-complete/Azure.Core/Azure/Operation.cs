using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Azure;

public abstract class Operation<T> where T : notnull
{
	public abstract string Id { get; }

	public abstract T Value { get; }

	public abstract bool HasCompleted { get; }

	public abstract bool HasValue { get; }

	public abstract Response GetRawResponse();

	public abstract ValueTask<Response<T>> WaitForCompletionAsync(CancellationToken cancellationToken = default(CancellationToken));

	public abstract ValueTask<Response<T>> WaitForCompletionAsync(TimeSpan pollingInterval, CancellationToken cancellationToken);

	public abstract ValueTask<Response> UpdateStatusAsync(CancellationToken cancellationToken = default(CancellationToken));

	public abstract Response UpdateStatus(CancellationToken cancellationToken = default(CancellationToken));

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}
}
