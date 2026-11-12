using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Server.Core.Connection;

namespace Server.Core.Ticket;

public interface IDalTicketPart
{
	Task<IEnumerable<WebTicketPart>> GetAllAsync(IClsConnectionData connection, short partition);

	Task<WebTicketPart> LoadDataAsync(Guid guid);

	Task<List<SaveResponse>> SaveAsync(short depId, long hashId, List<WebTicketPart> lst, ENSource source, CancellationToken token);

	Task<List<WebTicketPart>> GetallFromClients_ListForServerAsync(IClsConnectionData connection);

	Task<List<short>> GetPartitionsAsync(short depId, CancellationToken cancellationToken);
}
