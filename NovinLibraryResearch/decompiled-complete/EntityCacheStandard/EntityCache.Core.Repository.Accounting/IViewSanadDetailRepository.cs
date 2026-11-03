using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.Records.Sanad;

namespace EntityCache.Core.Repository.Accounting;

public interface IViewSanadDetailRepository : ISyncRepository<ViewSanadDetailBusiness>, IEditableDelivery, IRepository<ViewSanadDetailBusiness>, INoGuidRepository<ViewSanadDetailBusiness>, IDisposable
{
	Task<IEnumerable<ViewSanadDetailBusiness>> GetAllSanadOutOfFinancialPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken token, string tranName);

	Task<IEnumerable<ViewSanadDetailBusiness>> GetAllAsync(int sanadNo, CancellationToken token, string tranName);

	Task<int> GetSanadNoAsync(Guid guid, CancellationToken token, string tranName);

	Task<int> GetFutureSanadCountAsync(DateTime lastDate, CancellationToken token, string tranName);

	Task<List<ViewSanadExistRecord>> GetAllExistRecordAsync(CancellationToken token, string tranName);
}
