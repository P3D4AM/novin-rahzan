using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalProductGroup
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebPrdGroup> lst, CancellationToken cancellationToken, short tryCount = 10);

	Task<WebPrdGroup> LoadDataAsync(short DepId, short Id, CancellationToken cancellationToken, short tryCount = 10);

	Task<WebPrdGroup> LoadDataAsync(short DepId, Guid groupGuid, CancellationToken cancellationToken, short tryCount = 10);

	Task<List<WebPrdGroup>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken, short tryCount = 10);

	Task<List<WebPrdGroup>> SearchAsync(short depId, CancellationToken token, Dictionary<string, string> filters, Dictionary<string, string> orderBy);
}
