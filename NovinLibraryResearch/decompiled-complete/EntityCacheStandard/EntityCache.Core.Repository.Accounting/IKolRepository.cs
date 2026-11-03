using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting;

public interface IKolRepository : ISyncRepository<KolBusiness>, IEditableDelivery, IRepository<KolBusiness>, INoGuidRepository<KolBusiness>, IDisposable
{
	Task<KolBusiness> GetAsync(int codeKol, CancellationToken token, string tranName);

	Task<int> CountChilderenNotZeroAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<KolBusiness> lstHesabsListKol, bool updateAccount, short tryCount, CancellationToken token, string tranName);

	Task<IEnumerable<KolBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, Guid? groupGuid, string tranName);

	Task<int> NextCodeInGroupAsync(Guid groupGuid, int startCode, int endCode, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(KolBusiness kol, bool updateAccount, short tryCount, CancellationToken token, string tranName);

	Task<IEnumerable<KolBusiness>> GetAllBetweenCodesAsync(long? code1, long? code2, CancellationToken token, string tranName);

	Task<byte[]> GetGardeshHesabAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebKol> items, CancellationToken token, string tranName);
}
