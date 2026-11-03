using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Tickets;
using EntityCache.Business.Tickets;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Tickets;

internal class mapper : IItemAsyncMapper<TicketDetBusiness, TicketDet>, IItemAsyncMapper<TicketDet, TicketDetBusiness>
{
	public Task<TicketDet> MapAsync(string contextGuid, TicketDetBusiness source, TicketDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TicketDet();
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
			if (source.MetaData != null)
			{
				destination.MetaData = Json.ToStringJson(source.MetaData);
			}
			else
			{
				destination.MetaData = "";
			}
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketDetPersistenceRepositoryBase .cs", 73);
			throw;
		}
	}

	public Task<TicketDetBusiness> MapAsync(string contextGuid, TicketDet source, TicketDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketDetPersistenceRepositoryBase .cs"))
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
				destination.DateM = source.DateM;
				if (!string.IsNullOrEmpty(source.MetaData))
				{
					destination.MetaData = source.MetaData.FromJson<TicketMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketDetPersistenceRepositoryBase .cs", 99);
				}
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketDetPersistenceRepositoryBase .cs", 106);
				throw;
			}
		}
	}

	public Task<List<TicketDet>> MapListAsync(string contextGuid, IEnumerable<TicketDetBusiness> source, List<TicketDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TicketDetBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketDet> source, List<TicketDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
