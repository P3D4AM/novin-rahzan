using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalCusGroup
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebCusGroup> lst, CancellationToken token);

	Task<List<WebCusGroup>> GetAllAsync(short depId, DateTime Modified, CancellationToken cancellationToken);

	Task<ICusGroup> GetAsync(short depId, Guid groupGuid);

	Task<ICusGroup> GetAsync(short depId, short Id);
}
