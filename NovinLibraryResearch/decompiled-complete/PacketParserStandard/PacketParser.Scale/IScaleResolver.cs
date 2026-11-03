using System.Net;
using System.Threading.Tasks;
using PacketParser.Scale.Entities;

namespace PacketParser.Scale;

public interface IScaleResolver
{
	Task<ScaleResolutionResult> ResolveIp(IPAddress remoteAddress);

	Task<ScaleResolutionResult> ResolveIp(string serialPortName);
}
