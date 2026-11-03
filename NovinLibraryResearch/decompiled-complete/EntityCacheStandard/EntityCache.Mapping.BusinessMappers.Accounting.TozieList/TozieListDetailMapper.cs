using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.TozieList;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.TozieList;

public class TozieListDetailMapper : IItemAsyncMapper<TozieListDetailBusiness, WebTozieListDetail>, IItemAsyncMapper<TozieListDetailBusiness, TozieListDetailBusiness>, IItemAsyncMapper<WebTozieListDetail, TozieListDetailBusiness>
{
	public static TozieListDetailMapper Instance { get; private set; } = new TozieListDetailMapper();


	public Task<WebTozieListDetail> MapAsync(string contextGuid, TozieListDetailBusiness source, WebTozieListDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTozieListDetail();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TozieList\\TozieListDetailMapper.cs", 35);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<TozieListDetailBusiness> MapAsync(string contextGuid, TozieListDetailBusiness source, TozieListDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TozieListDetailBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Status = source.Status;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TozieList\\TozieListDetailMapper.cs", 59);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<TozieListDetailBusiness> MapAsync(string contextGuid, WebTozieListDetail source, TozieListDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TozieListDetailBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Status = source.Status;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\TozieList\\TozieListDetailMapper.cs", 83);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebTozieListDetail>> MapListAsync(string contextGuid, IEnumerable<TozieListDetailBusiness> source, List<WebTozieListDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TozieListDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TozieListDetailBusiness> source, List<TozieListDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TozieListDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTozieListDetail> source, List<TozieListDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
