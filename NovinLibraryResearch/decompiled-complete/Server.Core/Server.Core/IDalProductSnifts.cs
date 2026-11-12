using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Department;
using PacketParser.Entities.Snifts;

namespace Server.Core;

public interface IDalProductSnifts
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebLkInfo lk, WebProductSnift item);

	Task<List<WebProductSnift>> GetAllAsync(string prdCode);
}
