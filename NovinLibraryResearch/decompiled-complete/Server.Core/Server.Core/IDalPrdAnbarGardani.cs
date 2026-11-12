using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalPrdAnbarGardani
{
	Task<IEnumerable<SaveResponse>> SaveAsync(short DepId, long hashId, IEnumerable<WebPrdAnbarGardani> lst, ENSource source, CancellationToken token);

	Task<WebPrdAnbarGardani> LoadDataAsync(short depId, Guid guid);

	Task<IEnumerable<WebPrdAnbarGardani>> GetAll(short DepId, DateTime Modified, CancellationToken cancellationToken);
}
