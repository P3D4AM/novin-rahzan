using System.Threading;

namespace Microsoft.AspNetCore.Http;

public class HttpContextAccessor : IHttpContextAccessor
{
	private static AsyncLocal<HttpContext> _httpContextCurrent = new AsyncLocal<HttpContext>();

	public HttpContext HttpContext
	{
		get
		{
			return _httpContextCurrent.Value;
		}
		set
		{
			_httpContextCurrent.Value = value;
		}
	}
}
