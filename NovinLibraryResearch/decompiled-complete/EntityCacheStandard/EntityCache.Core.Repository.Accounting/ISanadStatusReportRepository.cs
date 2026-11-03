using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Report;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Sanad;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting;

public interface ISanadStatusReportRepository : ISyncRepository<SanadStatusReportBusiness>, IEditableDelivery, IRepository<SanadStatusReportBusiness>, INoGuidRepository<SanadStatusReportBusiness>, IDisposable
{
	Task<SanadStatusReportBusiness> GetAsync(int id, CancellationToken token, string tranName);

	Task<SanadStatusReportBusiness> GetAsync(string name, Document asType, CancellationToken token, string tranName);

	Task<IEnumerable<SanadStatusReportBusiness>> GetAllAsync(Document? asType, bool? status, CancellationToken token, string tranName);

	Task<int> GetNextIdAsync(CancellationToken token, string tranName);

	Task<IEnumerable<SanadStatusReportErrorTimeModel>> GetAllErrorTimeAsync(CancellationToken token, string tranName);

	Task<IEnumerable<SanadStatusReportForceUpdateModel>> GetAllForceUpdateAsync(DateTime dateMoasser, CancellationToken token, string tranName);

	Task<int> GetMinErrorTimeAsync(CancellationToken token, string tranName);

	Task<List<SanadStatusReportBusiness>> IsDuplicateCodeAsync(long minCode, long maxCode, Document asType, Guid guid, CancellationToken token, string tranName);
}
