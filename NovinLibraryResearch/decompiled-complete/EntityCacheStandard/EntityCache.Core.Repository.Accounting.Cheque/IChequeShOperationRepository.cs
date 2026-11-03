using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting.Cheque;

public interface IChequeShOperationRepository : ISyncRepository<ChequeShOperationBusiness>, IEditableDelivery, IRepository<ChequeShOperationBusiness>, INoGuidRepository<ChequeShOperationBusiness>, IDisposable
{
	Task<IEnumerable<ChequeShOperationBusiness>> GetAllAsync(ChequeShOperationFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveByChequeGuidAsync(Guid chequeGuid, short tryCount, CancellationToken token, string tranName);

	Task<List<ChequeShInstantModel>> GetAllChequeShInstantAsync(DateTime dateM, CancellationToken token, string tranName);

	Task<IEnumerable<ChequeShOperationReportBusiness>> GetAllReportAsync(Guid chequeGuid, CancellationToken token, string tranName);
}
