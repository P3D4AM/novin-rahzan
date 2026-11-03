using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class FileManagementMapper : IItemAsyncMapper<FileManagementBusiness, WebFileManagement>, IItemAsyncMapper<FileManagementBusiness, FileManagementBusiness>, IItemAsyncMapper<WebFileManagement, FileManagementBusiness>
{
	public static FileManagementMapper Instance { get; private set; } = new FileManagementMapper();


	public Task<WebFileManagement> MapAsync(string contextGuid, FileManagementBusiness source, WebFileManagement destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebFileManagement();
			}
			destination.CRC = source.CRC;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.FileName = source.FileName;
			destination.FullURL = source.FullURL;
			destination.PageURL = source.PageURL;
			destination.PartID = source.PartID;
			destination.Title = source.Title;
			destination.Version = source.Version;
			destination.VersionTXT = source.VersionTXT;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\FileManagementMapper.cs", 40);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebFileManagement>> MapListAsync(string contextGuid, IEnumerable<FileManagementBusiness> source, List<WebFileManagement> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<FileManagementBusiness> MapAsync(string contextGuid, FileManagementBusiness source, FileManagementBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FileManagementBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.CRC = source.CRC;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.FileName = source.FileName;
			destination.FullURL = source.FullURL;
			destination.PageURL = source.PageURL;
			destination.PartID = source.PartID;
			destination.Title = source.Title;
			destination.Version = source.Version;
			destination.VersionTXT = source.VersionTXT;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\FileManagementMapper.cs", 73);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<FileManagementBusiness>> MapListAsync(string contextGuid, IEnumerable<FileManagementBusiness> source, List<FileManagementBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<FileManagementBusiness> MapAsync(string contextGuid, WebFileManagement source, FileManagementBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FileManagementBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.CRC = source.CRC;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.FileName = source.FileName;
			destination.FullURL = source.FullURL;
			destination.PageURL = source.PageURL;
			destination.PartID = source.PartID;
			destination.Title = source.Title;
			destination.Version = source.Version;
			destination.VersionTXT = source.VersionTXT;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\FileManagementMapper.cs", 106);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<FileManagementBusiness>> MapListAsync(string contextGuid, IEnumerable<WebFileManagement> source, List<FileManagementBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
