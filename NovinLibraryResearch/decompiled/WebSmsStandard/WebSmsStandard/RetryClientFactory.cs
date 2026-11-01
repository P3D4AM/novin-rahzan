using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebSmsStandard;

internal class RetryClientFactory
{
	public class RetryHandler : DelegatingHandler
	{
		private readonly int _maxRetries = 3;

		private readonly int _delay = 2000;

		public RetryHandler(HttpMessageHandler innerHandler, int maxRetries, int delay)
			: base(innerHandler)
		{
			_maxRetries = maxRetries;
			_delay = delay;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			HttpResponseMessage response = null;
			for (int i = 0; i < _maxRetries; i++)
			{
				response = await base.SendAsync(request, cancellationToken);
				if (response.IsSuccessStatusCode)
				{
					return response;
				}
				await Task.Delay((int)((new Random().NextDouble() + 0.5) * (double)_delay));
			}
			return response;
		}
	}

	private int _maxRetries;

	private int _delay;

	public RetryClientFactory(int maxRetries = 5, int delay = 2000)
	{
		_maxRetries = maxRetries;
		_delay = delay;
	}

	public HttpClient GetClient()
	{
		return new HttpClient(new RetryHandler(new HttpClientHandler(), _maxRetries, _delay));
	}
}
