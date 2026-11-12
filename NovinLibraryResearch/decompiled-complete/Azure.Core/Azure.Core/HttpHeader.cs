using System;

namespace Azure.Core;

public readonly struct HttpHeader : IEquatable<HttpHeader>
{
	public static class Names
	{
		public static string Date => "Date";

		public static string XMsDate => "x-ms-date";

		public static string ContentType => "Content-Type";

		public static string ContentLength => "Content-Length";

		public static string ETag => "ETag";

		public static string XMsRequestId => "x-ms-request-id";

		public static string UserAgent => "User-Agent";

		public static string Accept => "Accept";

		public static string Authorization => "Authorization";

		public static string Range => "Range";

		public static string XMsRange => "x-ms-range";

		public static string IfMatch => "If-Match";

		public static string IfNoneMatch => "If-None-Match";

		public static string IfModifiedSince => "If-Modified-Since";

		public static string IfUnmodifiedSince => "If-Unmodified-Since";

		public static string Referer => "Referer";

		public static string Host => "Host";

		public static string ContentDisposition => "Content-Disposition";
	}

	public static class Common
	{
		private const string ApplicationJson = "application/json";

		private const string ApplicationOctetStream = "application/octet-stream";

		private const string ApplicationFormUrlEncoded = "application/x-www-form-urlencoded";

		public static readonly HttpHeader JsonContentType = new HttpHeader(Names.ContentType, "application/json");

		public static readonly HttpHeader JsonAccept = new HttpHeader(Names.Accept, "application/json");

		public static readonly HttpHeader OctetStreamContentType = new HttpHeader(Names.ContentType, "application/octet-stream");

		public static readonly HttpHeader FormUrlEncodedContentType = new HttpHeader(Names.ContentType, "application/x-www-form-urlencoded");
	}

	public string Name { get; }

	public string Value { get; }

	public HttpHeader(string name, string value)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("name shouldn't be null or empty", "name");
		}
		Name = name;
		Value = value;
	}

	public override int GetHashCode()
	{
		HashCodeBuilder hashCodeBuilder = default(HashCodeBuilder);
		hashCodeBuilder.Add<string>(Name, StringComparer.OrdinalIgnoreCase);
		hashCodeBuilder.Add(Value);
		return hashCodeBuilder.ToHashCode();
	}

	public override bool Equals(object? obj)
	{
		if (obj is HttpHeader other)
		{
			return Equals(other);
		}
		return false;
	}

	public override string ToString()
	{
		return Name + ":" + Value;
	}

	public bool Equals(HttpHeader other)
	{
		if (string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase))
		{
			return Value.Equals(other.Value, StringComparison.Ordinal);
		}
		return false;
	}
}
