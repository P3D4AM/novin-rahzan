using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core.TaxService;

public interface IDalTaxOrderLog
{
	Task<List<SaveResponse>> SaveAsync(short depId, ENSource source, List<WebTaxOrderLog> lst, CancellationToken token);
}
