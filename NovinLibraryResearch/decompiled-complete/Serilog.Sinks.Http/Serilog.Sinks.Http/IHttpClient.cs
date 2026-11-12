using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Serilog.Sinks.Http;

public interface IHttpClient : IDisposable
{
	void Configure(IConfiguration configuration);

	Task<HttpResponseMessage> PostAsync(string requestUri, Stream contentStream);
}
