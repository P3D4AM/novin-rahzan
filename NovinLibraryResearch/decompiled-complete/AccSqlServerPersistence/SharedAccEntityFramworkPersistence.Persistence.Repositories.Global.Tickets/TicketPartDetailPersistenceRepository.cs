using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository.Global.Ticket;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global.Tickets;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global.Tickets;

internal class TicketPartDetailPersistenceRepository : PersistenceMasterRepositoryNoGuid<TicketPartDetailBusiness, TicketPartDetail>, ITicketPartDetailRepository, INoGuidRepository<TicketPartDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TicketPartDetailBusiness, TicketPartDetail>, IItemAsyncMapper<TicketPartDetail, TicketPartDetailBusiness>
	{
		public Task<TicketPartDetail> MapAsync(string contextGuid, TicketPartDetailBusiness source, TicketPartDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TicketPartDetail();
				}
				destination.MasterGuid = source.MasterGuid;
				destination.TargetGuid = source.TargetGuid;
				destination.TargetType = source.TargetType;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 155);
				throw;
			}
		}

		public Task<TicketPartDetailBusiness> MapAsync(string contextGuid, TicketPartDetail source, TicketPartDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TicketPartDetailBusiness(contextGuid);
				}
				destination.MasterGuid = source.MasterGuid;
				destination.TargetGuid = source.TargetGuid;
				destination.TargetType = source.TargetType;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 179);
				throw;
			}
		}

		public Task<List<TicketPartDetail>> MapListAsync(string contextGuid, IEnumerable<TicketPartDetailBusiness> source, List<TicketPartDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TicketPartDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TicketPartDetail> source, List<TicketPartDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<TicketPartDetail, TicketPartDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TicketPartDetailBusiness, TicketPartDetail> MapToDatabase => Mapper;

	public TicketPartDetailPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<TicketPartDetailBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (!(items?.Any() ?? false))
				{
					return res;
				}
				List<TicketPartDetail> des = await MapToDatabase.MapListAsync(base.ContextGuid, items, null, tranName, token);
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 39);
				foreach (TicketPartDetail item in des)
				{
					item.MasterGuid = masterGuid;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item, item.MasterGuid, item.TargetGuid, item.TargetType), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 43);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 49);
				res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 50);
			}
			return res;
		}
	}

	public override Task<List<TicketPartDetailBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task GetAllAsync(TicketPartBusiness master, CancellationToken token, string tranName)
	{
		if (IsDisposed || token.IsCancellationRequested)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 59));
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					List<TicketPartDetail> list = await EntityFrameworkQueryableExtensions.ToListAsync<TicketPartDetail>(((IQueryable<TicketPartDetail>)c.TicketPartDetail).Where((TicketPartDetail x) => x.MasterGuid == master.Guid), token);
					if (list == null || !list.Any())
					{
						goto end_IL_0133;
					}
					foreach (TicketPartDetail item in list)
					{
						TicketPartDetailBusiness det = await MapFromDatabase.MapAsync(base.Context.Guid, item, null, tranName, token);
						await master.AddToDetailListAsync(det);
					}
					goto end_IL_011c;
					end_IL_0133:;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 74);
					throw;
				}
				return;
				end_IL_011c:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs");
				}
			}
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 92);
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(TicketPartDetailBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 102));
				TicketPartDetail ticketPartDetail = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(ticketPartDetail, ticketPartDetail.MasterGuid, ticketPartDetail.TargetGuid, ticketPartDetail.TargetType), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 104);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 108);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 109);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TicketPartDetailBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				if (items != null && items.Any())
				{
					foreach (TicketPartDetailBusiness item in items)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 122);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 126);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketPartDetailPersistenceRepository.cs", 127);
			}
			return ret;
		}
	}

	public Task<IEnumerable<TicketPartDetailBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}
}
