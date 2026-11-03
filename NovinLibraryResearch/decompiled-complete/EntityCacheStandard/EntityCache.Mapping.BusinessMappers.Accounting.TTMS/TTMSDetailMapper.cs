using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.TTMS;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.TTMS;

public class TTMSDetailMapper : IItemAsyncMapper<TTMSDetBusiness, TTMSDetBusiness>
{
	public static TTMSDetailMapper Instance { get; private set; } = new TTMSDetailMapper();


	public Task<TTMSDetBusiness> MapAsync(string contextGuid, TTMSDetBusiness source, TTMSDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TTMSDetBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Description = source.Description;
			destination.MasterGuid = source.MasterGuid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TTMS\\TTMSDetailMapper.cs", 36);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<TTMSDetBusiness>> MapListAsync(string contextGuid, IEnumerable<TTMSDetBusiness> source, List<TTMSDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
