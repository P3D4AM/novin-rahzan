using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class FileInfoMapper : IItemAsyncMapper<FileInfoBusiness, WebFileInfo>, IItemAsyncMapper<FileInfoBusiness, FileInfoBusiness>, IItemAsyncMapper<FileInfoReTryDownloadBusiness, FileInfoBusiness>, IItemAsyncMapper<FileInfoReTryDownloadBusiness, FileInfoReTryDownloadBusiness>, IItemAsyncMapper<FileInfoBusiness, FileInfoReTryDownloadBusiness>, IItemAsyncMapper<WebFileInfo, FileInfoBusiness>
{
	public static FileInfoMapper Instance { get; private set; } = new FileInfoMapper();


	public async Task<WebFileInfo> MapAsync(string contextGuid, FileInfoBusiness source, WebFileInfo destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebFileInfo();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Part = source.Part;
			destination.SubDirectory = source.SubDirectory;
			destination.Status = source.Status;
			destination.EntityGuid = source.EntityGuid;
			destination.Url = "";
			destination.Crc = source.Crc;
			destination.FileLen = source.FileLen;
			destination.FileName = source.FileName;
			destination.FullPath = "";
			destination.FileContent = source.FileContent;
			destination.CompressMode = source.CompressMode;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\FileInfoMapper.cs", 54);
			throw;
		}
		return destination;
	}

	public Task<List<WebFileInfo>> MapListAsync(string contextGuid, IEnumerable<FileInfoBusiness> source, List<WebFileInfo> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<FileInfoBusiness> MapAsync(string contextGuid, FileInfoBusiness source, FileInfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FileInfoBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Part = source.Part;
			destination.SubDirectory = source.SubDirectory;
			destination.Status = source.Status;
			destination.EntityGuid = source.EntityGuid;
			destination.Url = source.Url;
			destination.Crc = source.Crc;
			destination.FileLen = source.FileLen;
			destination.FileName = source.FileName;
			destination.FileContent = source.FileContent;
			destination.CompressMode = source.CompressMode;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\FileInfoMapper.cs", 96);
			throw;
		}
		return destination;
	}

	public Task<FileInfoBusiness> MapAsync(string contextGuid, WebFileInfo source, FileInfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FileInfoBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Part = source.Part;
			destination.SubDirectory = source.SubDirectory;
			destination.Status = source.Status;
			destination.EntityGuid = source.EntityGuid;
			destination.Url = source.Url;
			destination.Crc = source.Crc;
			destination.FileLen = source.FileLen;
			destination.FileName = source.FileName;
			destination.SetFullPath(source.FullPath);
			destination.FileContent = source.FileContent;
			destination.CompressMode = source.CompressMode;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\FileInfoMapper.cs", 137);
			throw;
		}
		return Task.FromResult(destination);
	}

	public async Task<FileInfoBusiness> MapAsync(string contextGuid, FileInfoReTryDownloadBusiness source, FileInfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new FileInfoBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Part = source.Part;
			destination.SubDirectory = source.SubDirectory;
			destination.Status = source.Status;
			destination.EntityGuid = source.EntityGuid;
			destination.Url = source.Url;
			FileInfoBusiness fileInfoBusiness = destination;
			fileInfoBusiness.Crc = await source.GetCrcAsync();
			fileInfoBusiness = destination;
			fileInfoBusiness.FileLen = await source.GetFileLenAsync();
			destination.FileName = source.FileName;
			destination.SetFullPath(source.FullPath);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\FileInfoMapper.cs", 168);
			throw;
		}
		return destination;
	}

	public async Task<FileInfoReTryDownloadBusiness> MapAsync(string contextGuid, FileInfoBusiness source, FileInfoReTryDownloadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FileInfoReTryDownloadBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Part = source.Part;
			destination.SubDirectory = source.SubDirectory;
			destination.Status = source.Status;
			destination.EntityGuid = source.EntityGuid;
			destination.Url = source.Url;
			destination.SetCrc(source.Crc);
			destination.SetFileLen(source.FileLen);
			destination.FileName = source.FileName;
			destination.FullPath = source.GetLocalFilePath();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\FileInfoMapper.cs", 199);
			throw;
		}
		return destination;
	}

	public async Task<FileInfoReTryDownloadBusiness> MapAsync(string contextGuid, FileInfoReTryDownloadBusiness source, FileInfoReTryDownloadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new FileInfoReTryDownloadBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			FileInfoReTryDownloadBusiness fileInfoReTryDownloadBusiness = destination;
			fileInfoReTryDownloadBusiness.SetCrc(await source.GetCrcAsync());
			fileInfoReTryDownloadBusiness = destination;
			fileInfoReTryDownloadBusiness.SetFileLen(await source.GetFileLenAsync());
			destination.FileName = source.FileName;
			destination.FullPath = source.FullPath;
			destination.Modified = source.Modified;
			destination.Part = source.Part;
			destination.SubDirectory = source.SubDirectory;
			destination.Status = source.Status;
			destination.EntityGuid = source.EntityGuid;
			destination.Url = source.Url;
			destination.ReTryCount = source.ReTryCount;
			destination.NextTryCount = source.NextTryCount;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\FileInfoMapper.cs", 232);
			throw;
		}
		return destination;
	}

	public Task<List<FileInfoReTryDownloadBusiness>> MapListAsync(string contextGuid, IEnumerable<FileInfoReTryDownloadBusiness> source, List<FileInfoReTryDownloadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<FileInfoBusiness>> MapListAsync(string contextGuid, IEnumerable<FileInfoBusiness> source, List<FileInfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<FileInfoBusiness>> MapListAsync(string contextGuid, IEnumerable<FileInfoReTryDownloadBusiness> source, List<FileInfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<FileInfoBusiness>> MapListAsync(string contextGuid, IEnumerable<WebFileInfo> source, List<FileInfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<FileInfoReTryDownloadBusiness>> MapListAsync(string contextGuid, IEnumerable<FileInfoBusiness> source, List<FileInfoReTryDownloadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
