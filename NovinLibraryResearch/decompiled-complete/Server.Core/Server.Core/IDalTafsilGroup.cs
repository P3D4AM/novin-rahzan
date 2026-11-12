using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalTafsilGroup
{
	Task<List<SaveResponse>> SaveAsync(short depId, long hashid, ENSource source, IEnumerable<WebTafsilGroup> lst, CancellationToken token, int tryCount = 10);

	Task<List<WebTafsilGroup>> GetAllAsync(short depId, DateTime modified, CancellationToken cancellationToken);

	Task<WebTafsilGroup> GetAsync(short depId, Guid guid);
}
