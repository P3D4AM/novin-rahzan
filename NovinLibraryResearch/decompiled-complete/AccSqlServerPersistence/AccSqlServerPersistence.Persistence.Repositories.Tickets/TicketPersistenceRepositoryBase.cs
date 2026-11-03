using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Tickets;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Ticket;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Tickets;

internal abstract class TicketPersistenceRepositoryBase : PersistenceRepository<TicketBusiness, Ticket>, ITicketRepository, IRepository<TicketBusiness>, INoGuidRepository<TicketBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TicketBusiness, Ticket>, IItemAsyncMapper<Ticket, TicketBusiness>
	{
		public Task<Ticket> MapAsync(string contextGuid, TicketBusiness source, Ticket destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs"))
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
						destination = new Ticket();
					}
					destination.Guid = source.Guid;
					destination.TargetType = source.TargetType;
					destination.TargetGuid = source.TargetGuid;
					destination.SourceGuid = source.SourceGuid;
					destination.Title = source.Title;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.StatusId = source.StatusId;
					destination.ReferenceGuid = source.ReferenceGuid;
					destination.Modified = source.Modified;
					destination.Status = source.Status;
					destination.Number = source.Number;
					destination.SourceType = source.SourceType;
					destination.Priority = source.Priority;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 174);
					throw;
				}
			}
		}

		public Task<TicketBusiness> MapAsync(string contextGuid, Ticket source, TicketBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs"))
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
						destination = new TicketBusiness(contextGuid);
					}
					destination.Guid = source.Guid;
					destination.SourceGuid = source.SourceGuid;
					destination.Title = source.Title;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.TargetType = source.TargetType;
					destination.TargetGuid = source.TargetGuid;
					destination.StatusId = source.StatusId;
					destination.ReferenceGuid = source.ReferenceGuid;
					destination.Modified = source.Modified;
					destination.Status = source.Status;
					destination.Number = source.Number;
					destination.Priority = source.Priority;
					destination.SourceType = source.SourceType;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 209);
					throw;
				}
			}
		}

		public Task<List<Ticket>> MapListAsync(string contextGuid, IEnumerable<TicketBusiness> source, List<Ticket> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TicketBusiness>> MapListAsync(string contextGuid, IEnumerable<Ticket> source, List<TicketBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => false;

	public override bool HasModified => false;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Ticket, TicketBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TicketBusiness, Ticket> MapToDatabase => Mapper;

	public override IItemMapper<Ticket, TicketBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public TicketPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 41);
		}
	}

	public virtual Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(Guid guid, short statusId, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 51));
				token.ThrowIfCancellationRequested();
				Ticket ticket = global_ModelContext.Ticket.Find(new object[1] { guid });
				if (ticket == null)
				{
					return res;
				}
				ticket.StatusId = statusId;
				ticket.Modified = DateTime.Now;
				((DbContext)global_ModelContext).Update<Ticket>(ticket);
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 59);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 62);
				res.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 63);
			}
			return res;
		}
	}

	public async Task<List<TicketTitleSuggest>> GetSuggestTitelsAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSuggestTitelsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs"))
		{
			Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetSuggestTitelsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 70));
			List<TicketTitleSuggest> result;
			try
			{
				List<string> list = ((IQueryable<Ticket>)global_ModelContext.Ticket).Select((Ticket q) => q.Title).Distinct().ToList();
				result = ((list != null && list.Any()) ? list.Select((string q) => new TicketTitleSuggest
				{
					IsChecked = false,
					Titel = q
				})?.ToList() : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetSuggestTitelsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 85);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await global_ModelContext.DisposeAsync("GetSuggestTitelsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> NextNumberAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs"))
		{
			Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 97));
			int result;
			try
			{
				IQueryable<int> source = ((IQueryable<Ticket>)global_ModelContext.Ticket).Select((Ticket q) => q.Number);
				int num = (source.Any() ? source.Max() : 0);
				result = num + 1;
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
				WebErrorLog.Instance.StartLog(ex2, "", "NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 108);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await global_ModelContext.DisposeAsync("NextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<TicketBusiness> GetByNumberAsync(int number, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 120));
			TicketBusiness result;
			try
			{
				_ = 1;
				try
				{
					Ticket source = ((IQueryable<Ticket>)c.Ticket).Where((Ticket q) => q.Number == number).SingleOrDefault();
					result = await Mapper.MapAsync(ContextGuid, source);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs", 131);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}
}
