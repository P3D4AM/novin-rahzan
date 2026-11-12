using System;
using System.Net;

namespace Microsoft.Identity.Client.Platforms.Shared.Desktop.OsBrowser;

internal class MessageAndHttpCode
{
	public HttpStatusCode HttpCode { get; }

	public string Message { get; }

	public MessageAndHttpCode(HttpStatusCode httpCode, string message)
	{
		HttpCode = httpCode;
		Message = message ?? throw new ArgumentNullException("message");
	}
}
