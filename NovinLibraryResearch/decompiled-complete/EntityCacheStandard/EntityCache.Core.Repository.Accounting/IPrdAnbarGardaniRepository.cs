using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Records.AnbarGardani;

namespace EntityCache.Core.Repository.Accounting;

public interface IPrdAnbarGardaniRepository : IEntityRepository<PrdAnbarGardaniBusiness, PrdAnbarGardaniReportBusiness>, IEditableDelivery, IDisposable
{
	Task<PrdAnbarGardaniBusiness> GetByNumberAsync(int number, CancellationToken token, string tranName);

	Task<PrdAnbarGardaniBusiness> GetAsync(int sanadNumber, CancellationToken token, string tranName);

	Task<int> NextNumberAsync(int? statusId, CancellationToken token, string tranName);

	Task<List<PrdAnbarGardaniDetBusiness>> GetAllAsync(PrdAnbarGardaniBusiness master, AnbarGardaniFilter filter, CancellationToken token, string tranName);

	Task<PrdAnbarGardaniBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName);

	Task<List<PrdAnbarGardaniReportBusiness>> GetAllAsync(AnbarGardaniFilter filter, CancellationToken token, string tranName);

	Task<Guid> GetDepotGuidAsync(Guid detailGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateTypeAsync(Guid guid, SanadType type, CancellationToken token, short tryCount, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateDateAsync(Guid guid, DateTime date, CancellationToken token, string tranName);

	Task<List<AnbarGardaniEkhtetamRecord>> GetAllEkhtetamRecordsAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName);
}
