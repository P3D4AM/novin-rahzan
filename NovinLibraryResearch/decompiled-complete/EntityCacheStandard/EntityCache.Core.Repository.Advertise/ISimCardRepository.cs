using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface ISimCardRepository : IRepository<SimCardBusiness>, INoGuidRepository<SimCardBusiness>, IDisposable
{
	Task<long> GetNextNumberAsync(short advType, short maxDailyCountDivar, short maxDailyCountSheypoor, string IP, short maxMountlyCountDivar, short maxMountlyCountSheypoor, int maxAdvCountInIpInDayDivar, int maxAdvCountInIpInDaySheypoor, string tranName);

	Task<List<long>> GetSimCardsForSendAdvAsync(string tranName);

	Task<string> GetOwnerNameAsync(long simCardNumber, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(List<SimCardBusiness> lst, short tryCount, string tranName);

	Task<SimCardBusiness> GetAsync(long number, string tranName);

	Task<bool> CheckNumberAsync(long number, Guid guid, string tranName);

	Task<bool> CheckUserNameAsync(string username, Guid guid, string tranName);

	Task<List<SimCardBusiness>> GetAllSpecialAsync(Expression<Func<ISimCard, bool>> where, string tranName);

	Task<IEnumerable<SimCardBusiness>> GetAllAsync(Guid? ownerGuid, string tranName);

	Task<int> DeleteCountAsync(string tranName);

	Task<ReturnedSaveFuncInfo> UpdateAllSheypoorBlocks(bool state, short tryCount, string tranName);
}
