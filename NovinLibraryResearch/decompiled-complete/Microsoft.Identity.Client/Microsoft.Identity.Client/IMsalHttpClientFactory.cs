using System.Net.Http;

namespace Microsoft.Identity.Client;

public interface IMsalHttpClientFactory
{
	HttpClient GetHttpClient();
}
