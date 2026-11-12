using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.SignalR.Internal;

public class HubOptionsSetup : IConfigureOptions<HubOptions>
{
	private readonly List<string> _protocols = new List<string>();

	internal static TimeSpan DefaultHandshakeTimeout => TimeSpan.FromSeconds(15.0);

	internal static TimeSpan DefaultKeepAliveInterval => TimeSpan.FromSeconds(15.0);

	public HubOptionsSetup(IEnumerable<IHubProtocol> protocols)
	{
		foreach (IHubProtocol protocol in protocols)
		{
			_protocols.Add(protocol.Name);
		}
	}

	public void Configure(HubOptions options)
	{
		if (options.SupportedProtocols == null)
		{
			options.SupportedProtocols = new List<string>();
		}
		if (!options.KeepAliveInterval.HasValue)
		{
			options.KeepAliveInterval = DefaultKeepAliveInterval;
		}
		if (!options.HandshakeTimeout.HasValue)
		{
			options.HandshakeTimeout = DefaultHandshakeTimeout;
		}
		foreach (string protocol in _protocols)
		{
			options.SupportedProtocols.Add(protocol);
		}
	}
}
public class HubOptionsSetup<THub> : IConfigureOptions<HubOptions<THub>> where THub : Hub
{
	private readonly HubOptions _hubOptions;

	public HubOptionsSetup(IOptions<HubOptions> options)
	{
		_hubOptions = options.Value;
	}

	public void Configure(HubOptions<THub> options)
	{
		options.SupportedProtocols = _hubOptions.SupportedProtocols;
		options.KeepAliveInterval = _hubOptions.KeepAliveInterval;
		options.HandshakeTimeout = _hubOptions.HandshakeTimeout;
	}
}
