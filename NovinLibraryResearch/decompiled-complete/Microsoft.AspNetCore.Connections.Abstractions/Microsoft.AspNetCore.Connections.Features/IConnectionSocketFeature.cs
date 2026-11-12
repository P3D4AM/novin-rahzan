using System.Net.Sockets;

namespace Microsoft.AspNetCore.Connections.Features;

public interface IConnectionSocketFeature
{
	Socket Socket { get; }
}
