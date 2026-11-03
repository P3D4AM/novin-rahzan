using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Upload;

public class UploadFilePartMapper : IItemAsyncMapper<UploadFilePartBusiness, WebUploadFilePart>, IItemAsyncMapper<UploadFilePartBusiness, UploadFilePartBusiness>, IItemAsyncMapper<WebUploadFilePart, UploadFilePartBusiness>
{
	public static UploadFilePartMapper Instance { get; private set; } = new UploadFilePartMapper();


	public Task<WebUploadFilePart> MapAsync(string contextGuid, UploadFilePartBusiness source, WebUploadFilePart destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebUploadFilePart();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Order = source.Order;
			destination.FileId = source.FileId;
			destination.RetryCount = source.RetryCount;
			destination.IsSent = source.IsSent;
			destination.NextTry = source.NextTry;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\UploadFilePartMapper.cs", 37);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebUploadFilePart>> MapListAsync(string contextGuid, IEnumerable<UploadFilePartBusiness> source, List<WebUploadFilePart> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<UploadFilePartBusiness> MapAsync(string contextGuid, UploadFilePartBusiness source, UploadFilePartBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new UploadFilePartBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Order = source.Order;
			destination.FileId = source.FileId;
			destination.RetryCount = source.RetryCount;
			destination.IsSent = source.IsSent;
			destination.NextTry = source.NextTry;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\UploadFilePartMapper.cs", 67);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<UploadFilePartBusiness>> MapListAsync(string contextGuid, IEnumerable<UploadFilePartBusiness> source, List<UploadFilePartBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public Task<UploadFilePartBusiness> MapAsync(string contextGuid, WebUploadFilePart source, UploadFilePartBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new UploadFilePartBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Order = source.Order;
			destination.FileId = source.FileId;
			destination.RetryCount = source.RetryCount;
			destination.IsSent = source.IsSent;
			destination.NextTry = source.NextTry;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\UploadFilePartMapper.cs", 99);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<UploadFilePartBusiness>> MapListAsync(string contextGuid, IEnumerable<WebUploadFilePart> source, List<UploadFilePartBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
