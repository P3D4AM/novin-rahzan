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

public interface IReceptionRepository : IEntityRepository<ReceptionBusiness, ReceptionReportBusiness>, IEditableDelivery, IDisposable
{
	Task<int> NextId2Async(int? statusId, CancellationToken token, string tranName);

	Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName);

	Task<ReceptionBusiness> GetFromDbAsync(Guid guid, OrderBusiness order, EnSystem system, CancellationToken token, string tranName);

	Task<ReceptionBusiness> GetAsync(Guid guid, OrderBusiness order, EnSystem system, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetReceptionTypeBySellAsync(List<Guid> lst1, List<Guid> lst2, CancellationToken token, string tranName);

	Task<List<ReceptionReportBusiness>> GetAllAsync(KhazaneFilter filters, CancellationToken token, string tranName);

	Task<List<Guid>> GetNotTransferedAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName);

	void ResetCache();

	Task<List<ENSource>> GetAllExistingSourceAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName);
}
