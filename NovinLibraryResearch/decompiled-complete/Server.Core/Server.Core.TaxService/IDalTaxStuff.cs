using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Accounting.TaxService;

namespace Server.Core.TaxService;

public interface IDalTaxStuff
{
	Task<List<WebTaxStuff>> GetAllByName(string name, CancellationToken cancellationToken);
}
