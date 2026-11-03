using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Coding;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Coding;

internal class AccountPersistenceRepository : PersistenceSyncRepository<AccountBusiness, AccountClass>, IAccountRepository, ISyncRepository<AccountBusiness>, IEditableDelivery, IRepository<AccountBusiness>, INoGuidRepository<AccountBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AccountBusiness, AccountClass>, IItemAsyncMapper<AccountClass, AccountBusiness>, IItemAsyncMapper<WebAccount, AccountClass>
	{
		public Task<AccountClass> MapAsync(string contextGuid, AccountBusiness source, AccountClass destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AccountClass();
				}
				destination.Guid = source.Guid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Account = source.Account;
				destination.Status = source.Status;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage ?? "";
				destination.DeliveryShortMessage = source.DeliveryShortMessage ?? "";
				destination.HostRefrenced = source.HostRefrenced ?? "";
				destination.DeliveryDate = source.DeliveryDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 470);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<AccountBusiness> MapAsync(string contextGuid, AccountClass source, AccountBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AccountBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Account = source.Account;
				destination.Status = source.Status;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced ?? "";
				destination.DeliveryDate = source.DeliveryDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 502);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<AccountClass> MapAsync(string contextGuid, WebAccount source, AccountClass destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AccountClass();
				}
				destination.Guid = source.Guid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.Account = source.Account;
				destination.Status = source.Status;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage ?? "";
				destination.DeliveryShortMessage = source.DeliveryShortMessage ?? "";
				destination.HostRefrenced = source.HostRefrenced ?? "";
				destination.DeliveryDate = source.DeliveryDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 534);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<AccountClass>> MapListAsync(string contextGuid, IEnumerable<AccountBusiness> source, List<AccountClass> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AccountBusiness>> MapListAsync(string contextGuid, IEnumerable<AccountClass> source, List<AccountBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AccountClass>> MapListAsync(string contextGuid, IEnumerable<WebAccount> source, List<AccountClass> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<AccountClass, AccountBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AccountBusiness, AccountClass> MapToDatabase => Mapper;

	public override IItemMapper<AccountClass, AccountBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public AccountPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 43);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(AccountBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<AccountBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid moeinGuid, Guid tafsilGuid, decimal account, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				_ = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 54));
				AccountBusiness item = new AccountBusiness(ContextGuid)
				{
					MoeinGuid = moeinGuid,
					TafsilGuid = tafsilGuid,
					Account = account,
					ServerStatus = ServerStatusForClient.None
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, 1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 62);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 66);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 67);
			}
			return res;
		}
	}

	public async Task<IEnumerable<AccountTableViewModel>> GetAllAsync(Guid tafsilGuid, bool? status, string tranName, CancellationToken token)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 73));
		IEnumerable<AccountTableViewModel> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<AccountClass> source = ((IQueryable<AccountClass>)c.Accounts).Where((AccountClass q) => q.TafsilGuid == tafsilGuid);
				if (status.HasValue)
				{
					source = source.Where((AccountClass q) => q.Status == ((bool?)status).Value);
				}
				result = await EntityFrameworkQueryableExtensions.ToListAsync<AccountTableViewModel>(source.Select((AccountClass q) => new AccountTableViewModel
				{
					Account = q.Account,
					MoeinCode = q.Moein.Code,
					MoeinGuid = q.MoeinGuid,
					MoeinName = q.Moein.Name,
					TafsilCode = q.Tafsil.Code,
					TafsilGuid = q.TafsilGuid,
					TafsilName = q.Tafsil.Name,
					Guid = q.Guid
				}), token);
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
				token.ThrowIfCancellationRequested();
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 96);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<decimal> GetAccountSumAsync(List<Guid> moeinGuidList, List<Guid> tafsilGuidList, bool? status, string tranName, CancellationToken token)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAccountSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 107));
		decimal result;
		try
		{
			IQueryable<AccountClass> queryable = (IQueryable<AccountClass>)acc_ModelContext.Accounts;
			if (moeinGuidList?.Any() ?? false)
			{
				queryable = queryable?.Where((AccountClass q) => moeinGuidList.Contains(q.MoeinGuid));
			}
			if (tafsilGuidList?.Any() ?? false)
			{
				queryable = queryable?.Where((AccountClass q) => tafsilGuidList.Contains(q.TafsilGuid));
			}
			if (status.HasValue)
			{
				queryable = queryable?.Where((AccountClass q) => q.Status == ((bool?)status).Value);
			}
			result = ((queryable != null && queryable.Any()) ? queryable.Sum((AccountClass q) => q.Account) : default(decimal));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAccountSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 125);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetAccountSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<decimal> GetAccountSumAsync(Guid tafsilGuid, List<Guid> moeinGuids, string tranName, CancellationToken token)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAccountSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 136));
		decimal result;
		try
		{
			IQueryable<AccountClass> queryable = ((IQueryable<AccountClass>)acc_ModelContext.Accounts)?.Where((AccountClass q) => q.TafsilGuid == tafsilGuid);
			if (moeinGuids?.Any() ?? false)
			{
				queryable = queryable?.Where((AccountClass q) => moeinGuids.Contains(q.MoeinGuid));
			}
			result = ((queryable != null && queryable.Any()) ? queryable.Sum((AccountClass q) => q.Account) : default(decimal));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAccountSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 152);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetAccountSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<AccountBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetFromDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 163));
		AccountBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				AccountClass accountClass = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<AccountClass>((IQueryable<AccountClass>)c.Accounts, (Expression<Func<AccountClass, bool>>)((AccountClass q) => q.Guid == guid), token);
				result = ((accountClass != null) ? (await MapFromDatabase.MapAsync(ContextGuid, accountClass, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetFromDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 174);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetFromDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override Task<AccountBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetFromDatabaseAsync(guid, token, tranName);
	}

	public override async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 187));
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				token.ThrowIfCancellationRequested();
				AccountClass accountClass = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<AccountClass>((IQueryable<AccountClass>)context.Accounts, (Expression<Func<AccountClass, bool>>)((AccountClass q) => q.Guid == item.Guid), token);
				if (accountClass == null)
				{
					return ret;
				}
				accountClass.HostRefrenced = item.HostRefrenced ?? "";
				accountClass.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				accountClass.DeliveryMessage = item.Message;
				accountClass.DeliveryShortMessage = item.Shortmessage;
				((DbContext)context).Update<AccountClass>(accountClass);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 201);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 204);
				ret.AddReturnedValue(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 205);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveNotRelatedAccountsAsync(List<Tuple<Guid, List<Guid>>> data, string tranName, CancellationToken token)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 213));
		using (new JaegerMonitor("RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (Tuple<Guid, List<Guid>> tafsil in data)
				{
					token.ThrowIfCancellationRequested();
					List<AccountClass> list = await EntityFrameworkQueryableExtensions.ToListAsync<AccountClass>(((IQueryable<AccountClass>)context.Accounts).Where((AccountClass p) => p.TafsilGuid == tafsil.Item1), token);
					if (list != null && list.Any())
					{
						List<AccountClass> list2 = list.Where(delegate(AccountClass p)
						{
							List<Guid> item = tafsil.Item2;
							return item == null || !item.Any() || !tafsil.Item2.Contains(p.MoeinGuid);
						}).ToList();
						if (list2 != null && list2.Any())
						{
							context.Accounts.RemoveRange((IEnumerable<AccountClass>)list2);
						}
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 232);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 235);
				ret.AddReturnedValue(ex, "", "RemoveNotRelatedAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 236);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetAccountAsync(AccountBusiness item, short tryCount, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 247));
				AccountClass accountClass = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				accountClass.Status = true;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(accountClass, accountClass.MoeinGuid, accountClass.TafsilGuid), ReturnedState.None, "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 250);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 254);
				ret.AddReturnedValue(ex, "", "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 255);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> BallanceAccountAsync(AccountBusiness item, short tryCount, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 266));
				AccountClass accountClass = await c.Accounts.FindAsync(new object[2] { item.MoeinGuid, item.TafsilGuid });
				if (accountClass == null)
				{
					accountClass = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				}
				else
				{
					accountClass.Account += item.Account;
				}
				accountClass.ServerStatus = ServerStatusForClient.None;
				accountClass.Status = true;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(accountClass, accountClass.MoeinGuid, accountClass.TafsilGuid), ReturnedState.None, "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 274);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 278);
				ret.AddReturnedValue(ex, "", "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 279);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<byte[]?>> GetBallanceAsync(Guid moeinGuid, Guid tafsilGuid, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("GetBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfoWithValue<byte[]?> ret = new ReturnedSaveFuncInfoWithValue<byte[]>();
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 287));
			try
			{
				_ = 1;
				try
				{
					ret.value = (await c.AccountBallance.FindAsync(new object[2] { moeinGuid, tafsilGuid }))?.GardeshHesab;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 295);
					ret.AddReturnedValue(ex, "", "GetBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 296);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs");
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveBallanceAsync(Guid moeinGuid, Guid tafsilGuid, byte[]? gardesh, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("SaveBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 312));
				AccountBallance accountBallance = await c.AccountBallance.FindAsync(new object[2] { moeinGuid, tafsilGuid });
				if (accountBallance == null)
				{
					accountBallance = new AccountBallance
					{
						MoeinGuid = moeinGuid,
						TafsilGuid = tafsilGuid
					};
				}
				accountBallance.GardeshHesab = gardesh;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(accountBallance, moeinGuid, tafsilGuid), ReturnedState.None, "SaveBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 322);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 326);
				ret.AddReturnedValue(ex, "", "SaveBallanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 327);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<WebAccount> accounts, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 338));
				token.ThrowIfCancellationRequested();
				List<Guid> moeinGuids = ((IQueryable<Moein>)c.Moein).Select((Moein p) => p.Guid).ToList() ?? new List<Guid>();
				token.ThrowIfCancellationRequested();
				List<Guid> TafsilGuids = ((IQueryable<Tafsil>)c.Tafsil).Select((Tafsil p) => p.Guid).ToList() ?? new List<Guid>();
				token.ThrowIfCancellationRequested();
				accounts = accounts.Where((WebAccount p) => moeinGuids.Contains(p.MoeinGuid) && TafsilGuids.Contains(p.TafsilGuid)).ToList();
				foreach (WebAccount account in accounts)
				{
					AccountClass accountClass = await Mapper.MapAsync(ContextGuid, account, null, tranName, token);
					token.ThrowIfCancellationRequested();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(accountClass, accountClass.MoeinGuid, accountClass.TafsilGuid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 350);
					if (account.GardeshHesab?.Any() ?? false)
					{
						AccountBallance item = new AccountBallance
						{
							MoeinGuid = account.MoeinGuid,
							TafsilGuid = account.TafsilGuid,
							GardeshHesab = account.GardeshHesab.ToArray()
						};
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, account.MoeinGuid, account.TafsilGuid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 360);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 364);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 367);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 368);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid tafsilGuid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 379))).Database, "Update Acc_Accounts Set ServerStatus = {0} ,modified=getdate() Where TafsilGuid={1} And ServerStatus<>{0}", (IEnumerable<object>)new List<object>
				{
					(short)status,
					tafsilGuid
				}, token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 385);
				ret.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 386);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<AccountBusiness>> GetAllAsync(Guid? moeinGuid, Guid? tafsilGuid, bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 392));
		IEnumerable<AccountBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<AccountClass> queryable = (IQueryable<AccountClass>)c.Accounts;
				if ((moeinGuid ?? Guid.Empty) != Guid.Empty)
				{
					queryable = queryable?.Where((AccountClass q) => q.MoeinGuid == ((Guid?)moeinGuid).Value);
				}
				if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
				{
					queryable = queryable?.Where((AccountClass q) => q.TafsilGuid == ((Guid?)tafsilGuid).Value);
				}
				if (status.HasValue)
				{
					queryable = queryable?.Where((AccountClass q) => q.Status == ((bool?)status).Value);
				}
				result = ((!token.IsCancellationRequested && !IsDisposed && queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 406);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<AccountBusiness>> GetAllAsync(List<Guid> tafsilGuidList, bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 417));
		IEnumerable<AccountBusiness> result;
		try
		{
			_ = 1;
			try
			{
				if (!(tafsilGuidList?.Any() ?? false))
				{
					result = null;
				}
				else
				{
					IQueryable<AccountClass> queryable = (IQueryable<AccountClass>)c.Accounts;
					if (status.HasValue)
					{
						queryable = queryable?.Where((AccountClass q) => q.Status == ((bool?)status).Value);
					}
					queryable = queryable?.Where((AccountClass q) => tafsilGuidList.Contains(q.TafsilGuid));
					result = ((!token.IsCancellationRequested && !IsDisposed && queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs", 431);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\AccountPersistenceRepository.cs");
			}
		}
		return result;
	}
}
