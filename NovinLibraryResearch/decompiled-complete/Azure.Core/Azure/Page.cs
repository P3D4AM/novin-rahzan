using System.Collections.Generic;
using System.ComponentModel;

namespace Azure;

public abstract class Page<T>
{
	private class PageCore : Page<T>
	{
		private readonly Response _response;

		public override IReadOnlyList<T> Values { get; }

		public override string? ContinuationToken { get; }

		public PageCore(IReadOnlyList<T> values, string? continuationToken, Response response)
		{
			_response = response;
			Values = values;
			ContinuationToken = continuationToken;
		}

		public override Response GetRawResponse()
		{
			return _response;
		}
	}

	public abstract IReadOnlyList<T> Values { get; }

	public abstract string? ContinuationToken { get; }

	public abstract Response GetRawResponse();

	public static Page<T> FromValues(IReadOnlyList<T> values, string? continuationToken, Response response)
	{
		return new PageCore(values, continuationToken, response);
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
