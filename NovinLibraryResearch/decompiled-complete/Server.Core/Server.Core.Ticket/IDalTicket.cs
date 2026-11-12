using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Server.Core.Connection;

namespace Server.Core.Ticket;

public interface IDalTicket
{
	Task<IEnumerable<WebTicket>> GetAllAsync(short depId, DateTime date1, DateTime date2, CancellationToken cancellationToken);

	Task<IEnumerable<WebTicket>> GetAllAsync(EnTargetType ticketTarget, IClsConnectionData connection, short partition);

	Task<WebTicket> LoadDataAsync(Guid guid);

	Task<IEnumerable<SaveResponse>> Save(short depId, long hashId, IEnumerable<WebTicket> lst, ENSource source, CancellationToken token);

	Task<IEnumerable<WebTicket>> GetallFromClients_ListForServerAsync(IClsConnectionData connection);

	Task<List<short>> GetPartitionsAsync(short depId, CancellationToken cancellationToken);
}
