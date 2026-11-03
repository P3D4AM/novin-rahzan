using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.TafsilPersistanceRepo;

public abstract class TafsilPersistenceRepositoryBase : PersistenceSyncRepository<TafsilBusiness, Tafsil>, ITafsilRepository, ISyncRepository<TafsilBusiness>, IEditableDelivery, IRepository<TafsilBusiness>, INoGuidRepository<TafsilBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TafsilBusiness, Tafsil>, IItemAsyncMapper<Tafsil, TafsilBusiness>, IItemMapper<Tafsil, TafsilBusiness>
	{
		public Task<Tafsil> MapAsync(string contextGuid, TafsilBusiness source, Tafsil destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Tafsil();
				}
				destination.Guid = source.Guid;
				destination.Name = source.Name;
				destination.Code = source.Code;
				destination.Status = source.Status;
				destination.Description = source.Description ?? "";
				destination.System = source.System;
				destination.TafsilGroupGuid = source.TafsilGroupGuid;
				destination.HType = source.HType;
				destination.DateM = source.DateM;
				destination.Modified = source.Modified;
				destination.SerializedData = source.SerializedData;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.SumAghsat = source.SumAghsat;
				destination.Version = source.Version;
				destination.IsSpecial = source.IsSpecial;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 711);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<TafsilBusiness> MapAsync(string contextGuid, Tafsil source, TafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public TafsilBusiness Map(string contextGuid, Tafsil source, TafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException("این کد نباید فراخوانی شود.");
		}

		public Task<List<Tafsil>> MapListAsync(string contextGuid, IEnumerable<TafsilBusiness> source, List<Tafsil> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<Tafsil> source, List<TafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	internal class TafsilBusinessWithEmptyConstructor : TafsilBusiness
	{
		public TafsilBusinessWithEmptyConstructor()
			: base(Guid.Empty.ToString())
		{
		}
	}

	private class clsAccount
	{
		public Guid Guid { get; set; }

		public decimal Account { get; set; }
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Tafsil, TafsilBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TafsilBusiness, Tafsil> MapToDatabase => Mapper;

	public override IItemMapper<Tafsil, TafsilBusiness> MapFromDatabaseNoneAsync => Mapper;

	public TafsilPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public virtual async Task<TafsilBusiness> GetAsync(Guid hesabGuid, HesabType hesabtype, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 36));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			TafsilBusiness result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TafsilBusinessWithEmptyConstructor>(from p in (IQueryable<Tafsil>)c.Tafsil
						where p.Guid == hesabGuid && p.HType == hesabtype
						select p into u
						select new TafsilBusinessWithEmptyConstructor
						{
							ChangeStatus = EntityChangeSt.Edited,
							ContextGuid = ContextGuid,
							Status = u.Status,
							Guid = u.Guid,
							DeliveryMessage = u.DeliveryMessage,
							DeliveryShortMessage = u.DeliveryShortMessage,
							Code = u.Code,
							DateM = u.DateM,
							DeliveryDate = u.DeliveryDate,
							DeliveryId = u.DeliveryId,
							Description = u.Description,
							HType = u.HType,
							HostRefrenced = u.HostRefrenced,
							Modified = u.Modified,
							Name = u.Name,
							SerializedData = u.SerializedData,
							ServerStatus = u.ServerStatus,
							System = u.System,
							TafsilGroupGuid = u.TafsilGroupGuid,
							Account = u.Accounts.Sum((AccountClass q) => q.Account),
							IsSpecial = u.IsSpecial,
							Version = u.Version
						}, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 73);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public override TafsilBusiness Get(Guid hesabGuid, CancellationToken token, string tranName)
	{
		if (ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar)
		{
			throw new NotImplementedException();
		}
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)dbContext.GetContext(tranName, 60000, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 85);
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			try
			{
				return (from p in (IQueryable<Tafsil>)acc_ModelContext.Tafsil
					where p.Guid == hesabGuid
					select p into u
					select new TafsilBusinessWithEmptyConstructor
					{
						ChangeStatus = EntityChangeSt.Edited,
						ContextGuid = ContextGuid,
						Status = u.Status,
						Guid = u.Guid,
						DeliveryMessage = u.DeliveryMessage,
						DeliveryShortMessage = u.DeliveryShortMessage,
						Code = u.Code,
						DateM = u.DateM,
						DeliveryDate = u.DeliveryDate,
						DeliveryId = u.DeliveryId,
						Description = u.Description,
						HType = u.HType,
						HostRefrenced = u.HostRefrenced,
						Modified = u.Modified,
						Name = u.Name,
						SerializedData = u.SerializedData,
						ServerStatus = u.ServerStatus,
						System = u.System,
						TafsilGroupGuid = u.TafsilGroupGuid,
						Account = u.Accounts.Sum((AccountClass q) => q.Account),
						IsSpecial = u.IsSpecial,
						Version = u.Version
					}).FirstOrDefault();
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
				WebErrorLog.Instance.StartLog(ex2, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 120);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					acc_ModelContext.Dispose("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
				}
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 135))).Database, "Update Acc_Tafsil    Set ServerStatus = {0}    ,modified=getdate()    Where Guid={1} And    ServerStatus<>{0}", (IEnumerable<object>)new List<object>
				{
					(short)status,
					guid
				}, token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 145);
				ret.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 146);
			}
			return ret;
		}
	}

	public override async Task<TafsilBusiness> GetAsync(Guid hesabGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 152));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			TafsilBusiness result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TafsilBusinessWithEmptyConstructor>(from p in (IQueryable<Tafsil>)c.Tafsil
						where p.Guid == hesabGuid
						select p into u
						select new TafsilBusinessWithEmptyConstructor
						{
							ChangeStatus = EntityChangeSt.Edited,
							ContextGuid = ContextGuid,
							Status = u.Status,
							Guid = u.Guid,
							DeliveryMessage = u.DeliveryMessage,
							DeliveryShortMessage = u.DeliveryShortMessage,
							Code = u.Code,
							DateM = u.DateM,
							DeliveryDate = u.DeliveryDate,
							DeliveryId = u.DeliveryId,
							Description = u.Description,
							HType = u.HType,
							HostRefrenced = u.HostRefrenced,
							Modified = u.Modified,
							Name = u.Name,
							SerializedData = u.SerializedData,
							ServerStatus = u.ServerStatus,
							System = u.System,
							TafsilGroupGuid = u.TafsilGroupGuid,
							Account = u.Accounts.Sum((AccountClass q) => q.Account),
							IsSpecial = u.IsSpecial,
							Version = u.Version
						}, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 188);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public override Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("Should Not Remove Data!!!");
	}

	public override Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("Should Not Delete Range Data!!!");
	}

	public virtual async Task<TafsilBusiness> GetAsync(long tafsilCode, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 201));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			TafsilBusiness result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TafsilBusinessWithEmptyConstructor>(from p in (IQueryable<Tafsil>)c.Tafsil
						where p.Code == tafsilCode
						select p into u
						select new TafsilBusinessWithEmptyConstructor
						{
							ContextGuid = ContextGuid,
							ChangeStatus = EntityChangeSt.Edited,
							Status = u.Status,
							Guid = u.Guid,
							DeliveryMessage = u.DeliveryMessage,
							DeliveryShortMessage = u.DeliveryShortMessage,
							Code = u.Code,
							DateM = u.DateM,
							DeliveryDate = u.DeliveryDate,
							DeliveryId = u.DeliveryId,
							Description = u.Description,
							HType = u.HType,
							HostRefrenced = u.HostRefrenced,
							Modified = u.Modified,
							Name = u.Name,
							SerializedData = u.SerializedData,
							ServerStatus = u.ServerStatus,
							System = u.System,
							TafsilGroupGuid = u.TafsilGroupGuid,
							Account = u.Accounts.Sum((AccountClass q) => q.Account),
							IsSpecial = u.IsSpecial,
							Version = u.Version
						}, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 235);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public Task<string> NextTafsilSayerAsync(Guid ParentGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(TafsilBusiness tafsil, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("SaveAsync - please fill parameter updateAccount!");
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(TafsilBusiness tafsil, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 257));
				Tafsil tafsil2 = await MapToDatabase.MapAsync(ContextGuid, tafsil, null, tranName, token);
				Tafsil tafsil3 = ((DbContext)c).Find<Tafsil>(new object[1] { tafsil.Guid });
				if (tafsil3 == null)
				{
					tafsil2.Modified = DateTime.Now;
					((DbContext)c).Add<Tafsil>(tafsil2);
				}
				else
				{
					((EntityEntry)((DbContext)c).Entry<Tafsil>(tafsil3)).CurrentValues.SetValues((object)tafsil2);
					tafsil3.Modified = DateTime.Now;
					((DbContext)c).Update<Tafsil>(tafsil3);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 275);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 276);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TafsilBusiness> lstHesabsListTafsil, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				if (lstHesabsListTafsil != null && lstHesabsListTafsil.Any())
				{
					foreach (TafsilBusiness item in lstHesabsListTafsil)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 289);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 293);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 294);
			}
			return ret;
		}
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateAccountAsync(Guid guid, decimal accountChange, short tryCount, CancellationToken token, string tranName)
	{
		throw new Exception("UpdateAccountAsync");
	}

	public async Task<ReturnedSaveFuncInfo> UpdateSumAghsatAsync(Guid guid, decimal price, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 306));
				Tafsil tafsil = await c.Tafsil.FindAsync(new object[1] { guid });
				if (tafsil != null)
				{
					tafsil.SumAghsat -= price;
					((DbContext)c).Update<Tafsil>(tafsil);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 316);
				ret.AddReturnedValue(ex, "", "UpdateSumAghsatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 317);
			}
			return ret;
		}
	}

	public async Task<TafsilBusiness> GetByTypeAndCodeAsync(HesabType hesabType, long code, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByTypeAndCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 323));
		using (new JaegerMonitor("GetByTypeAndCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			TafsilBusiness result;
			try
			{
				result = (from p in (IQueryable<Tafsil>)acc_ModelContext.Tafsil
					where p.HType == hesabType && p.Code == code
					select p into u
					select new TafsilBusinessWithEmptyConstructor
					{
						Status = u.Status,
						Guid = u.Guid,
						DeliveryMessage = u.DeliveryMessage,
						DeliveryShortMessage = u.DeliveryShortMessage,
						Code = u.Code,
						DateM = u.DateM,
						DeliveryDate = u.DeliveryDate,
						DeliveryId = u.DeliveryId,
						Description = u.Description,
						HType = u.HType,
						HostRefrenced = u.HostRefrenced,
						Modified = u.Modified,
						Name = u.Name,
						SerializedData = u.SerializedData,
						ServerStatus = u.ServerStatus,
						System = u.System,
						TafsilGroupGuid = u.TafsilGroupGuid,
						Account = u.Accounts.Sum((AccountClass q) => q.Account),
						IsSpecial = u.IsSpecial,
						Version = u.Version
					}).SingleOrDefault();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetByTypeAndCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 355);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GetByTypeAndCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<long>> NextCodeInGroupAsync(Guid tafsilLevelGuid, EnSuggestionType suggestionMode, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 367));
		int line = 1;
		using (new JaegerMonitor("NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfoWithValue<long> res = new ReturnedSaveFuncInfoWithValue<long>();
			ReturnedSaveFuncInfoWithValue<long> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					TafsilGroup tafsilLevel = await c.TafsilLevel.FindAsync(new object[1] { tafsilLevelGuid });
					switch (suggestionMode)
					{
					case EnSuggestionType.FillGaps:
					{
						List<long> list = await EntityFrameworkQueryableExtensions.ToListAsync<long>(((IQueryable<Tafsil>)c.Tafsil).Select((Tafsil q) => q.Code), token);
						long num3 = tafsilLevel.CodeMin;
						while (true)
						{
							if (num3 < tafsilLevel.CodeMax)
							{
								if (!list.Contains(num3))
								{
									res.value = num3;
									result = res;
									break;
								}
								num3++;
								continue;
							}
							line = 4;
							res.AddError(EnSystem.None, "رنج کدهای گروه تفصیلی".GetTranslate() + $"({tafsilLevel.Name})({tafsilLevel.CodeMin}-{tafsilLevel.CodeMax})" + "پر شده است.".GetTranslate(), "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 384);
							line = 5;
							res.value = tafsilLevel.CodeMax;
							line = 6;
							result = res;
							break;
						}
						break;
					}
					case EnSuggestionType.NextBiggest:
					{
						IQueryable<Tafsil> queryable = ((IQueryable<Tafsil>)c.Tafsil).Where((Tafsil p) => p.TafsilGroupGuid == tafsilLevelGuid);
						object obj;
						if (queryable == null)
						{
							obj = null;
						}
						else
						{
							IQueryable<Tafsil> queryable2 = queryable.OrderByDescending((Tafsil x) => x.Code).Take(1);
							obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Tafsil>(queryable2, token) : null);
						}
						long num2 = (await (Task<Tafsil>)obj)?.Code ?? 0;
						num2++;
						if (num2 > tafsilLevel.CodeMax)
						{
							result = await NextCodeInGroupAsync(tafsilLevelGuid, EnSuggestionType.FillGaps, token, tranName);
							break;
						}
						res.value = num2;
						goto end_IL_0132;
					}
					default:
						throw new NotImplementedException();
					}
					goto IL_069e;
					end_IL_0132:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $" The Error Line Is :{line}", "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 403);
					res.AddReturnedValue(ex, "", "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 404);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
				}
			}
			return res;
			IL_069e:
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 422);
		}
	}

	public async Task<ConcurrentDictionary<Guid, decimal>> GetAccountsAsync(CancellationToken token, string tranName)
	{
		ConcurrentDictionary<Guid, decimal> ret = new ConcurrentDictionary<Guid, decimal>();
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 427));
		using (new JaegerMonitor("GetAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			try
			{
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GetAccountsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
				}
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TafsilBusiness> lstHesabsListTafsil, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("SaveRangeAsync - please fill parameter updateAccount!");
	}

	public async Task<TafsilBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 453));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			TafsilBusiness result;
			try
			{
				result = (from p in (IQueryable<Tafsil>)acc_ModelContext.Tafsil
					where p.Name == name
					select p into u
					select new TafsilBusinessWithEmptyConstructor
					{
						Status = u.Status,
						Guid = u.Guid,
						DeliveryMessage = u.DeliveryMessage,
						DeliveryShortMessage = u.DeliveryShortMessage,
						Code = u.Code,
						DateM = u.DateM,
						DeliveryDate = u.DeliveryDate,
						DeliveryId = u.DeliveryId,
						Description = u.Description,
						HType = u.HType,
						HostRefrenced = u.HostRefrenced,
						Modified = u.Modified,
						Name = u.Name,
						SerializedData = u.SerializedData,
						ServerStatus = u.ServerStatus,
						System = u.System,
						TafsilGroupGuid = u.TafsilGroupGuid,
						IsSpecial = u.IsSpecial,
						Account = u.Accounts.Sum((AccountClass q) => q.Account),
						Version = u.Version
					}).SingleOrDefault();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 486);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public virtual async Task<IEnumerable<TafsilBusiness>> GetAllAsync(EnSystem system, TafsilFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 497));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			IEnumerable<TafsilBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					AccGlobalSettings setting = await dbContext.GetLocalSettingAsync(ContextGuid);
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						if (!(filter?.MoeinGuidList?.Any()).GetValueOrDefault())
						{
							if (filter == null)
							{
								filter = new TafsilFilter
								{
									Status = null
								};
							}
							filter.MoeinGuidList = new List<Guid>();
						}
						IQueryable<TafsilBusinessWithEmptyConstructor> query2 = ((IQueryable<Tafsil>)c.Tafsil).Select((Tafsil u) => new TafsilBusinessWithEmptyConstructor
						{
							ChangeStatus = EntityChangeSt.Edited,
							ContextGuid = ContextGuid,
							Status = u.Status,
							Guid = u.Guid,
							DeliveryMessage = u.DeliveryMessage,
							DeliveryShortMessage = u.DeliveryShortMessage,
							Code = u.Code,
							DateM = u.DateM,
							DeliveryDate = u.DeliveryDate,
							DeliveryId = u.DeliveryId,
							Description = u.Description,
							HType = u.HType,
							HostRefrenced = u.HostRefrenced,
							Modified = u.Modified,
							Name = u.Name,
							SerializedData = u.SerializedData,
							ServerStatus = u.ServerStatus,
							System = u.System,
							TafsilGroupGuid = u.TafsilGroupGuid,
							Account = (filter.MoeinGuidList.Any() ? u.Accounts.Where((AccountClass q) => filter.MoeinGuidList.Contains(q.MoeinGuid)).Sum((AccountClass w) => w.Account) : u.Accounts.Sum((AccountClass w) => w.Account)),
							IsSpecial = u.IsSpecial,
							Version = u.Version
						});
						if (token.IsCancellationRequested)
						{
							result = null;
						}
						else
						{
							if (filter != null)
							{
								if (filter.RelatedMoeinGuid.HasValue && filter.RelatedMoeinGuid != Guid.Empty && !(filter.TafsilGroupGuids?.Any() ?? false))
								{
									IEnumerable<MoeinTafsilGroupRelationsBusiness> enumerable = await MoeinTafsilGroupRelationsBusiness.GetAllAsync(ContextGuid, filter.RelatedMoeinGuid, null, token, tranName);
									if (enumerable != null && enumerable.Any())
									{
										filter.TafsilGroupGuids = enumerable.Select((MoeinTafsilGroupRelationsBusiness q) => q.TafsilGroupGuid);
									}
								}
								if (setting != null && filter.HesabType.GetValueOrDefault() != HesabType.Bank && filter.HesabType.GetValueOrDefault() != HesabType.Sandogh)
								{
									if (setting.Sanads.MoeinNotGroupTafsil)
									{
										if ((filter.TafsilGroupGuids?.Any() ?? false) || filter.TafsilLevel != 0)
										{
											query2 = query2.Where((TafsilBusinessWithEmptyConstructor p) => filter.TafsilGroupGuids.Contains(p.TafsilGroupGuid));
										}
									}
									else if ((filter.TafsilGroupGuids?.Any() ?? false) || filter.TafsilLevel != 0)
									{
										query2 = query2.Where((TafsilBusinessWithEmptyConstructor p) => filter.TafsilGroupGuids.Contains(p.TafsilGroupGuid));
									}
								}
								if (filter.HesabType.GetValueOrDefault() != 0)
								{
									query2 = query2.Where((TafsilBusinessWithEmptyConstructor p) => (int?)p.HType == (int?)filter.HesabType);
								}
								if (!string.IsNullOrEmpty(filter.NameSearchString))
								{
									if (!filter.NameSearchLocation.HasValue || filter.NameSearchLocation == SearchLocation.All)
									{
										foreach (string word2 in filter.NameSearchString.RemoveCharactersForLinkSearch()?.FixString()?.SplitString()?.Distinct() ?? new List<string>())
										{
											if (!string.IsNullOrEmpty(word2))
											{
												query2 = query2.Where((TafsilBusinessWithEmptyConstructor p) => DbFunctionsExtensions.Like(EF.Functions, p.Name, $"%{word2}%"));
											}
										}
									}
									else if (filter.NameSearchLocation.GetValueOrDefault() == SearchLocation.Start)
									{
										query2 = query2.Where((TafsilBusinessWithEmptyConstructor q) => q.Name.StartsWith(filter.NameSearchString));
									}
									else if (filter.NameSearchLocation.GetValueOrDefault() == SearchLocation.End)
									{
										query2 = query2.Where((TafsilBusinessWithEmptyConstructor q) => q.Name.EndsWith(filter.NameSearchString));
									}
									else if (filter.NameSearchLocation.GetValueOrDefault() == SearchLocation.Equal)
									{
										query2 = query2.Where((TafsilBusinessWithEmptyConstructor q) => q.Name == filter.NameSearchString);
									}
								}
								if (!string.IsNullOrEmpty(filter.CodeSearchString))
								{
									if (!filter.CodeSearchLocation.HasValue || filter.CodeSearchLocation == SearchLocation.All)
									{
										foreach (string word in filter.CodeSearchString.RemoveCharactersForLinkSearch()?.FixString()?.SplitString()?.Distinct() ?? new List<string>())
										{
											if (!string.IsNullOrEmpty(word))
											{
												query2 = query2?.Where((TafsilBusinessWithEmptyConstructor p) => DbFunctionsExtensions.Like(EF.Functions, p.Code.ToString(), $"%{word}%"));
											}
										}
									}
									else if (filter.CodeSearchLocation.GetValueOrDefault() == SearchLocation.Start)
									{
										query2 = query2.Where((TafsilBusinessWithEmptyConstructor q) => q.Code.ToString().StartsWith(filter.CodeSearchString));
									}
									else if (filter.CodeSearchLocation.GetValueOrDefault() == SearchLocation.End)
									{
										query2 = query2.Where((TafsilBusinessWithEmptyConstructor q) => q.Code.ToString().EndsWith(filter.CodeSearchString));
									}
									else if (filter.CodeSearchLocation.GetValueOrDefault() == SearchLocation.Equal)
									{
										query2 = query2.Where((TafsilBusinessWithEmptyConstructor q) => q.Code == filter.CodeSearchString.ParseToLong(0L));
									}
								}
								if ((filter.TafsilGroupGuids?.Any() ?? false) || filter.TafsilLevel != 0)
								{
									query2 = query2.Where((TafsilBusinessWithEmptyConstructor p) => filter.TafsilGroupGuids.Contains(p.TafsilGroupGuid));
								}
								if ((filter.TafsilGroupGuid ?? Guid.Empty) != Guid.Empty)
								{
									query2 = query2.Where((TafsilBusinessWithEmptyConstructor q) => q.TafsilGroupGuid == filter.TafsilGroupGuid);
								}
								if (filter.StartCode.HasValue)
								{
									query2 = query2.Where((TafsilBusinessWithEmptyConstructor p) => (long?)p.Code >= filter.StartCode);
								}
								if (filter.EndCode.HasValue)
								{
									query2 = query2.Where((TafsilBusinessWithEmptyConstructor p) => (long?)p.Code <= filter.EndCode);
								}
								if (filter.Status.HasValue)
								{
									query2 = query2.Where((TafsilBusinessWithEmptyConstructor p) => p.Status == filter.Status.Value);
								}
							}
							query2 = from x in query2
								orderby x.Name, x.Code
								select x;
							result = ((!token.IsCancellationRequested) ? (await EntityFrameworkQueryableExtensions.ToListAsync<TafsilBusinessWithEmptyConstructor>(query2, token)) : null);
						}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 616);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TafsilBusiness> list, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 630);
			try
			{
				if (!(list?.Any() ?? false))
				{
					return ret;
				}
				foreach (Tafsil item in await MapToDatabase.MapListAsync(ContextGuid, list, null, tranName, token))
				{
					token.ThrowIfCancellationRequested();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, item.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 638);
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 644);
				ret.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 645);
			}
			return ret;
		}
	}

	public async Task<List<Guid>> CheckExistAsync(List<Guid> lst, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 651));
		List<Guid> result;
		try
		{
			_ = 1;
			try
			{
				if (!(lst?.Any() ?? false))
				{
					result = null;
				}
				else
				{
					List<Tafsil> list = await EntityFrameworkQueryableExtensions.ToListAsync<Tafsil>(((IQueryable<Tafsil>)c.Tafsil).Where((Tafsil q) => lst.Contains(q.Guid)), token);
					result = ((list != null && list.Any()) ? list.Select((Tafsil q) => q.Guid).ToList() : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "CheckExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs", 663);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("CheckExistAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}
}
