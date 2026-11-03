using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Interfaces;

public interface IDataItem
{
	Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount, CancellationToken token, string tranName);
}
