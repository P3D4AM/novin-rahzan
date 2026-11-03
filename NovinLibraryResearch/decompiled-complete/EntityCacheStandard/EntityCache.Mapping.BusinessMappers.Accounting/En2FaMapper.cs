using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class En2FaMapper : IItemAsyncMapper<En2FaBusiness, WebEn2Fa>, IItemAsyncMapper<En2FaBusiness, En2FaBusiness>, IItemAsyncMapper<WebEn2Fa, En2FaBusiness>
{
	public static En2FaMapper Instance { get; private set; } = new En2FaMapper();


	public Task<WebEn2Fa> MapAsync(string contextGuid, En2FaBusiness source, WebEn2Fa destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebEn2Fa();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.LName = source.LName;
			destination.FName = source.FName;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\En2FaMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<En2FaBusiness> MapAsync(string contextGuid, En2FaBusiness source, En2FaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new En2FaBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.LName = source.LName;
			destination.FName = source.FName;
			destination.Status = source.Status;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\En2FaMapper.cs", 61);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<En2FaBusiness> MapAsync(string contextGuid, WebEn2Fa source, En2FaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new En2FaBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.LName = source.LName;
			destination.FName = source.FName;
			destination.Status = source.Status;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\En2FaMapper.cs", 87);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebEn2Fa>> MapListAsync(string contextGuid, IEnumerable<En2FaBusiness> source, List<WebEn2Fa> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<En2FaBusiness>> MapListAsync(string contextGuid, IEnumerable<En2FaBusiness> source, List<En2FaBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<En2FaBusiness>> MapListAsync(string contextGuid, IEnumerable<WebEn2Fa> source, List<En2FaBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
