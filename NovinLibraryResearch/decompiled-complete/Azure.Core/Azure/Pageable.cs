using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Azure;

public abstract class Pageable<T> : IEnumerable<T>, IEnumerable where T : notnull
{
	private class StaticPageable : Pageable<T>
	{
		private readonly IEnumerable<Page<T>> _pages;

		public StaticPageable(IEnumerable<Page<T>> pages)
		{
			_pages = pages;
		}

		public override IEnumerable<Page<T>> AsPages(string? continuationToken = null, int? pageSizeHint = null)
		{
			return _pages;
		}
	}

	protected virtual CancellationToken CancellationToken { get; }

	protected Pageable()
	{
		CancellationToken = CancellationToken.None;
	}

	protected Pageable(CancellationToken cancellationToken)
	{
		CancellationToken = cancellationToken;
	}

	public abstract IEnumerable<Page<T>> AsPages(string? continuationToken = null, int? pageSizeHint = null);

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public virtual IEnumerator<T> GetEnumerator()
	{
		foreach (Page<T> item in AsPages())
		{
			foreach (T value in item.Values)
			{
				yield return value;
			}
		}
	}

	public static Pageable<T> FromPages(IEnumerable<Page<T>> pages)
	{
		return new StaticPageable(pages);
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
