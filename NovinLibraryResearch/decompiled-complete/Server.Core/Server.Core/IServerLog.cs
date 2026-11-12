using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace Server.Core;

public interface IServerLog
{
	Task<ReturnedSaveFuncInfo> InsertLogAsync(string ip, short part, short subPart, string description, CancellationToken token, short tryCount = 10);
}
