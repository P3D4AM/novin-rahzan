using System.Threading;
using System.Threading.Tasks;

namespace PacketParser.Entities.Interfaces.Accounting.DepotReceipt;

public interface IHasQuantity
{
	decimal Quantity { get; }

	Task SetQuantityAsync(EnSystem system, decimal quantity, CancellationToken token = default(CancellationToken), string tranName = "");
}
