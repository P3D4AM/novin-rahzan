using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Reservation;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Reservation;

public class ReservationDetailMapper : IItemAsyncMapper<ReservationDetailBusiness, ReservationDetailBusiness>
{
	public static ReservationDetailMapper Instance { get; private set; } = new ReservationDetailMapper();


	public async Task<ReservationDetailBusiness> MapAsync(string contextGuid, ReservationDetailBusiness source, ReservationDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new NotImplementedException("Please Fill Destination !!");
		}
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
			destination.OrderGuid = source.OrderGuid;
			await destination.SetPlacementGuidAsync(source.PlacementGuid, EnSystem.Mapper, token, tranName);
			destination.StartDate = source.StartDate;
			destination.EndDate = source.EndDate;
			destination.PeopleCount = source.PeopleCount;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Reservation\\ReservationDetailMapper.cs", 38);
			throw;
		}
	}

	public Task<List<ReservationDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<ReservationDetailBusiness> source, List<ReservationDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
