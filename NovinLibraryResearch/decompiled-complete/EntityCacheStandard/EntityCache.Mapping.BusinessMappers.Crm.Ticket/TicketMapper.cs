using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Crm.Ticket;

public class TicketMapper : IItemAsyncMapper<TicketBusiness, WebTicket>, IItemAsyncMapper<TicketBusiness, TicketBusiness>, IItemAsyncMapper<WebTicket, TicketBusiness>, IItemAsyncMapper<TicketReportBusiness, WebTicketReport>, IItemAsyncMapper<WebTicketReport, TicketReportBusiness>, IItemAsyncMapper<TicketReportBusiness, TicketBusiness>
{
	public static TicketMapper Instance { get; private set; } = new TicketMapper();


	public async Task<WebTicket> MapAsync(string contextGuid, TicketBusiness source, WebTicket destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTicket();
			}
			destination.Guid = source.Guid;
			destination.Number = source.Number;
			destination.TargetType = source.TargetType;
			destination.SourceGuid = source.SourceGuid;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.TargetGuid = source.TargetGuid;
			destination.StatusId = source.StatusId;
			destination.ReferenceGuid = source.ReferenceGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.SourceType = source.SourceType;
			destination.Priority = source.Priority;
			if (source.AddedFiles?.Any() ?? false)
			{
				WebTicket webTicket = destination;
				webTicket.AddedFiles = await FileInfoMapper.Instance.MapListAsync(contextGuid, source.AddedFiles, new List<WebFileInfo>(), tranName, token);
			}
			if (source.RemovedFiles?.Any() ?? false)
			{
				WebTicket webTicket = destination;
				webTicket.RemovedFiles = await FileInfoMapper.Instance.MapListAsync(contextGuid, source.RemovedFiles, new List<WebFileInfo>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketMapper.cs", 55);
			throw;
		}
	}

	public Task<List<WebTicket>> MapListAsync(string contextGuid, IEnumerable<TicketBusiness> source, List<WebTicket> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<TicketBusiness> MapAsync(string contextGuid, TicketBusiness source, TicketBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Number = source.Number;
			destination.SourceGuid = source.SourceGuid;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.TargetType = source.TargetType;
			destination.TargetGuid = source.TargetGuid;
			destination.StatusId = source.StatusId;
			destination.ReferenceGuid = source.ReferenceGuid;
			destination.Modified = source.Modified;
			destination.SourceType = source.SourceType;
			destination.Status = source.Status;
			destination.Priority = source.Priority;
			if (source.AddedFiles != null)
			{
				foreach (FileInfoBusiness addedFile in source.AddedFiles)
				{
					TicketBusiness ticketBusiness = destination;
					await ticketBusiness.AddFileAsync(await FileInfoMapper.Instance.MapAsync(contextGuid, addedFile, new FileInfoBusiness(contextGuid), tranName, token));
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketMapper.cs", 93);
			throw;
		}
	}

	public Task<List<TicketBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketBusiness> source, List<TicketBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<TicketBusiness> MapAsync(string contextGuid, WebTicket source, TicketBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Number = source.Number;
			destination.SourceGuid = source.SourceGuid;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.TargetType = source.TargetType;
			destination.SourceType = source.SourceType;
			destination.TargetGuid = source.TargetGuid;
			destination.StatusId = source.StatusId;
			destination.ReferenceGuid = source.ReferenceGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Priority = source.Priority;
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
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketMapper.cs", 138);
			throw;
		}
	}

	public Task<List<TicketBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTicket> source, List<TicketBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.Number = source.Number;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.SourceName = source.SourceName;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.TargetType = source.TargetType;
			destination.TargetGuid = source.TargetGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.PartName = source.PartName;
			destination.StatusId = source.StatusId;
			destination.StatusName = source.StatusName;
			destination.ReferenceGuid = source.ReferenceGuid;
			destination.Priority = source.Priority;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketMapper.cs", 176);
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
			destination.Guid = source.Guid;
			destination.Number = source.Number;
			destination.SourceGuid = source.SourceGuid;
			destination.SourceType = source.SourceType;
			destination.SourceName = source.SourceName;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.TargetType = source.TargetType;
			destination.TargetGuid = source.TargetGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.PartName = source.PartName;
			destination.StatusId = source.StatusId;
			destination.StatusName = source.StatusName;
			destination.ReferenceGuid = source.ReferenceGuid;
			destination.Priority = source.Priority;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketMapper.cs", 214);
			throw;
		}
	}

	public Task<List<TicketReportBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTicketReport> source, List<TicketReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<TicketBusiness> MapAsync(string contextGuid, TicketReportBusiness source, TicketBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Number = source.Number;
			destination.SourceGuid = source.SourceGuid;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.Description = source.Description;
			destination.TargetType = source.TargetType;
			destination.TargetGuid = source.TargetGuid;
			destination.SourceType = source.SourceType;
			destination.StatusId = source.StatusId;
			destination.ReferenceGuid = source.ReferenceGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Priority = source.Priority;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Ticket\\TicketMapper.cs", 248);
			throw;
		}
	}

	public Task<List<TicketBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketReportBusiness> source, List<TicketBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
