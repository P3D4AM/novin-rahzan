using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Crm.Ticket;

public class TicketDetMapper : IItemAsyncMapper<TicketDetBusiness, WebTicketDet>, IItemAsyncMapper<TicketDetBusiness, TicketDetBusiness>, IItemAsyncMapper<WebTicketDet, TicketDetBusiness>, IItemAsyncMapper<TicketReportBusiness, TicketDetBusiness>, IItemAsyncMapper<TicketDetBusiness, TicketReportBusiness>, IItemAsyncMapper<TicketReportBusiness, WebTicketDet>, IItemAsyncMapper<WebTicketDet, TicketReportBusiness>, IItemAsyncMapper<TicketReportBusiness, WebTicketReport>, IItemAsyncMapper<WebTicketReport, TicketReportBusiness>
{
	public static TicketDetMapper Instance { get; private set; } = new TicketDetMapper();


	public async Task<WebTicketDet> MapAsync(string contextGuid, TicketDetBusiness source, WebTicketDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTicketDet();
			}
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.IsSeen = source.IsSeen;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			if (source.AddedFiles?.Any() ?? false)
			{
				WebTicketDet webTicketDet = destination;
				webTicketDet.AddedFiles = await FileInfoMapper.Instance.MapListAsync(contextGuid, source.AddedFiles, new List<WebFileInfo>(), tranName, token);
			}
			if (source.RemovedFiles?.Any() ?? false)
			{
				WebTicketDet webTicketDet = destination;
				webTicketDet.RemovedFiles = await FileInfoMapper.Instance.MapListAsync(contextGuid, source.RemovedFiles, new List<WebFileInfo>(), tranName, token);
			}
			if (source.MetaData != null)
			{
				destination.MetaData = new TicketMetaData
				{
					DepId = source.MetaData.DepId,
					LkSerial = source.MetaData.LkSerial,
					Source = source.MetaData.Source,
					UserId = source.MetaData.UserId,
					AndroidIme = source.MetaData.AndroidIme,
					UserName = source.MetaData.UserName
				};
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketDetMapper.cs", 65);
			throw;
		}
	}

	public Task<List<WebTicketDet>> MapListAsync(string contextGuid, IEnumerable<TicketDetBusiness> source, List<WebTicketDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<TicketDetBusiness> MapAsync(string contextGuid, TicketDetBusiness source, TicketDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.IsSeen = source.IsSeen;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.Description = source.Description;
			if (source.MetaData != null)
			{
				destination.MetaData = new TicketMetaData
				{
					DepId = source.MetaData.DepId,
					LkSerial = source.MetaData.LkSerial,
					Source = source.MetaData.Source,
					UserId = source.MetaData.UserId,
					AndroidIme = source.MetaData.AndroidIme,
					UserName = source.MetaData.UserName
				};
			}
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketDetMapper.cs", 104);
			throw;
		}
	}

	public Task<List<TicketDetBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketDetBusiness> source, List<TicketDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<TicketDetBusiness> MapAsync(string contextGuid, WebTicketDet source, TicketDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new TicketDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.IsSeen = source.IsSeen;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			if (source.AddedFiles?.Any() ?? false)
			{
				foreach (FileInfoBusiness item in await FileInfoMapper.Instance.MapListAsync(contextGuid, source.AddedFiles, new List<FileInfoBusiness>(), tranName, token))
				{
					await destination.AddFileAsync(item);
				}
			}
			if (source.RemovedFiles?.Any() ?? false)
			{
				foreach (FileInfoBusiness item2 in await FileInfoMapper.Instance.MapListAsync(contextGuid, source.RemovedFiles, new List<FileInfoBusiness>(), tranName, token))
				{
					await destination.RemoveFileAsync(item2);
				}
			}
			if (source.MetaData != null)
			{
				destination.MetaData = new TicketMetaData
				{
					DepId = source.MetaData.DepId,
					LkSerial = source.MetaData.LkSerial,
					Source = source.MetaData.Source,
					UserId = source.MetaData.UserId,
					AndroidIme = source.MetaData.AndroidIme,
					UserName = source.MetaData.UserName
				};
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketDetMapper.cs", 154);
			throw;
		}
	}

	public Task<List<TicketDetBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTicketDet> source, List<TicketDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebTicketDet> MapAsync(string contextGuid, TicketReportBusiness source, WebTicketDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTicketDet();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.MasterGuid = source?.MasterGuid ?? Guid.Empty;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketDetMapper.cs", 184);
			throw;
		}
	}

	public Task<TicketReportBusiness> MapAsync(string contextGuid, WebTicketDet source, TicketReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketReportBusiness();
			}
			destination.Guid = source.Guid;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketDetMapper.cs", 211);
			throw;
		}
	}

	public Task<List<WebTicketDet>> MapListAsync(string contextGuid, IEnumerable<TicketReportBusiness> source, List<WebTicketDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TicketReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTicketDet> source, List<TicketReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<TicketReportBusiness> MapAsync(string contextGuid, TicketDetBusiness source, TicketReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketReportBusiness();
			}
			destination.Guid = source.Guid;
			destination.AddedFiles = source.AddedFiles;
			destination.RemovedFiles = source.RemovedFiles;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.MasterGuid = source.MasterGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketDetMapper.cs", 245);
			throw;
		}
	}

	public Task<List<TicketReportBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketDetBusiness> source, List<TicketReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<TicketDetBusiness> MapAsync(string contextGuid, TicketReportBusiness source, TicketDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.MasterGuid = source?.MasterGuid ?? Guid.Empty;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketDetMapper.cs", 277);
			throw;
		}
	}

	public Task<List<TicketDetBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketReportBusiness> source, List<TicketDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<WebTicketReport> MapAsync(string contextGuid, TicketReportBusiness source, WebTicketReport destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTicketReport();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.TargetType = source.TargetType;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
			destination.SourceName = source.SourceName;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketDetMapper.cs", 308);
			throw;
		}
	}

	public Task<List<WebTicketReport>> MapListAsync(string contextGuid, IEnumerable<TicketReportBusiness> source, List<WebTicketReport> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<TicketReportBusiness> MapAsync(string contextGuid, WebTicketReport source, TicketReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketReportBusiness();
			}
			destination.TargetType = source.TargetType;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.Description = source.Description;
			destination.SourceName = source.SourceName;
			destination.DateM = source.DateM;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketDetMapper.cs", 339);
			throw;
		}
	}

	public Task<List<TicketReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTicketReport> source, List<TicketReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
