using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Scale;

public class Scale_InfoMapper : IItemAsyncMapper<Scale_InfoBusiness, Scale_InfoBusiness>
{
	public static Scale_InfoMapper Instance { get; private set; } = new Scale_InfoMapper();


	public Task<Scale_InfoBusiness> MapAsync(string contextGuid, Scale_InfoBusiness source, Scale_InfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Scale_InfoBusiness(contextGuid);
			}
			destination.ScaleGuid = source.ScaleGuid;
			destination.PortNumber = source.PortNumber;
			destination.TimerInterval = source.TimerInterval;
			destination.Name = source.Name;
			destination.ScaleGuid = source.ScaleGuid;
			destination.Deleted = source.Deleted;
			destination.IP = source.IP;
			destination.EndMessage = source.EndMessage;
			destination.ShopName = source.ShopName;
			destination.AccountingMemory = source.AccountingMemory;
			destination.AccountingDivision = source.AccountingDivision;
			destination.AccountingIP = source.AccountingIP;
			destination.MNGLock = source.MNGLock;
			destination.FactorNumber = source.FactorNumber;
			destination.ConnectionType = source.ConnectionType;
			destination.Model = source.IP;
			destination.Serial = source.Serial;
			destination.Description = source.Description;
			destination.AccountingDate = source.AccountingDate;
			destination.Guid = source.Guid;
			destination.AccountingID = source.AccountingID;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Scale\\Scale_InfoMapper.cs", 51);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Scale_InfoBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_InfoBusiness> source, List<Scale_InfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
