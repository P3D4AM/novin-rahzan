using System.Threading;
using System.Threading.Tasks;

namespace PacketParser.Scale.Services;

public interface IScaleManager
{
	Task Start(CancellationToken token);

	void Force2ReadReceipt();

	void Force2WriteMemories();
}
