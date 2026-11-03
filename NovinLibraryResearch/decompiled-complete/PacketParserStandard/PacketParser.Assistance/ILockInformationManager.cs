using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;

namespace PacketParser.Assistance;

public abstract class ILockInformationManager
{
	public abstract Task<ReturnedSaveFuncInfoWithValue<LKInfo>> CheckLkWithWebServiceAsync(Package package, bool ignoreHttpExceptions, LKInfo lk = null);
}
