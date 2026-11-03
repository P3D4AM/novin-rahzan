using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Tickets;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository.Tickets;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData.Ticket;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global.Tickets;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Global.Tickets;

namespace AccSqlServerPersistence.Persistence.Repositories.Tickets;

internal class TicketPartPersistenceRepository : PersistenceRepository<TicketPartBusiness, TicketPart>, ITicketPartRepository, IRepository<TicketPartBusiness>, INoGuidRepository<TicketPartBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TicketPartBusiness, TicketPart>, IItemAsyncMapper<TicketPart, TicketPartBusiness>
	{
		public Task<TicketPart> MapAsync(string contextGuid, TicketPartBusiness source, TicketPart destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs"))
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
						destination = new TicketPart();
					}
					destination.Guid = source.Guid;
					destination.Status = source.Status;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.ParentGuid = source.ParentGuid;
					destination.Description = source.Description;
					destination.Ordinal = source.Ordinal;
					if (source.MetaData != null)
					{
						destination.MetaData = Json.ToStringJson(source.MetaData);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 199);
					throw;
				}
			}
		}

		public Task<TicketPartBusiness> MapAsync(string contextGuid, TicketPart source, TicketPartBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs"))
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
						destination = new TicketPartBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Status = source.Status;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.ParentGuid = source.ParentGuid;
					destination.Description = source.Description;
					destination.Ordinal = source.Ordinal;
					if (!string.IsNullOrEmpty(source.MetaData))
					{
						destination.MetaData = source.MetaData.FromJson<TicketPartMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 223);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 230);
					throw;
				}
			}
		}

		public Task<List<TicketPart>> MapListAsync(string contextGuid, IEnumerable<TicketPartBusiness> source, List<TicketPart> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TicketPartBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketPart> source, List<TicketPartBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private TicketPartDetailPersistenceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<TicketPart, TicketPartBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TicketPartBusiness, TicketPart> MapToDatabase => Mapper;

	public override IItemMapper<TicketPart, TicketPartBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public TicketPartPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new TicketPartDetailPersistenceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				_detRepo?.Dispose();
				_detRepo = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 48);
		}
	}

	public override async Task<TicketPartBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 52));
		TicketPartBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				TicketPart ticketPart = await c.TicketPart.FindAsync(new object[1] { guid });
				if (ticketPart == null)
				{
					result = null;
				}
				else
				{
					TicketPartBusiness mappedItem = await MapFromDatabase.MapAsync(ContextGuid, ticketPart, null, tranName, token);
					await _detRepo.GetAllAsync(mappedItem, token, tranName);
					result = mappedItem;
				}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 65);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<TicketPartBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 76));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs"))
		{
			TicketPartBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					TicketPart ticketPart = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TicketPart>((IQueryable<TicketPart>)c.TicketPart, (Expression<Func<TicketPart, bool>>)((TicketPart x) => x.Name == name), token);
					if (ticketPart == null)
					{
						result = null;
					}
					else
					{
						TicketPartBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, ticketPart, null, tranName, token);
						await _detRepo.GetAllAsync(ret, token, tranName);
						result = ret;
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 91);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<IEnumerable<TicketPartBusiness>> GetSubItemsAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("Use Cache !!!");
	}

	public async Task<short> GetNextOrdinalAsync(CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 103));
		short result;
		try
		{
			_ = 1;
			try
			{
				result = (short)(((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<TicketPart>(((IQueryable<TicketPart>)c.TicketPart).OrderByDescending((TicketPart x) => x.Ordinal).Take(1), token))?.Ordinal ?? 0) + 1);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 113);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TicketPartBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 129));
				TicketPart ticketPart = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(ticketPart, ticketPart.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 131);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 133);
				if (ret.HasError)
				{
					return ret;
				}
				if (item.DetailList?.Any() ?? false)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetailList?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 136);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 143);
				ret.AddReturnedValue(ex3, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 144);
			}
			return ret;
		}
	}

	public async Task<List<Guid>> GetAllRelatedPartsAsync(Guid targetGuid, CancellationToken token, string tranName)
	{
		Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllRelatedPartsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 150));
		List<Guid> result;
		try
		{
			if (targetGuid == Guid.Empty)
			{
				result = null;
			}
			else
			{
				List<Guid> list = ((IQueryable<TicketPartDetail>)global_ModelContext.TicketPartDetail).Where((TicketPartDetail q) => q.TargetGuid == targetGuid)?.Select((TicketPartDetail q) => q.MasterGuid)?.Distinct()?.ToList();
				result = list;
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetAllRelatedPartsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs", 161);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await global_ModelContext.DisposeAsync("GetAllRelatedPartsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartPersistenceRepository.cs");
			}
		}
		return result;
	}
}
