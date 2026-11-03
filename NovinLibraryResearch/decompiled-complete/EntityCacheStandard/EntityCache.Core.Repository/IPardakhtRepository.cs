using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository;

public interface IPardakhtRepository : IEntityRepository<PardakhtBusiness, PardakhtReportBusiness>, IEditableDelivery, IDisposable
{
	Task<IEnumerable<PardakhtReportBusiness>> GetAllAsync(KhazaneFilter filter, CancellationToken token, string tranName);

	Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName);

	Task<int> NextId2Async(int? statusId, CancellationToken token, string tranName);

	Task<PardakhtBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName);

	Task<List<Guid>> GetNotTransferedAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(PardakhtBusiness item, EnSystem system, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName);

	void ResetCache();

	Task<List<ENSource>> GetAllExistingSourceAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName);
}
