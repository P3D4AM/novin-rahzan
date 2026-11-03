using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid.Summary;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Tolid;

public class TolidSummaryDetailMapper : IItemAsyncMapper<TolidSummaryDetailBusiness, WebTolidSummaryDetail>, IItemAsyncMapper<TolidSummaryDetailBusiness, TolidSummaryDetailBusiness>, IItemAsyncMapper<WebTolidSummaryDetail, TolidSummaryDetailBusiness>
{
	public static TolidSummaryDetailMapper Instance { get; private set; } = new TolidSummaryDetailMapper();


	public Task<WebTolidSummaryDetail> MapAsync(string contextGuid, TolidSummaryDetailBusiness source, WebTolidSummaryDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTolidSummaryDetail();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ProductGuid = source.ProductGuid;
			destination.DepotGuid = source.DepotGuid;
			destination.TolidGuid = source.TolidGuid;
			destination.RealAmount = source.RealAmount;
			destination.AvailableAmount = source.AvailableAmount;
			destination.ProducedAmount = source.ProducedAmount;
			destination.MasterGuid = source.MasterGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidSummary\\TolidSummaryDetailMapper.cs", 40);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<TolidSummaryDetailBusiness> MapAsync(string contextGuid, TolidSummaryDetailBusiness source, TolidSummaryDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TolidSummaryDetailBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ProductGuid = source.ProductGuid;
			destination.DepotGuid = source.DepotGuid;
			destination.TolidGuid = source.TolidGuid;
			destination.RealAmount = source.RealAmount;
			destination.AvailableAmount = source.AvailableAmount;
			destination.ProducedAmount = source.ProducedAmount;
			destination.MasterGuid = source.MasterGuid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidSummary\\TolidSummaryDetailMapper.cs", 69);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<TolidSummaryDetailBusiness> MapAsync(string contextGuid, WebTolidSummaryDetail source, TolidSummaryDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TolidSummaryDetailBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ProductGuid = source.ProductGuid;
			destination.DepotGuid = source.DepotGuid;
			destination.TolidGuid = source.TolidGuid;
			destination.RealAmount = source.RealAmount;
			destination.AvailableAmount = source.AvailableAmount;
			destination.ProducedAmount = source.ProducedAmount;
			destination.MasterGuid = source.MasterGuid;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidSummary\\TolidSummaryDetailMapper.cs", 98);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebTolidSummaryDetail>> MapListAsync(string contextGuid, IEnumerable<TolidSummaryDetailBusiness> source, List<WebTolidSummaryDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TolidSummaryDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidSummaryDetailBusiness> source, List<TolidSummaryDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TolidSummaryDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTolidSummaryDetail> source, List<TolidSummaryDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
