using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Entities.Accounting;

namespace Server.Core.TaxService;

public interface IDalTaxError
{
	Task<List<WebTaxErrors>> GetErrorsAsync();
}
