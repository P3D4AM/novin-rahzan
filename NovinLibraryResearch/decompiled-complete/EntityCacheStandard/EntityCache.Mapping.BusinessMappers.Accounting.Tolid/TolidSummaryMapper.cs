using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid.Summary;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Tolid;

public class TolidSummaryMapper : IItemAsyncMapper<TolidSummaryBusiness, WebTolidSummary>, IItemAsyncMapper<TolidSummaryBusiness, TolidSummaryBusiness>, IItemAsyncMapper<WebTolidSummary, TolidSummaryBusiness>
{
	public static TolidSummaryMapper Instance { get; private set; } = new TolidSummaryMapper();


	public Task<WebTolidSummary> MapAsync(string contextGuid, TolidSummaryBusiness source, WebTolidSummary destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTolidSummary();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.Id2 = source.Id2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidSummary\\TolidSummaryMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<TolidSummaryBusiness> MapAsync(string contextGuid, TolidSummaryBusiness source, TolidSummaryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TolidSummaryBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.Id2 = source.Id2;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidSummary\\TolidSummaryMapper.cs", 63);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<TolidSummaryBusiness> MapAsync(string contextGuid, WebTolidSummary source, TolidSummaryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TolidSummaryBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.Id2 = source.Id2;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidSummary\\TolidSummaryMapper.cs", 89);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebTolidSummary>> MapListAsync(string contextGuid, IEnumerable<TolidSummaryBusiness> source, List<WebTolidSummary> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TolidSummaryBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidSummaryBusiness> source, List<TolidSummaryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TolidSummaryBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTolidSummary> source, List<TolidSummaryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
