using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalChequeShOperation
{
	Task<List<SaveResponse>> SaveAsync(short DepId, long hashid, ENSource source, List<WebChequeShOperation> lst, CancellationToken token);

	Task<WebChequeShOperation> GetAsync(Guid guid, short depId);
}
