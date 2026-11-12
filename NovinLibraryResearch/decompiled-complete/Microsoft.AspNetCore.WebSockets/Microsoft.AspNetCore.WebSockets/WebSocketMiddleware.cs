using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebSockets.Internal;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.WebSockets;

public class WebSocketMiddleware
{
	private class UpgradeHandshake : IHttpWebSocketFeature
	{
		private readonly HttpContext _context;

		private readonly IHttpUpgradeFeature _upgradeFeature;

		private readonly WebSocketOptions _options;

		public bool IsWebSocketRequest
		{
			get
			{
				if (!_upgradeFeature.IsUpgradableRequest)
				{
					return false;
				}
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				foreach (string neededHeader in HandshakeHelpers.NeededHeaders)
				{
					string[] commaSeparatedValues = _context.Request.Headers.GetCommaSeparatedValues(neededHeader);
					foreach (string value in commaSeparatedValues)
					{
						list.Add(new KeyValuePair<string, string>(neededHeader, value));
					}
				}
				return HandshakeHelpers.CheckSupportedWebSocketRequest(_context.Request.Method, list);
			}
		}

		public UpgradeHandshake(HttpContext context, IHttpUpgradeFeature upgradeFeature, WebSocketOptions options)
		{
			_context = context;
			_upgradeFeature = upgradeFeature;
			_options = options;
		}

		public async Task<WebSocket> AcceptAsync(WebSocketAcceptContext acceptContext)
		{
			if (!IsWebSocketRequest)
			{
				throw new InvalidOperationException("Not a WebSocket request.");
			}
			string subProtocol = null;
			if (acceptContext != null)
			{
				subProtocol = acceptContext.SubProtocol;
			}
			TimeSpan keepAliveInterval = _options.KeepAliveInterval;
			_ = _options.ReceiveBufferSize;
			if (acceptContext is ExtendedWebSocketAcceptContext { ReceiveBufferSize: var receiveBufferSize } extendedWebSocketAcceptContext)
			{
				if (receiveBufferSize.HasValue)
				{
					_ = extendedWebSocketAcceptContext.ReceiveBufferSize.Value;
				}
				if (extendedWebSocketAcceptContext.KeepAliveInterval.HasValue)
				{
					keepAliveInterval = extendedWebSocketAcceptContext.KeepAliveInterval.Value;
				}
			}
			foreach (KeyValuePair<string, string> item in HandshakeHelpers.GenerateResponseHeaders(string.Join(", ", (string?[]?)_context.Request.Headers["Sec-WebSocket-Key"]), subProtocol))
			{
				_context.Response.Headers[item.Key] = item.Value;
			}
			return WebSocketProtocol.CreateFromStream(await _upgradeFeature.UpgradeAsync(), isServer: true, subProtocol, keepAliveInterval);
		}
	}

	private readonly RequestDelegate _next;

	private readonly WebSocketOptions _options;

	public WebSocketMiddleware(RequestDelegate next, IOptions<WebSocketOptions> options)
	{
		if (next == null)
		{
			throw new ArgumentNullException("next");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		_next = next;
		_options = options.Value;
	}

	public Task Invoke(HttpContext context)
	{
		IHttpUpgradeFeature httpUpgradeFeature = context.Features.Get<IHttpUpgradeFeature>();
		if (httpUpgradeFeature != null && context.Features.Get<IHttpWebSocketFeature>() == null)
		{
			context.Features.Set((IHttpWebSocketFeature)new UpgradeHandshake(context, httpUpgradeFeature, _options));
		}
		return _next(context);
	}
}
