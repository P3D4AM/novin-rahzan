using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface IProductGroupRepository : ISyncRepository<PrdGroupBusiness>, IEditableDelivery, IRepository<PrdGroupBusiness>, INoGuidRepository<PrdGroupBusiness>, IDisposable
{
	Task<IEnumerable<PrdGroupBusiness>> GetListByGrpNameAsync(string name, CancellationToken token, string tranName);

	Task<PrdGroupBusiness> GetAsync(long fullCode, CancellationToken token, string tranName);

	Task<PrdGroupBusiness> GetAsync(string name, CancellationToken token, string tranName);

	Task<long> GetNextCodeAsync(Guid parent, CancellationToken token, string tranName);

	Task<IEnumerable<PrdGroupBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<bool>> HasImageAsync(string fileName, CancellationToken token, string tranName);

	void ResetCache(CancellationToken token, string tranName);

	Task<IEnumerable<PrdGroupBusiness>> GetChildrenAsync(Guid parentGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateSubGroupSpecialAsync(Guid masterGuid, bool special, string technicalInfo, CancellationToken token, string tranName);
}
