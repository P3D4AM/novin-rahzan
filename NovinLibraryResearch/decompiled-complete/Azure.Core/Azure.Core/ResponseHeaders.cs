using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Azure.Core;

public readonly struct ResponseHeaders : IEnumerable<HttpHeader>, IEnumerable
{
	private readonly Response _response;

	public DateTimeOffset? Date
	{
		get
		{
			if (!TryGetValue(HttpHeader.Names.Date, out string value) && !TryGetValue(HttpHeader.Names.XMsDate, out value))
			{
				return null;
			}
			return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture);
		}
	}

	public string? ContentType
	{
		get
		{
			if (!TryGetValue(HttpHeader.Names.ContentType, out string value))
			{
				return null;
			}
			return value;
		}
	}

	public int? ContentLength
	{
		get
		{
			if (!TryGetValue(HttpHeader.Names.ContentLength, out string value))
			{
				return null;
			}
			return int.Parse(value, CultureInfo.InvariantCulture);
		}
	}

	public ETag? ETag
	{
		get
		{
			if (!TryGetValue(HttpHeader.Names.ETag, out string value))
			{
				return null;
			}
			return Azure.ETag.Parse(value);
		}
	}

	public string? RequestId
	{
		get
		{
			if (!TryGetValue(HttpHeader.Names.XMsRequestId, out string value))
			{
				return null;
			}
			return value;
		}
	}

	internal ResponseHeaders(Response response)
	{
		_response = response;
	}

	public IEnumerator<HttpHeader> GetEnumerator()
	{
		return _response.EnumerateHeaders().GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _response.EnumerateHeaders().GetEnumerator();
	}

	public bool TryGetValue(string name, [NotNullWhen(true)] out string? value)
	{
		return _response.TryGetHeader(name, out value);
	}

	public bool TryGetValues(string name, [NotNullWhen(true)] out IEnumerable<string>? values)
	{
		return _response.TryGetHeaderValues(name, out values);
	}

	public bool Contains(string name)
	{
		return _response.ContainsHeader(name);
	}
}
