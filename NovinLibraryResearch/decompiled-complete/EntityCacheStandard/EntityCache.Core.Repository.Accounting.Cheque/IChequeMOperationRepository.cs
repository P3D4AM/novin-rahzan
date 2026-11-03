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

public interface IChequeMOperationRepository : ISyncRepository<ChequeMOperationBusiness>, IEditableDelivery, IRepository<ChequeMOperationBusiness>, INoGuidRepository<ChequeMOperationBusiness>, IDisposable
{
	Task<short> GetNextOrdinalAsync(Guid chequeGuid, CancellationToken token, string tranName);

	Task<IEnumerable<ChequeMOperationBusiness>> GetAllAsync(ChequeMOperationFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveByChequeGuidAsync(Guid chequeGuid, short tryCount, CancellationToken token, string tranName);

	Task<IEnumerable<ChequeMOperationReportBusiness>> GetAllReportAsync(Guid chequeGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> IncreaseOrinalAsync(Guid chequeGuid, short orinal, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateDataBaseOperationsAsync(List<Tuple<Guid, short>> tuples, CancellationToken token, string tranName);

	Task<List<MoghayeratChequeAvalDoreModel>> GetAllAvalDoreMoghyerat(Guid chequeMojodGuid, Guid chequeVagozarGuid, CancellationToken token, string tranName);

	Task<List<ChequeMInstantModel>> GetAllChequeMInstantAsync(DateTime dateM, CancellationToken token, string tranName);
}
