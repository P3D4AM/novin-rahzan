using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class AbadDetMapper : IItemAsyncMapper<AbadDetBusiness, WebAbad>, IItemAsyncMapper<AbadDetBusiness, AbadDetBusiness>, IItemAsyncMapper<WebAbad, AbadDetBusiness>
{
	public static AbadDetMapper Instance { get; private set; } = new AbadDetMapper();


	public Task<WebAbad> MapAsync(string contextGuid, AbadDetBusiness source, WebAbad destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebAbad();
			}
			destination.X = source.X;
			destination.Y = source.Y;
			destination.Z = source.Z;
			destination.Count = source.Count;
			destination.ShapeType = source.ShapeType;
			destination.ShapeInformation = source.ShapeInformation;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\AbadDetMapper.cs", 39);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<AbadDetBusiness> MapAsync(string contextGuid, AbadDetBusiness source, AbadDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AbadDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.X = source.X;
			destination.Y = source.Y;
			destination.Z = source.Z;
			destination.Count = source.Count;
			destination.ShapeType = source.ShapeType;
			destination.ShapeInformation = source.ShapeInformation;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.MasterGuid = source.MasterGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\AbadDetMapper.cs", 68);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<AbadDetBusiness> MapAsync(string contextGuid, WebAbad source, AbadDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AbadDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.X = source.X;
			destination.Y = source.Y;
			destination.Z = source.Z;
			destination.Count = source.Count;
			destination.ShapeType = source.ShapeType;
			destination.ShapeInformation = source.ShapeInformation;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\AbadDetMapper.cs", 96);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebAbad>> MapListAsync(string contextGuid, IEnumerable<AbadDetBusiness> source, List<WebAbad> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AbadDetBusiness>> MapListAsync(string contextGuid, IEnumerable<AbadDetBusiness> source, List<AbadDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AbadDetBusiness>> MapListAsync(string contextGuid, IEnumerable<WebAbad> source, List<AbadDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
