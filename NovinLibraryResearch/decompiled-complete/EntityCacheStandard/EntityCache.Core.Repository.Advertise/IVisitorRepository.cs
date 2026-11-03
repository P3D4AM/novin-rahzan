using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Advertise;

public interface IVisitorRepository : IRepository<VisitorBusiness>, INoGuidRepository<VisitorBusiness>, IDisposable
{
	Task<MasterSlaveAdvReplacementsModel> GetMasterSlaveAdvReplacementsAsync(long simcardNumber, string tranName);

	Task<List<VisitorBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeEnableStatusAsync(Guid guid, string status, short tryCount, string tranName);

	Task<int> DeleteCountAsync(short tryCount, string tranName);

	Task<List<VisitorBusiness>> GetAllAsync(Guid depGuid, string tranName);
}
