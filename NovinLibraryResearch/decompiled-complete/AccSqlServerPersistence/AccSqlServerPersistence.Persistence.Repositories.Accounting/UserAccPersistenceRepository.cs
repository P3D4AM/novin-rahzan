using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Cache.Repository.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class UserAccPersistenceRepository : PersistenceSyncRepository<UserAccBusiness, UserAcc>, IUserAccRepository, ISyncRepository<UserAccBusiness>, IEditableDelivery, IRepository<UserAccBusiness>, INoGuidRepository<UserAccBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<UserAccBusiness, UserAcc>, IItemAsyncMapper<UserAcc, UserAccBusiness>, IItemMapper<UserAcc, UserAccBusiness>
	{
		public Task<UserAcc> MapAsync(string contextGuid, UserAccBusiness source, UserAcc destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new UserAcc();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				if (string.IsNullOrEmpty(destination.Description))
				{
					destination.Description = "";
				}
				destination.Name = source.Name;
				destination.Status = source.Status;
				destination.UserTime = source.UserTime;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage ?? "";
				destination.DeliveryShortMessage = source.DeliveryShortMessage ?? "";
				destination.HostRefrenced = source.HostRefrenced ?? "";
				destination.UserAccessStr = source.UserAccess.ToString().EncryptData(source.Guid.ToString());
				UserAcc userAcc = destination;
				if (userAcc.SerializedData == null)
				{
					string text2 = (userAcc.SerializedData = "");
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 284);
				throw;
			}
		}

		public Task<UserAccBusiness> MapAsync(string contextGuid, UserAcc source, UserAccBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public UserAccBusiness Map(string contextGuid, UserAcc source, UserAccBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new UserAccBusiness(contextGuid, source.Guid);
				}
				destination.ContextGuid = contextGuid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Name = source.Name;
				destination.Status = source.Status;
				destination.UserAccess = source.UserAccess;
				destination.UserTime = source.UserTime;
				destination.UserAccess = source.UserAccessStr.DecryptString(source.Guid.ToString()).FromJson<AccessLevel>("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 307);
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				return destination;
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
				WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 320);
				throw;
			}
		}

		public Task<List<UserAcc>> MapListAsync(string contextGuid, IEnumerable<UserAccBusiness> source, List<UserAcc> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<UserAccBusiness>> MapListAsync(string contextGuid, IEnumerable<UserAcc> source, List<UserAccBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<UserAcc, UserAccBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<UserAccBusiness, UserAcc> MapToDatabase => Mapper;

	public override IItemMapper<UserAcc, UserAccBusiness> MapFromDatabaseNoneAsync => Mapper;

	public UserAccPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public new async Task<UserAccBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 29));
			UserAccBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					UserAcc userAcc = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserAcc>((IQueryable<UserAcc>)c.UserAcc, (Expression<Func<UserAcc, bool>>)((UserAcc x) => x.Guid == guid), token);
					result = ((userAcc != null) ? (await MapFromDatabase.MapAsync(ContextGuid, userAcc, null, tranName, token)) : null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 42);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<IEnumerable<UserAccBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", $"PersistenceNoGuidRepository.GetAll_{typeof(UserAccBusiness)}"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 55));
			IEnumerable<UserAccBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<UserAcc> source = ((IEnumerable<UserAcc>)c.UserAcc)?.ToList();
					result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 66);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 78));
			List<Guid> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<Guid> list = new List<Guid>();
					List<UserAcc> list2 = await EntityFrameworkQueryableExtensions.ToListAsync<UserAcc>(((IQueryable<UserAcc>)c.UserAcc).Where((UserAcc x) => x.ServerStatus == ServerStatusForClient.None), token);
					if (list2 != null && list2.Any())
					{
						list.AddRange(list2.Select((UserAcc q) => q.Guid).ToList());
					}
					if (list2 == null)
					{
						new List<UserAcc>();
					}
					List<Cashier> list3 = await EntityFrameworkQueryableExtensions.ToListAsync<Cashier>(((IQueryable<Cashier>)c.Cashiers).Where((Cashier x) => x.ServerStatus == ServerStatusForClient.None), token);
					if (list3 != null && list3.Any())
					{
						list.AddRange(list3.Select((Cashier q) => q.UserGuid).ToList());
					}
					result = list.Distinct().ToList();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 98);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 110));
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				token.ThrowIfCancellationRequested();
				UserAcc userAcc = await context.UserAcc.FindAsync(new object[1] { item.Guid });
				if (userAcc == null)
				{
					return ret;
				}
				userAcc.HostRefrenced = item.HostRefrenced;
				userAcc.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				userAcc.DeliveryMessage = item.Message;
				userAcc.DeliveryShortMessage = item.Shortmessage;
				((DbContext)context).Update<UserAcc>(userAcc);
				List<Cashier> list = await EntityFrameworkQueryableExtensions.ToListAsync<Cashier>(((IQueryable<Cashier>)context.Cashiers).Where((Cashier q) => q.UserGuid == item.Guid), token);
				if (list == null || !list.Any())
				{
					return ret;
				}
				foreach (Cashier item2 in list)
				{
					item2.HostRefrenced = item.HostRefrenced;
					item2.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
					item2.DeliveryMessage = item.Message;
					item2.DeliveryShortMessage = item.Shortmessage;
					((DbContext)context).Update<Cashier>(item2);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 137);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 141);
				ret.AddReturnedValue(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 142);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(UserAccBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 154));
				if (item == null)
				{
					return ret;
				}
				UserAcc userAcc = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(userAcc, userAcc.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 157);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "Data:" + Json.ToStringJson(item), "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 161);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 162);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<UserAccBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (UserAccBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 174);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 178);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 179);
			}
			return ret;
		}
	}

	public async Task<UserAccBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 187));
			UserAccBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					UserAcc source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserAcc>((IQueryable<UserAcc>)c.UserAcc, (Expression<Func<UserAcc, bool>>)((UserAcc x) => x.Name == name), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 198);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<UserAccBusiness> GetAsync(short id, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 210));
			UserAccBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					UserAcc source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<UserAcc>((IQueryable<UserAcc>)c.UserAcc, (Expression<Func<UserAcc, bool>>)((UserAcc x) => x.Id == id), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 221);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs");
				}
			}
			return result;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\UserAccPersistenceRepository.cs", 242);
		}
	}
}
