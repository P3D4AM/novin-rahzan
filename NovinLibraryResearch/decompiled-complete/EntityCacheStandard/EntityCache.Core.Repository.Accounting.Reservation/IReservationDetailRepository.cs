using System;
using EntityCache.Business.Accounting.Reservation;

namespace EntityCache.Core.Repository.Accounting.Reservation;

public interface IReservationDetailRepository : IPersistenceMasterRepository<ReservationDetailBusiness>, IDisposable
{
}
