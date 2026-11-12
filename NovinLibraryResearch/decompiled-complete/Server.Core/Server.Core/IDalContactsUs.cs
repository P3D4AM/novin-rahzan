using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalContactsUs
{
	Task<List<SaveResponse>> SaveAsync(short depId, List<WebContactUs> lst, CancellationToken token, ENSource source);

	Task<WebContactUs> LoadDataAsync(short depId, int Id);

	Task<List<WebContactUs>> GetAllAsync(short depId, DateTime Modified, CancellationToken cancellationToken);
}
