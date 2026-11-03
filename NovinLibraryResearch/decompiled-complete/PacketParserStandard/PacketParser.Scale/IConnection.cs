using System.Threading;
using System.Threading.Tasks;

namespace PacketParser.Scale;

public interface IConnection
{
	Task SendAsync(string data, CancellationToken token = default(CancellationToken));
}
