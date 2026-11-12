using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalCusTitle
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashId, ENSource source, List<WebCusTitle> lst, CancellationToken token);

	Task<List<WebCusTitle>> GetAll(short DepId, DateTime Modified, CancellationToken cancellationToken);

	Task<WebCusTitle> GetAsync(short depId, Guid guid);

	Task<WebCusTitle> GetAsync(short DepId, short Id);
}
