using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalProductVahed
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebPrdVahed> lst, CancellationToken token);

	Task<List<WebPrdVahed>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken);

	Task<List<WebPrdVahed>> SearchAsync(short depId, CancellationToken token, Dictionary<string, string> filters, Dictionary<string, string> orderBy);

	Task<WebPrdVahed> GetAsync(short depId, Guid guid);

	Task<WebPrdVahed> GetAsync(short depId, string vahed2Name);
}
