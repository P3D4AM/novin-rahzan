using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Message;

namespace Server.Core;

public interface IDalMessageSession
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebMessageSession messageSession);

	Task<List<WebMessageSession>> GetAllAsync(short depID, CancellationToken token);

	Task<List<WebMessageSession>> GetAllAsync(CancellationToken token);

	Task<WebMessageSession> GetAsync(short depID, long sender, CancellationToken token);

	Task<bool> Update_LastStatus_Async(short depId, ENTelegramSessionStatus en, long sender, CancellationToken token);

	Task<bool> DeleteSession(long sender, short depID);

	Task<ReturnedSaveFuncInfo> SaveLogAsync(WebMessageLog log);

	Task<List<WebMessageLog>> GetLogsAsync(short depID, long senderNumber, CancellationToken token);
}
