using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Department;
using PacketParser.Entities.Snifts;

namespace Server.Core;

public interface IDalCustomerSnifts
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebLkInfo lk, WebCustomerSnifts item);
}
