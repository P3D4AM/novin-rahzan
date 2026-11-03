using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Aghsat;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Filters.Configs;

namespace EntityCache.Core.Repository.Accounting.Aghsat;

public interface IAghsatMRepository : ISyncRepository<AghsatMBusiness>, IEditableDelivery, IRepository<AghsatMBusiness>, INoGuidRepository<AghsatMBusiness>, IDisposable
{
	Task<int> NextId2Async(CancellationToken token, string tranName);

	Task<IEnumerable<AghsatMBusiness>> GetAllByGroupGuidAsync(Guid groupGuid, CancellationToken token, string tranName);

	Task<List<AghsatMBusiness>> GetAllNotPayedAsync(Guid? hGuid, HesabType? hesabType, CancellationToken token, string tranName);

	Task<IEnumerable<AghsatMBusiness>> GetAllAsync(CancellationToken token, AghsatMFilter filter, string tranName);

	Task<AghsatMBusiness> GetAsync(int id2, CancellationToken token, string tranName);

	Task<int> GetCountByHGuidAsync(Guid tafsilGuid, CancellationToken token, string tranName);

	Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName);

	Task<IEnumerable<AghsatMSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName);

	Task<IEnumerable<AghsatMDelayModel>> GetAllDelayedInstallmentsAsync(AghsatMDelayedConfig config, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateReceptionInfoAsync(Guid? receptionGuid, CancellationToken token, string tranName);
}
