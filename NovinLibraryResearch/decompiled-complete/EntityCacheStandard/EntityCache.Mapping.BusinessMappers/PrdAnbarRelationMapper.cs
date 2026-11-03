using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class PrdAnbarRelationMapper : IItemAsyncMapper<PrdAnbarRelationBusiness, WebPrdAnbarRelation>, IItemAsyncMapper<PrdAnbarRelationBusiness, PrdAnbarRelationBusiness>, IItemAsyncMapper<WebPrdAnbarRelation, PrdAnbarRelationBusiness>
{
	public static PrdAnbarRelationMapper Instance { get; private set; } = new PrdAnbarRelationMapper();


	public Task<WebPrdAnbarRelation> MapAsync(string contextGuid, PrdAnbarRelationBusiness source, WebPrdAnbarRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdAnbarRelation();
			}
			destination.Amount = source.Amount;
			destination.AnbarGuid = source.AnbarGuid;
			destination.Min = source.Min;
			destination.Max = source.Max;
			destination.PrdGuid = source.PrdGuid;
			destination.Modified = source.Modified;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\PrdAnbarRelationMapper.cs", 37);
			throw;
		}
	}

	public Task<List<WebPrdAnbarRelation>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarRelationBusiness> source, List<WebPrdAnbarRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PrdAnbarRelationBusiness> MapAsync(string contextGuid, PrdAnbarRelationBusiness source, PrdAnbarRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdAnbarRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Amount = source.Amount;
			destination.AnbarGuid = source.AnbarGuid;
			destination.Min = source.Min;
			destination.Max = source.Max;
			destination.PrdGuid = source.PrdGuid;
			destination.Modified = source.Modified;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\PrdAnbarRelationMapper.cs", 64);
			throw;
		}
	}

	public Task<List<PrdAnbarRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarRelationBusiness> source, List<PrdAnbarRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PrdAnbarRelationBusiness> MapAsync(string contextGuid, WebPrdAnbarRelation source, PrdAnbarRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdAnbarRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Amount = source.Amount;
			destination.AnbarGuid = source.AnbarGuid;
			destination.Min = source.Min;
			destination.Max = source.Max;
			destination.PrdGuid = source.PrdGuid;
			destination.Modified = source.Modified;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\PrdAnbarRelationMapper.cs", 91);
			throw;
		}
	}

	public Task<List<PrdAnbarRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdAnbarRelation> source, List<PrdAnbarRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
