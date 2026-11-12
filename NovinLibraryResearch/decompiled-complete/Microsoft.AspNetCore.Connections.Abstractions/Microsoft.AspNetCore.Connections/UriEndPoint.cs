using System;
using System.Net;

namespace Microsoft.AspNetCore.Connections;

public class UriEndPoint : EndPoint
{
	public Uri Uri { get; }

	public UriEndPoint(Uri uri)
	{
		Uri = uri ?? throw new ArgumentNullException("uri");
	}

	public override string ToString()
	{
		return Uri.ToString();
	}
}
