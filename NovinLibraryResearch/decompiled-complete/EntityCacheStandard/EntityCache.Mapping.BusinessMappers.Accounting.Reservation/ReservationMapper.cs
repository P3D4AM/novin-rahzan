using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Reservation;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Reservation;

public class ReservationMapper : IItemAsyncMapper<ReservationBusiness, ReservationBusiness>
{
	public static ReservationMapper Instance { get; private set; } = new ReservationMapper();


	public Task<ReservationBusiness> MapAsync(string contextGuid, ReservationBusiness source, ReservationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new ReservationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.DateM = source.DateM;
			destination.TafsilGuid = source.TafsilGuid;
			destination.OrderGuid = source.OrderGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.Number = source.Number;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Reservation\\ReservationMapper.cs", 40);
			throw;
		}
	}

	public Task<List<ReservationBusiness>> MapListAsync(string contextGuid, IEnumerable<ReservationBusiness> source, List<ReservationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
