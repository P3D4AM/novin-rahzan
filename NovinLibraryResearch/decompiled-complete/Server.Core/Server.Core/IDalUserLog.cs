using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalUserLog
{
	Task<List<SaveResponse>> SaveAsync(short depId, long hashId, ENSource source, List<WebCashierLog> lst, CancellationToken token);

	Task<List<WebCashierLog>> GetAllAsync(short depId, DateTime modified, CancellationToken cancellationToken);

	Task<WebCashierLog> GetAsync(Guid guid, short depId);
}
