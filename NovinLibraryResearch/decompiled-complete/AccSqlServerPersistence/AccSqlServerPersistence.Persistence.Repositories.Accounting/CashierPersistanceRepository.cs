using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting.UserSettings;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class CashierPersistanceRepository : PersistenceSyncRepository<CashierBusiness, Cashier>, ICashierRepository, ISyncRepository<CashierBusiness>, IEditableDelivery, IRepository<CashierBusiness>, INoGuidRepository<CashierBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CashierBusiness, Cashier>, IItemAsyncMapper<Cashier, CashierBusiness>
	{
		public async Task<Cashier> MapAsync(string contextGuid, CashierBusiness source, Cashier destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Cashier();
				}
				destination.Guid = source.Guid;
				destination.UserGuid = source.UserGuid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.UserName = source.UserName;
				destination.HashedPassword = source.HashedPassword;
				destination.Description = source.Description ?? "";
				destination.DateM = source.DateM;
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.Setting = source.Settings.ToString();
				destination.LoginAccess = source.LoginAccess;
				destination.BankGuid1 = (((source.BankGuid1 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid1);
				destination.BankGuid2 = (((source.BankGuid2 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid2);
				destination.BankGuid3 = (((source.BankGuid3 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid3);
				destination.BankGuid4 = (((source.BankGuid4 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid4);
				Cashier cashier = destination;
				Guid? cusGuid = ((!(((await source.GetCusGuidAsync(EnSystem.Mapper)) ?? Guid.Empty) == Guid.Empty)) ? (await source.GetCusGuidAsync(EnSystem.Mapper)) : null);
				cashier.CusGuid = cusGuid;
				destination.SandoghGuid = (((source.SandoghGuid ?? Guid.Empty) == Guid.Empty) ? null : source.SandoghGuid);
				destination.DepotGuid = (((source.DepotGuid ?? Guid.Empty) == Guid.Empty) ? null : source.DepotGuid);
				destination.AllVisitorAccess = source.AllVisitorAccess;
				destination.LoginAccess = source.LoginAccess;
				destination.AllSandoghsAccess = source.AllSandoghsAccess;
				destination.AllDepotsAccess = source.AllDepotsAccess;
				destination.AllBanksAccess = source.AllBanksAccess;
				destination.VisitorGuid = (((source.VisitorGuid ?? Guid.Empty) == Guid.Empty) ? null : source.VisitorGuid);
				destination.DefaultCusGroup = (((source.DefaultCusGroup ?? Guid.Empty) == Guid.Empty) ? null : source.DefaultCusGroup);
				destination.LoginAccess = source.LoginAccess;
				destination.ReceptionSmsCustomerGuid = source.ReceptionSmsCustomerGuid;
				destination.PardakhtSmsCustomerGuid = source.PardakhtSmsCustomerGuid;
				destination.StrCashierTime = Json.ToStringJson(source.CashierTime);
				destination.FareMoeinGuid = ((source.FareMoeinGuid == Guid.Empty) ? ParentDefaults.MoeinCoding.CLSMoein31 : source.FareMoeinGuid);
				destination.FareTafsilGuid = (((source.FareTafsilGuid ?? Guid.Empty) == Guid.Empty) ? null : source.FareTafsilGuid);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 245);
				throw;
			}
			return destination;
		}

		public Task<CashierBusiness> MapAsync(string contextGuid, Cashier source, CashierBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CashierBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.UserGuid = source.UserGuid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.UserName = source.UserName;
				destination.HashedPassword = source.HashedPassword;
				destination.Description = source.Description;
				destination.DateM = source.DateM;
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				if (string.IsNullOrEmpty(source.Setting))
				{
					source.Setting = Json.ToStringJson(new UserSettings());
				}
				destination.SetSetting(source.Setting);
				destination.BankGuid1 = (((source.BankGuid1 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid1);
				destination.BankGuid2 = (((source.BankGuid2 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid2);
				destination.BankGuid3 = (((source.BankGuid3 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid3);
				destination.BankGuid4 = (((source.BankGuid4 ?? Guid.Empty) == Guid.Empty) ? null : source.BankGuid4);
				destination.SetCusGuid(((source.CusGuid ?? Guid.Empty) == Guid.Empty) ? null : source.CusGuid);
				destination.SandoghGuid = (((source.SandoghGuid ?? Guid.Empty) == Guid.Empty) ? null : source.SandoghGuid);
				destination.DepotGuid = (((source.DepotGuid ?? Guid.Empty) == Guid.Empty) ? null : source.DepotGuid);
				destination.AllVisitorAccess = source.AllVisitorAccess;
				destination.LoginAccess = source.LoginAccess;
				destination.AllSandoghsAccess = source.AllSandoghsAccess;
				destination.AllDepotsAccess = source.AllDepotsAccess;
				destination.AllBanksAccess = source.AllBanksAccess;
				destination.VisitorGuid = (((source.VisitorGuid ?? Guid.Empty) == Guid.Empty) ? null : source.VisitorGuid);
				destination.DefaultCusGroup = (((source.DefaultCusGroup ?? Guid.Empty) == Guid.Empty) ? null : source.DefaultCusGroup);
				destination.CashierTime = source.StrCashierTime.FromJson<List<CashierTime>>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 292);
				destination.ReceptionSmsCustomerGuid = source.ReceptionSmsCustomerGuid;
				destination.PardakhtSmsCustomerGuid = source.PardakhtSmsCustomerGuid;
				destination.FareMoeinGuid = source.FareMoeinGuid;
				destination.FareTafsilGuid = (((source.FareTafsilGuid ?? Guid.Empty) == Guid.Empty) ? null : source.FareTafsilGuid);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 303);
				throw;
			}
		}

		public Task<List<Cashier>> MapListAsync(string contextGuid, IEnumerable<CashierBusiness> source, List<Cashier> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CashierBusiness>> MapListAsync(string contextGuid, IEnumerable<Cashier> source, List<CashierBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Cashier, CashierBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CashierBusiness, Cashier> MapToDatabase => Mapper;

	public override IItemMapper<Cashier, CashierBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public CashierPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override CashierBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> CheckUserNameAsync(string uName, Guid _guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 40));
			bool result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Cashier>((IQueryable<Cashier>)c.Cashiers, (Expression<Func<Cashier, bool>>)((Cashier x) => x.UserName == uName && x.Guid != _guid && x.Status), token) == null;
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
					WebErrorLog.Instance.StartLog(ex2, "", "CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 50);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CashierBusiness> GetAsync(string uName, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 62));
			CashierBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					uName = uName?.FixString()?.ToLower() ?? "";
					Cashier source = (await EntityFrameworkQueryableExtensions.ToListAsync<Cashier>((IQueryable<Cashier>)c.Cashiers, token))?.FirstOrDefault((Cashier x) => x.UserName.Trim().ToLower().FixString() == uName && x.Status);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 76);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CashierBusiness> GetByNameAsync(string name, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 88));
			CashierBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Cashier source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Cashier>((IQueryable<Cashier>)c.Cashiers, (Expression<Func<Cashier, bool>>)((Cashier x) => x.Name == name), token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 100);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CashierBusiness cashier, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				if (cashier == null)
				{
					throw new NullReferenceException("cashier is null!!");
				}
				if (cashier.Settings == null)
				{
					throw new NullReferenceException("cashier settings is null!!");
				}
				if (string.IsNullOrEmpty(Json.ToStringJson(cashier.Settings)))
				{
					throw new NullReferenceException("cashier settings string is null or empty!!");
				}
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 121));
				Cashier cashier2 = await MapToDatabase.MapAsync(ContextGuid, cashier, new Cashier(), tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(cashier2, cashier2.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 123);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 127);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 128);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<CashierBusiness>> GetAllAsync(Guid userGuid, bool? status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 135));
			IEnumerable<CashierBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Cashier> queryable = ((IQueryable<Cashier>)c.Cashiers).Where((Cashier x) => x.UserGuid == userGuid);
					if (status.HasValue)
					{
						queryable = queryable.Where((Cashier q) => (bool?)q.Status == status);
					}
					IItemAsyncMapper<Cashier, CashierBusiness> mapFromDatabase = MapFromDatabase;
					string contextGuid = ContextGuid;
					result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<Cashier>(queryable, token), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 146);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CashierBusiness> GetByUsernameAsync(string username, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetByUsernameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByUsernameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 158));
			CashierBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Cashier source = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Cashier>((IQueryable<Cashier>)c.Cashiers, (Expression<Func<Cashier, bool>>)((Cashier x) => x.UserName == username), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "UserName:" + username, "GetByUsernameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 168);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByUsernameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\CashierPersistanceRepository.cs", 187);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
