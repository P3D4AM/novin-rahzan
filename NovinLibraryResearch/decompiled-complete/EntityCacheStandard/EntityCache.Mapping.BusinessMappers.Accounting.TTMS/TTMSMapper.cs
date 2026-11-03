using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.TTMS;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.TTMS;

public class TTMSMapper : IItemAsyncMapper<TTMSBusiness, TTMSBusiness>
{
	public static TTMSMapper Instance { get; private set; } = new TTMSMapper();


	public Task<TTMSBusiness> MapAsync(string contextGuid, TTMSBusiness source, TTMSBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TTMSBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.DateM = source.DateM;
			destination.FilePath = source.FilePath;
			destination.Priod = source.Priod;
			destination.Year = source.Year;
			destination.State = source.State;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TTMS\\TTMSMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<TTMSBusiness>> MapListAsync(string contextGuid, IEnumerable<TTMSBusiness> source, List<TTMSBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
