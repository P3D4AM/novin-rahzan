using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalNote
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashId, ENSource source, List<WebNote> lst, CancellationToken token);

	Task<WebNote> GetAsync(short depId, Guid guid);

	Task<List<WebNote>> GetAllAsync(short depId);
}
