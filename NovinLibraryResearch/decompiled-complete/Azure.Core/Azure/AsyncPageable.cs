using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Azure;

public abstract class AsyncPageable<T> : IAsyncEnumerable<T> where T : notnull
{
	private class StaticPageable : AsyncPageable<T>
	{
		private readonly IEnumerable<Page<T>> _pages;

		public StaticPageable(IEnumerable<Page<T>> pages)
		{
			_pages = pages;
		}

		public override async IAsyncEnumerable<Page<T>> AsPages(string? continuationToken = null, int? pageSizeHint = null)
		{
			foreach (Page<T> page in _pages)
			{
				yield return page;
			}
		}
	}

	protected virtual CancellationToken CancellationToken { get; }

	protected AsyncPageable()
	{
		CancellationToken = CancellationToken.None;
	}

	protected AsyncPageable(CancellationToken cancellationToken)
	{
		CancellationToken = cancellationToken;
	}

	public abstract IAsyncEnumerable<Page<T>> AsPages(string? continuationToken = null, int? pageSizeHint = null);

	public virtual async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
	{
		await foreach (Page<T> item in AsPages().ConfigureAwait(continueOnCapturedContext: false).WithCancellation(cancellationToken))
		{
			foreach (T value in item.Values)
			{
				yield return value;
			}
		}
	}

	public static AsyncPageable<T> FromPages(IEnumerable<Page<T>> pages)
	{
		return new StaticPageable(pages);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object? obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
