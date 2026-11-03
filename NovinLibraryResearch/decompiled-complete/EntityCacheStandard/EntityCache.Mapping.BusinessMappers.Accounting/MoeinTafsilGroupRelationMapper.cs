using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class MoeinTafsilGroupRelationMapper : IItemAsyncMapper<MoeinTafsilGroupRelationsBusiness, WebMoeinTafsilGroupRelation>, IItemAsyncMapper<MoeinTafsilGroupRelationsBusiness, MoeinTafsilGroupRelationsBusiness>, IItemAsyncMapper<WebMoeinTafsilGroupRelation, MoeinTafsilGroupRelationsBusiness>
{
	public static MoeinTafsilGroupRelationMapper Instance { get; private set; } = new MoeinTafsilGroupRelationMapper();


	public Task<MoeinTafsilGroupRelationsBusiness> MapAsync(string contextGuid, WebMoeinTafsilGroupRelation source, MoeinTafsilGroupRelationsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MoeinTafsilGroupRelationsBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.Level = source.Level;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\MoeinTafsilGroupRelationMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<MoeinTafsilGroupRelationsBusiness> MapAsync(string contextGuid, MoeinTafsilGroupRelationsBusiness source, MoeinTafsilGroupRelationsBusiness destination, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MoeinTafsilGroupRelationsBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.Level = source.Level;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\MoeinTafsilGroupRelationMapper.cs", 57);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<WebMoeinTafsilGroupRelation> MapAsync(string contextGuid, MoeinTafsilGroupRelationsBusiness source, WebMoeinTafsilGroupRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebMoeinTafsilGroupRelation();
			}
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.Level = source.Level;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\MoeinTafsilGroupRelationMapper.cs", 78);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<MoeinTafsilGroupRelationsBusiness>> MapListAsync(string contextGuid, IEnumerable<WebMoeinTafsilGroupRelation> source, List<MoeinTafsilGroupRelationsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<MoeinTafsilGroupRelationsBusiness>> MapListAsync(string contextGuid, IEnumerable<MoeinTafsilGroupRelationsBusiness> source, List<MoeinTafsilGroupRelationsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebMoeinTafsilGroupRelation>> MapListAsync(string contextGuid, IEnumerable<MoeinTafsilGroupRelationsBusiness> source, List<WebMoeinTafsilGroupRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
