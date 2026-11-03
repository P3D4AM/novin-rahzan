using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class PrdSerailDetailMapper : IItemAsyncMapper<PrdSerialDetailBusiness, WebPrdSerialDetail>, IItemAsyncMapper<PrdSerialDetailBusiness, PrdSerialDetailBusiness>, IItemAsyncMapper<WebPrdSerialDetail, PrdSerialDetailBusiness>
{
	public static PrdSerailDetailMapper Instance { get; private set; } = new PrdSerailDetailMapper();


	public Task<PrdSerialDetailBusiness> MapAsync(string contextGuid, WebPrdSerialDetail source, PrdSerialDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdSerialDetailBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.DetailGuid = source.DetailGuid;
			destination.AsType = source.AsType;
			destination.IsVorod = source.IsVorod;
			destination.Ordinal = source.Ordinal;
			destination.SanadDate = source.SanadDate;
			destination.ContextGuid = contextGuid;
			return Task.FromResult(destination);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdSerailDetailMapper.cs", 42);
			throw;
		}
	}

	public Task<PrdSerialDetailBusiness> MapAsync(string contextGuid, PrdSerialDetailBusiness source, PrdSerialDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdSerialDetailBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.DetailGuid = source.DetailGuid;
			destination.AsType = source.AsType;
			destination.IsVorod = source.IsVorod;
			destination.Ordinal = source.Ordinal;
			destination.ContextGuid = contextGuid;
			destination.SanadDate = source.SanadDate;
			return Task.FromResult(destination);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdSerailDetailMapper.cs", 71);
			throw;
		}
	}

	public Task<WebPrdSerialDetail> MapAsync(string contextGuid, PrdSerialDetailBusiness source, WebPrdSerialDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdSerialDetail();
			}
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.DetailGuid = source.DetailGuid;
			destination.AsType = source.AsType;
			destination.IsVorod = source.IsVorod;
			destination.Ordinal = source.Ordinal;
			destination.SanadDate = source.SanadDate;
			return Task.FromResult(destination);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdSerailDetailMapper.cs", 99);
			throw;
		}
	}

	public Task<List<PrdSerialDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdSerialDetail> source, List<PrdSerialDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdSerialDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdSerialDetailBusiness> source, List<PrdSerialDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebPrdSerialDetail>> MapListAsync(string contextGuid, IEnumerable<PrdSerialDetailBusiness> source, List<WebPrdSerialDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
