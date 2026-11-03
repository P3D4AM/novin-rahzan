using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class SimcardMapper : IItemAsyncMapper<SimCardBusiness, SimCardBusiness>
{
	public static SimcardMapper Instance { get; private set; } = new SimcardMapper();


	public Task<SimCardBusiness> MapAsync(string contextGuid, SimCardBusiness source, SimCardBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SimCardBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DivarModified = source.DivarModified;
			destination.SheypoorModified = source.SheypoorModified;
			destination.NextUseDivar = source.NextUseDivar;
			destination.NextUseSheypoor = source.NextUseSheypoor;
			destination.isSheypoorBlocked = source.isSheypoorBlocked;
			destination.isSendDivarAdv = source.isSendDivarAdv;
			destination.isSendSheypoorAdv = source.isSendSheypoorAdv;
			destination.Number = source.Number;
			destination.OwnerGuid = source.OwnerGuid;
			destination.UseCount = source.UseCount;
			destination.Status = source.Status;
			destination.Operator = source.Operator;
			destination.UserName = source.UserName;
			destination.IsChecked = source.IsChecked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\SimcardMapper.cs", 45);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<SimCardBusiness>> MapListAsync(string contextGuid, IEnumerable<SimCardBusiness> source, List<SimCardBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
