using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Department;

namespace Server.Core;

public interface IDalLkInfo
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebLkInfo info, CancellationToken token);
}
