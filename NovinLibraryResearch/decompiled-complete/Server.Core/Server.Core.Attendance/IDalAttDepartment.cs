using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.EventArgs;

namespace Server.Core.Attendance;

public interface IDalAttDepartment
{
	Task<List<SaveResponse>> SaveAsync(short DepId, List<WebAttDepartment> lst, CancellationToken token);
}
