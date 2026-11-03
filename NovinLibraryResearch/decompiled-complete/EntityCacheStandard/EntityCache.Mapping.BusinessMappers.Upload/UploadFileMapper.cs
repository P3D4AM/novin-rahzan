using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Upload;

public class UploadFileMapper : IItemAsyncMapper<UploadFileBusiness, WebUploadFile>, IItemAsyncMapper<UploadFileBusiness, UploadFileBusiness>, IItemAsyncMapper<WebUploadFile, UploadFileBusiness>
{
	public static UploadFileMapper Instance { get; private set; } = new UploadFileMapper();


	public Task<WebUploadFile> MapAsync(string contextGuid, UploadFileBusiness source, WebUploadFile destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebUploadFile();
			}
			destination.Id = source.Id;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Crc = source.Crc;
			destination.FileManagementParts = source.FileManagementParts;
			destination.MinChunk = source.MinChunk;
			destination.MaxChunk = source.MaxChunk;
			destination.UrlPath = source.UrlPath;
			destination.PartCount = source.PartCount;
			destination.PartSent = source.PartSent;
			destination.SubDirectory = source.SubDirectory;
			destination.RetryCount = source.RetryCount;
			destination.InsertTime = source.InsertTime;
			destination.EntityGuid = source.EntityGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\UploadFileMapper.cs", 45);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebUploadFile>> MapListAsync(string contextGuid, IEnumerable<UploadFileBusiness> source, List<WebUploadFile> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<UploadFileBusiness> MapAsync(string contextGuid, UploadFileBusiness source, UploadFileBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new UploadFileBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Id = source.Id;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Crc = source.Crc;
			destination.FileManagementParts = source.FileManagementParts;
			destination.MinChunk = source.MinChunk;
			destination.MaxChunk = source.MaxChunk;
			destination.UrlPath = source.UrlPath;
			destination.PartCount = source.PartCount;
			destination.PartSent = source.PartSent;
			destination.SubDirectory = source.SubDirectory;
			destination.RetryCount = source.RetryCount;
			destination.InsertTime = source.InsertTime;
			destination.EntityGuid = source.EntityGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\UploadFileMapper.cs", 83);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<UploadFileBusiness>> MapListAsync(string contextGuid, IEnumerable<UploadFileBusiness> source, List<UploadFileBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<UploadFileBusiness> MapAsync(string contextGuid, WebUploadFile source, UploadFileBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new UploadFileBusiness(contextGuid);
			}
			destination.Id = source.Id;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Crc = source.Crc;
			destination.FileManagementParts = source.FileManagementParts;
			destination.MinChunk = source.MinChunk;
			destination.MaxChunk = source.MaxChunk;
			destination.UrlPath = source.UrlPath;
			destination.PartCount = source.PartCount;
			destination.PartSent = source.PartSent;
			destination.SubDirectory = source.SubDirectory;
			destination.RetryCount = source.RetryCount;
			destination.InsertTime = source.InsertTime;
			destination.EntityGuid = source.EntityGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Upload\\UploadFileMapper.cs", 120);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<UploadFileBusiness>> MapListAsync(string contextGuid, IEnumerable<WebUploadFile> source, List<UploadFileBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
