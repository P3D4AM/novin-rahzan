using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Advertise;

public class VisitorMapper : IItemAsyncMapper<VisitorBusiness, VisitorBusiness>
{
	public static VisitorMapper Instance { get; private set; } = new VisitorMapper();


	public Task<VisitorBusiness> MapAsync(string contextGuid, VisitorBusiness source, VisitorBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new VisitorBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.MasterCount = source.MasterCount;
			destination.SlaveCount = source.SlaveCount;
			destination.IsAgent = source.IsAgent;
			destination.AdvReplacement = source.AdvReplacement;
			destination.FullName = source.FullName;
			destination.Status = source.Status;
			destination.IsChecked = source.IsChecked;
			destination.Enable_Status = source.Enable_Status;
			destination.Phone = source.Phone;
			destination.DepartmentGuid = source.DepartmentGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Advertise\\VisitorMapper.cs", 41);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<VisitorBusiness>> MapListAsync(string contextGuid, IEnumerable<VisitorBusiness> source, List<VisitorBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
