using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Microsoft.IdentityModel.Protocols;

public class HttpRequestData
{
	private IDictionary<string, IEnumerable<string>> _headers = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase);

	public Uri Uri { get; set; }

	public string Method { get; set; }

	public byte[] Body { get; set; }

	public IDictionary<string, IEnumerable<string>> Headers
	{
		get
		{
			return _headers;
		}
		set
		{
			_headers = value ?? throw new ArgumentNullException("Headers");
		}
	}

	public IDictionary<string, object> PropertyBag { get; set; }

	public void AppendHeaders(HttpHeaders headers)
	{
		if (headers == null)
		{
			return;
		}
		foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
		{
			if (Headers.ContainsKey(header.Key))
			{
				Headers[header.Key] = Headers[header.Key].Concat(header.Value);
			}
			else
			{
				Headers.Add(header.Key, header.Value);
			}
		}
	}
}
