using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalWooPendingRequest
{
	Task<List<WebWooPendingRequest>> GetAllAsync(short depId);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebWooPendingRequest request);

	Task<ReturnedSaveFuncInfo> DeleteAsync(WebWooPendingRequest request);
}
