using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting;

public interface IMoeinRepository : ISyncRepository<MoeinBusiness>, IEditableDelivery, IRepository<MoeinBusiness>, INoGuidRepository<MoeinBusiness>, IDisposable
{
	Task<byte[]> GetGardeshHesabAsync(Guid guid, CancellationToken token, string tranName);

	Task<MoeinBusiness> GetAsync(long CodeMoein, CancellationToken token, string tranName);

	Task<IEnumerable<MoeinBusiness>> GetAllBetweenCodesAsync(long? code1, long? code2, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveMoeinRelationTafsilLevelAsync(Guid moeinGuid, List<TafsilGroupBusiness> lst, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<MoeinBusiness> lstHesabsListMoein, bool updateAccount, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateAccountAsync(Guid guid, decimal p0, short tryCount, CancellationToken token, string tranName);

	Task<long> NextCodeInGroupAsync(Guid KolGuid, long startRange, long endRange, CancellationToken token, string tranName);

	Task<IEnumerable<MoeinBusiness>> GetAllAsync(string search, Guid? kolGuid, Guid? tafsilLevelGuid, CancellationToken token, bool? status, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(MoeinBusiness moein, bool updateAccount, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebMoein> items, CancellationToken token, string tranName);
}
