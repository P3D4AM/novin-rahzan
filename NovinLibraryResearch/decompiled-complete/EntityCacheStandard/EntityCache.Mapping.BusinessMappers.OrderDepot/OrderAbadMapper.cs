using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class OrderAbadMapper : IItemAsyncMapper<OrderAbadBusiness, WebOrderAbad>, IItemAsyncMapper<OrderAbadBusiness, OrderAbadBusiness>, IItemAsyncMapper<WebOrderAbad, OrderAbadBusiness>
{
	public static OrderAbadMapper Instance { get; private set; } = new OrderAbadMapper();


	public async Task<WebOrderAbad> MapAsync(string contextGuid, OrderAbadBusiness source, WebOrderAbad destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebOrderAbad();
			}
			destination.OrderType = source.OrderType;
			destination.DetailGuid = source.DetailGuid;
			destination.RefrencedGuid = source.RefrencedGuid;
			destination.ShapeInfo = source.ShapeInfo;
			WebOrderAbad webOrderAbad = destination;
			webOrderAbad.ListAbad = await AbadDetMapper.Instance.MapListAsync(contextGuid, source?.ListAbad, new List<WebAbad>(), tranName, token);
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderAbadMapper.cs", 39);
			throw;
		}
	}

	public Task<List<WebOrderAbad>> MapListAsync(string contextGuid, IEnumerable<OrderAbadBusiness> source, List<WebOrderAbad> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<OrderAbadBusiness> MapAsync(string contextGuid, OrderAbadBusiness source, OrderAbadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new OrderAbadBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.OrderType = source.OrderType;
			destination.DetailGuid = source.DetailGuid;
			destination.RefrencedGuid = source.RefrencedGuid;
			destination.ShapeInfo = source.ShapeInfo;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			OrderAbadBusiness orderAbadBusiness = destination;
			orderAbadBusiness.AddRangeToDetList(await AbadDetMapper.Instance.MapListAsync(contextGuid, source?.ListAbad, new List<AbadDetBusiness>(), tranName, token), clearDets: true);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderAbadMapper.cs", 67);
			throw;
		}
	}

	public Task<List<OrderAbadBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderAbadBusiness> source, List<OrderAbadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<OrderAbadBusiness> MapAsync(string contextGuid, WebOrderAbad source, OrderAbadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new OrderAbadBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.OrderType = source.OrderType;
			destination.DetailGuid = source.DetailGuid;
			destination.RefrencedGuid = source.RefrencedGuid;
			destination.ShapeInfo = source.ShapeInfo;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderAbadMapper.cs", 94);
			throw;
		}
	}

	public Task<List<OrderAbadBusiness>> MapListAsync(string contextGuid, IEnumerable<WebOrderAbad> source, List<OrderAbadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
