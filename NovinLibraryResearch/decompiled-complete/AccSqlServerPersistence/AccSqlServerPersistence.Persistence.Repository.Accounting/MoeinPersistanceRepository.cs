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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using Serilog;

namespace AccSqlServerPersistence.Persistence.Repository.Accounting;

internal class MoeinPersistanceRepository : PersistenceSyncRepository<MoeinBusiness, Moein>, IMoeinRepository, ISyncRepository<MoeinBusiness>, IEditableDelivery, IRepository<MoeinBusiness>, INoGuidRepository<MoeinBusiness>, IDisposable, IItemAsyncMapper<Moein, MoeinBusiness>
{
	private class mapper : IItemAsyncMapper<MoeinBusiness, Moein>, IItemAsyncMapper<Moein, MoeinBusiness>, IItemMapper<Moein, MoeinBusiness>, IItemAsyncMapper<WebMoein, Moein>
	{
		public Task<Moein> MapAsync(string contextGuid, MoeinBusiness source, Moein destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
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
						destination = new Moein();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.KolGuid = source.KolGuid;
					destination.Code = source.Code;
					destination.Status = source.Status;
					destination.Description = source.Description ?? "";
					destination.Mahiat = source.Mahiat;
					destination.System = source.System;
					destination.Peygiri = source.Peygiri;
					destination.Tedadi = source.Tedadi;
					destination.Account = source.Account;
					destination.DateM = source.DateM;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.GardeshHesab = source.GardeshHesab;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 530);
					throw;
				}
			}
		}

		public Task<MoeinBusiness> MapAsync(string contextGuid, Moein source, MoeinBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public MoeinBusiness Map(string contextGuid, Moein source, MoeinBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException("این کد نباید فراخوانی شود.");
		}

		public Task<List<Moein>> MapListAsync(string contextGuid, IEnumerable<MoeinBusiness> source, List<Moein> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<MoeinBusiness>> MapListAsync(string contextGuid, IEnumerable<Moein> source, List<MoeinBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<Moein> MapAsync(string contextGuid, WebMoein source, Moein destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
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
						destination = new Moein();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.KolGuid = source.KolGuid;
					destination.Code = source.Code;
					destination.Status = source.Status;
					destination.Description = source.Description ?? "";
					destination.Mahiat = source.Mahiat;
					destination.System = source.System;
					destination.Peygiri = source.Peygiri;
					destination.Tedadi = source.Tedadi;
					destination.Account = source.Account;
					destination.DateM = source.DateM;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.GardeshHesab = source.GardeshHesab;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 578);
					throw;
				}
			}
		}

		public Task<List<Moein>> MapListAsync(string contextGuid, IEnumerable<WebMoein> source, List<Moein> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Moein, MoeinBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<MoeinBusiness, Moein> MapToDatabase => Mapper;

	public override IItemMapper<Moein, MoeinBusiness> MapFromDatabaseNoneAsync => Mapper;

	public MoeinPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("Should Not Remove Data!!!");
	}

	public override Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("Should Not Delete Range Data!!!");
	}

	public async Task<byte[]> GetGardeshHesabAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetGardeshHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetGardeshHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 37));
			byte[] result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Moein>((IQueryable<Moein>)c.Moein, (Expression<Func<Moein, bool>>)((Moein x) => x.Guid == guid), token))?.GardeshHesab;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetGardeshHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 48);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetGardeshHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<MoeinBusiness>> GetAllBetweenCodesAsync(long? code1, long? code2, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllBetweenCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 59));
		using (new JaegerMonitor("GetAllBetweenCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			IEnumerable<MoeinBusiness> result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.ToListAsync<MoeinBusinessWithEmptyConstructor>(from p in (IQueryable<Moein>)c.Moein
						where ((code1 ?? 0) <= 0 || p.Code >= ((long?)code1).Value) && ((code2 ?? 0) <= 0 || p.Code <= ((long?)code2).Value)
						select p into u
						select new MoeinBusinessWithEmptyConstructor
						{
							ContextGuid = ContextGuid,
							ChangeStatus = EntityChangeSt.Edited,
							Status = u.Status,
							Guid = u.Guid,
							DeliveryMessage = u.DeliveryMessage,
							DeliveryShortMessage = u.DeliveryShortMessage,
							Account = u.Account,
							Code = u.Code,
							DateM = u.DateM,
							DeliveryDate = u.DeliveryDate,
							DeliveryId = u.DeliveryId,
							Description = u.Description,
							HostRefrenced = u.HostRefrenced,
							Modified = u.Modified,
							Name = u.Name,
							ServerStatus = u.ServerStatus,
							System = u.System,
							KolGuid = u.KolGuid,
							Mahiat = u.Mahiat,
							Peygiri = u.Peygiri,
							Tedadi = u.Tedadi
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllBetweenCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 94);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllBetweenCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveMoeinRelationTafsilLevelAsync(Guid moeinGuid, List<TafsilGroupBusiness> lst, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<MoeinBusiness> lstHesabsListMoein, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (!(lstHesabsListMoein?.Any() ?? false))
				{
					return ret;
				}
				foreach (MoeinBusiness item in lstHesabsListMoein)
				{
					if (item != null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 118);
						if (ret.HasError)
						{
							return ret;
						}
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 126);
				throw;
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<MoeinBusiness> lstHesabsListMoein, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("please fill parameter updateAccount!");
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(MoeinBusiness moein, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("please fill parameter updateAccount!");
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(MoeinBusiness moein, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 140));
				Moein newMoein = await MapToDatabase.MapAsync(ContextGuid, moein, null, tranName, token);
				Moein moein2 = await ((DbContext)c).FindAsync<Moein>(new object[1] { moein.Guid });
				if (moein2 == null)
				{
					moein2 = newMoein;
					if (!updateAccount)
					{
						moein2.Account = 0m;
					}
					((DbContext)c).Add<Moein>(newMoein);
				}
				else
				{
					if (moein2.KolGuid != moein.KolGuid)
					{
						throw new InvalidOperationException($"update kolGuid from ({moein2.KolGuid}) to ({moein.KolGuid}) is not valid!");
					}
					if (!updateAccount)
					{
						newMoein.Account = moein2.Account;
					}
					((EntityEntry)((DbContext)c).Entry<Moein>(moein2)).CurrentValues.SetValues((object)newMoein);
					((DbContext)c).Update<Moein>(moein2);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 160);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 161);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateAccountAsync(Guid guid, decimal p0, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 172));
				Moein item = await c.Moein.FindAsync(new object[1] { guid });
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (item != null)
				{
					item.Account += p0;
					Kol kol = await c.Kol.FindAsync(new object[1] { item.KolGuid });
					kol.Account += p0;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(kol, kol.Guid), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 179);
				}
				if (res.HasError)
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, guid), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 182);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 186);
				res.AddReturnedValue(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 187);
			}
			return res;
		}
	}

	public override async Task<MoeinBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 194));
			MoeinBusiness result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<MoeinBusinessWithEmptyConstructor> queryable = from p in (IQueryable<Moein>)c.Moein
						where p.Guid == guid
						select p into u
						select new MoeinBusinessWithEmptyConstructor
						{
							ContextGuid = ContextGuid,
							ChangeStatus = EntityChangeSt.Edited,
							Status = u.Status,
							Guid = u.Guid,
							DeliveryMessage = u.DeliveryMessage,
							DeliveryShortMessage = u.DeliveryShortMessage,
							Account = u.Account,
							Code = u.Code,
							DateM = u.DateM,
							DeliveryDate = u.DeliveryDate,
							DeliveryId = u.DeliveryId,
							Description = u.Description,
							HostRefrenced = u.HostRefrenced,
							Modified = u.Modified,
							Name = u.Name,
							ServerStatus = u.ServerStatus,
							System = u.System,
							KolGuid = u.KolGuid,
							Mahiat = u.Mahiat,
							Peygiri = u.Peygiri,
							Tedadi = u.Tedadi
						};
					result = await ((queryable != null) ? EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<MoeinBusinessWithEmptyConstructor>(queryable, token) : null);
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
					WebErrorLog.Instance.StartLog(ex2, $"Guid:{guid}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 226);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<MoeinBusiness>> GetAllAsync(string search, Guid? kolGuid, Guid? tafsilLevelGuid, CancellationToken token, bool? status, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 237));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			IEnumerable<MoeinBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IEnumerable<string> enumerable = search?.RemoveCharactersForLinkSearch()?.FixString()?.SplitString()?.Distinct();
					IQueryable<Moein> queryable = ((IQueryable<Moein>)c.Moein).Select((Moein u) => new Moein
					{
						MoeinTafsilLevelRelation = u.MoeinTafsilLevelRelation,
						Status = u.Status,
						Guid = u.Guid,
						DeliveryMessage = u.DeliveryMessage,
						DeliveryShortMessage = u.DeliveryShortMessage,
						Account = u.Account,
						Code = u.Code,
						DateM = u.DateM,
						DeliveryDate = u.DeliveryDate,
						DeliveryId = u.DeliveryId,
						Description = u.Description,
						HostRefrenced = u.HostRefrenced,
						Modified = u.Modified,
						Name = u.Name,
						ServerStatus = u.ServerStatus,
						System = u.System,
						KolGuid = u.KolGuid,
						Mahiat = u.Mahiat,
						Peygiri = u.Peygiri,
						Tedadi = u.Tedadi
					});
					if (enumerable != null && enumerable.Any())
					{
						foreach (string word in enumerable)
						{
							if (!string.IsNullOrEmpty(word))
							{
								queryable = queryable.Where((Moein p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.Name, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.Code.ToString(), $"%{word}%"));
							}
						}
					}
					if (status.HasValue)
					{
						queryable = queryable.Where((Moein p) => p.Status == ((bool?)status).Value);
					}
					if ((kolGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable.Where((Moein q) => q.KolGuid == kolGuid);
					}
					if ((tafsilLevelGuid ?? Guid.Empty) != Guid.Empty)
					{
						EntityFrameworkQueryableExtensions.Include<Moein, ICollection<MoeinTafsilGroupRelations>>(queryable, (Expression<Func<Moein, ICollection<MoeinTafsilGroupRelations>>>)((Moein p) => p.MoeinTafsilLevelRelation));
						queryable = queryable.Where((Moein q) => q.MoeinTafsilLevelRelation.Any((MoeinTafsilGroupRelations tlr) => tlr.TafsilGroupGuid == tafsilLevelGuid));
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						List<MoeinBusiness> list;
						if (token.IsCancellationRequested)
						{
							list = null;
						}
						else
						{
							string contextGuid = ContextGuid;
							list = await MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<Moein>(queryable, default(CancellationToken)), null, tranName, token);
						}
						result = list;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 286);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<long> NextCodeInGroupAsync(Guid KolGuid, long startCode, long endCode, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 298));
			long result;
			try
			{
				IQueryable<long> source = from p in (IQueryable<Moein>)acc_ModelContext.Moein
					where p.KolGuid == KolGuid
					select p into q
					select q.Code;
				IQueryable<long> source2 = ((IQueryable<Moein>)acc_ModelContext.Moein).Select((Moein q) => q.Code);
				startCode++;
				long num = startCode;
				while (true)
				{
					if (num < endCode)
					{
						if (!source.Contains(num) && !source2.Contains(num))
						{
							result = num;
							break;
						}
						num++;
						continue;
					}
					result = startCode;
					break;
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
				WebErrorLog.Instance.StartLog(ex2, "", "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 314);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override MoeinBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)dbContext.GetContext(tranName, 60000, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 326);
			try
			{
				return (from p in (IQueryable<Moein>)acc_ModelContext.Moein
					where p.Guid == guid
					select p into u
					select new MoeinBusinessWithEmptyConstructor
					{
						ContextGuid = ContextGuid,
						ChangeStatus = EntityChangeSt.Edited,
						Status = u.Status,
						Guid = u.Guid,
						DeliveryMessage = u.DeliveryMessage,
						DeliveryShortMessage = u.DeliveryShortMessage,
						Account = u.Account,
						Code = u.Code,
						DateM = u.DateM,
						DeliveryDate = u.DeliveryDate,
						DeliveryId = u.DeliveryId,
						Description = u.Description,
						HostRefrenced = u.HostRefrenced,
						Modified = u.Modified,
						Name = u.Name,
						ServerStatus = u.ServerStatus,
						System = u.System,
						KolGuid = u.KolGuid,
						Mahiat = u.Mahiat,
						Peygiri = u.Peygiri,
						Tedadi = u.Tedadi
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
				WebErrorLog.Instance.StartLog(ex2, $"Guid:{guid}", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 358);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					acc_ModelContext.Dispose("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs");
				}
			}
		}
	}

	public async Task<MoeinBusiness> GetAsync(long CodeMoein, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 370));
			MoeinBusiness result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<MoeinBusinessWithEmptyConstructor>(from p in (IQueryable<Moein>)c.Moein
						where p.Code == CodeMoein
						select p into u
						select new MoeinBusinessWithEmptyConstructor
						{
							ContextGuid = ContextGuid,
							ChangeStatus = EntityChangeSt.Edited,
							Status = u.Status,
							Guid = u.Guid,
							DeliveryMessage = u.DeliveryMessage,
							DeliveryShortMessage = u.DeliveryShortMessage,
							Account = u.Account,
							Code = u.Code,
							DateM = u.DateM,
							DeliveryDate = u.DeliveryDate,
							DeliveryId = u.DeliveryId,
							Description = u.Description,
							HostRefrenced = u.HostRefrenced,
							Modified = u.Modified,
							Name = u.Name,
							ServerStatus = u.ServerStatus,
							System = u.System,
							KolGuid = u.KolGuid,
							Mahiat = u.Mahiat,
							Peygiri = u.Peygiri,
							Tedadi = u.Tedadi
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
					if (ex2.Message.Contains("Sequence contains more than one element"))
					{
						Log.Debug(ex2, $"Code:{CodeMoein}");
					}
					else
					{
						WebErrorLog.Instance.StartLog(ex2, $"Code:{CodeMoein}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 405);
					}
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 423);
		}
	}

	public Task<MoeinBusiness> MapAsync(string contextGuid, Moein source, MoeinBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
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
					destination = new MoeinBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.KolGuid = source.KolGuid;
				destination.Code = source.Code;
				destination.Status = source.Status;
				destination.Description = source.Description;
				destination.Mahiat = source.Mahiat;
				destination.System = source.System;
				destination.Peygiri = source.Peygiri;
				destination.Tedadi = source.Tedadi;
				destination.Account = source.Account;
				destination.DateM = source.DateM;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.ChangeStatus = EntityChangeSt.Edited;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 464);
				throw;
			}
		}
	}

	public Task<List<MoeinBusiness>> MapListAsync(string contextGuid, IEnumerable<Moein> source, List<MoeinBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebMoein> items, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 477));
				List<Moein> list = await Mapper.MapListAsync(ContextGuid, items, null, tranName, token);
				if (list == null || !list.Any())
				{
					return ret;
				}
				foreach (Moein item in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, token, item.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 481);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 485);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\MoeinPersistanceRepository.cs", 486);
			}
			return ret;
		}
	}
}
