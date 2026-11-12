using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Microsoft.AspNetCore.SignalR;

public interface IHubProtocolResolver
{
	IReadOnlyList<IHubProtocol> AllProtocols { get; }

	IHubProtocol GetProtocol(string protocolName, IReadOnlyList<string> supportedProtocols);
}
