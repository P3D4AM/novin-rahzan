using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Server.Core.Connection;

namespace Server.Core.Ticket;

public interface IDalTicketDet
{
	Task<List<WebTicketDet>> GetAllAsync(Guid ticketGuid);

	Task<List<WebTicketDet>> GetAllAsync(EnTargetType ticketTarget, IClsConnectionData connection, short partition);

	Task<WebTicketDet> LoadDataAsync(Guid guid);

	Task<List<SaveResponse>> Save(short depId, long hashId, List<WebTicketDet> lst, ENSource source, CancellationToken token);

	Task<List<WebTicketDet>> GetallFromClients_ListForServerAsync(IClsConnectionData connection);

	Task<List<short>> GetPartitionsAsync(short depId, CancellationToken cancellationToken);
}
