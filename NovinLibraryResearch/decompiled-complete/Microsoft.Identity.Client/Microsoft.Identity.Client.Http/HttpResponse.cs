using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace Microsoft.Identity.Client.Http;

internal class HttpResponse
{
	public HttpResponseHeaders Headers { get; set; }

	public IDictionary<string, string> HeadersAsDictionary
	{
		get
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (Headers != null)
			{
				foreach (KeyValuePair<string, IEnumerable<string>> header in Headers)
				{
					dictionary[header.Key] = header.Value.First();
				}
			}
			return dictionary;
		}
	}

	public HttpStatusCode StatusCode { get; set; }

	public string UserAgent { get; set; }

	public string Body { get; set; }
}
