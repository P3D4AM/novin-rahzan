using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class ScriptMapper : IItemAsyncMapper<ScriptBusiness, WebScript>, IItemAsyncMapper<ScriptBusiness, ScriptBusiness>, IItemAsyncMapper<WebScript, ScriptBusiness>
{
	public static ScriptMapper Instance { get; private set; } = new ScriptMapper();


	public Task<WebScript> MapAsync(string contextGuid, ScriptBusiness source, WebScript destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebScript();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.ExecDesc = source.ExecDesc;
			destination.ExecSt = source.ExecSt;
			destination.ExecTime = source.ExecTime;
			destination.NextTime = source.NextTime;
			destination.Script = source.Script;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ScriptMapper.cs", 39);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebScript>> MapListAsync(string contextGuid, IEnumerable<ScriptBusiness> source, List<WebScript> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ScriptBusiness> MapAsync(string contextGuid, ScriptBusiness source, ScriptBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ScriptBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.ExecDesc = source.ExecDesc;
			destination.ExecSt = source.ExecSt;
			destination.ExecTime = source.ExecTime;
			destination.NextTime = source.NextTime;
			destination.Script = source.Script;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ScriptMapper.cs", 72);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ScriptBusiness>> MapListAsync(string contextGuid, IEnumerable<ScriptBusiness> source, List<ScriptBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ScriptBusiness> MapAsync(string contextGuid, WebScript source, ScriptBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ScriptBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.ExecDesc = source.ExecDesc;
			destination.ExecSt = source.ExecSt;
			destination.ExecTime = source.ExecTime;
			destination.NextTime = source.NextTime;
			destination.Script = source.Script;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\ScriptMapper.cs", 104);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ScriptBusiness>> MapListAsync(string contextGuid, IEnumerable<WebScript> source, List<ScriptBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
