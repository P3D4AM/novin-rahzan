using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Reservation;
using PacketParser.Filters.Hotel;

namespace EntityCache.Core.Repository.Accounting.Reservation;

public interface IReservationRepository : IRepository<ReservationBusiness>, INoGuidRepository<ReservationBusiness>, IDisposable
{
	Task<IEnumerable<ReservationReportBusiness>> GetAllModelAsync(ReservationFilter filter, CancellationToken token, string tranName);

	Task<int> GetNextNumberAsync(CancellationToken token, string tranName);

	Task<ReservationBusiness> GetAsync(int number, CancellationToken token, string tranName);
}
