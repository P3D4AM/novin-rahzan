using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.AspNetCore.SignalR.Internal;

public class DefaultHubProtocolResolver : IHubProtocolResolver
{
	private static class Log
	{
		private static readonly Action<ILogger, string, Type, Exception> _registeredSignalRProtocol = LoggerMessage.Define<string, Type>(LogLevel.Debug, new EventId(1, "RegisteredSignalRProtocol"), "Registered SignalR Protocol: {ProtocolName}, implemented by {ImplementationType}.");

		private static readonly Action<ILogger, string, Exception> _foundImplementationForProtocol = LoggerMessage.Define<string>(LogLevel.Debug, new EventId(2, "FoundImplementationForProtocol"), "Found protocol implementation for requested protocol: {ProtocolName}.");

		public static void RegisteredSignalRProtocol(ILogger logger, string protocolName, Type implementationType)
		{
			_registeredSignalRProtocol(logger, protocolName, implementationType, null);
		}

		public static void FoundImplementationForProtocol(ILogger logger, string protocolName)
		{
			_foundImplementationForProtocol(logger, protocolName, null);
		}
	}

	private readonly ILogger<DefaultHubProtocolResolver> _logger;

	private readonly List<IHubProtocol> _hubProtocols;

	private readonly Dictionary<string, IHubProtocol> _availableProtocols;

	public IReadOnlyList<IHubProtocol> AllProtocols => _hubProtocols;

	public DefaultHubProtocolResolver(IEnumerable<IHubProtocol> availableProtocols, ILogger<DefaultHubProtocolResolver> logger)
	{
		_logger = logger ?? NullLogger<DefaultHubProtocolResolver>.Instance;
		_availableProtocols = new Dictionary<string, IHubProtocol>(StringComparer.OrdinalIgnoreCase);
		_hubProtocols = availableProtocols.ToList();
		foreach (IHubProtocol hubProtocol in _hubProtocols)
		{
			if (_availableProtocols.ContainsKey(hubProtocol.Name))
			{
				throw new InvalidOperationException("Multiple Hub Protocols with the name '" + hubProtocol.Name + "' were registered.");
			}
			Log.RegisteredSignalRProtocol(_logger, hubProtocol.Name, hubProtocol.GetType());
			_availableProtocols.Add(hubProtocol.Name, hubProtocol);
		}
	}

	public virtual IHubProtocol GetProtocol(string protocolName, IReadOnlyList<string> supportedProtocols)
	{
		protocolName = protocolName ?? throw new ArgumentNullException("protocolName");
		if (_availableProtocols.TryGetValue(protocolName, out var value) && (supportedProtocols == null || supportedProtocols.Contains<string>(protocolName, StringComparer.OrdinalIgnoreCase)))
		{
			Log.FoundImplementationForProtocol(_logger, protocolName);
			return value;
		}
		return null;
	}
}
