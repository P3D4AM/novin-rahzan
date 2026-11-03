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

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class KolPersistanceRepository : PersistenceSyncRepository<KolBusiness, Kol>, IKolRepository, ISyncRepository<KolBusiness>, IEditableDelivery, IRepository<KolBusiness>, INoGuidRepository<KolBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<KolBusiness, Kol>, IItemAsyncMapper<Kol, KolBusiness>, IItemMapper<Kol, KolBusiness>, IItemAsyncMapper<WebKol, Kol>
	{
		public KolBusiness Map(string contextGuid, Kol source, KolBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException("نباید این تابع صدا زده شود.");
		}

		public Task<Kol> MapAsync(string contextGuid, KolBusiness source, Kol destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
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
						destination = new Kol();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.GroupGuid = source.GroupGuid;
					destination.Code = source.Code;
					destination.Status = source.Status;
					destination.Description = source.Description;
					destination.System = source.System;
					destination.Account = source.Account;
					destination.DateM = source.DateM;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.GardeshHesab = source.GardeshHesab;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 501);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<KolBusiness> MapAsync(string contextGuid, Kol source, KolBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public Task<Kol> MapAsync(string contextGuid, WebKol source, Kol destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
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
						destination = new Kol();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.GroupGuid = source.GroupGuid;
					destination.Code = source.Code;
					destination.Status = source.Status;
					destination.Description = source.Description;
					destination.System = source.System;
					destination.Account = source.Account;
					destination.DateM = source.DateM;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.GardeshHesab = source.GardeshHesab;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 541);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<Kol>> MapListAsync(string contextGuid, IEnumerable<KolBusiness> source, List<Kol> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<KolBusiness>> MapListAsync(string contextGuid, IEnumerable<Kol> source, List<KolBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Kol>> MapListAsync(string contextGuid, IEnumerable<WebKol> source, List<Kol> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Kol, KolBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<KolBusiness, Kol> MapToDatabase => Mapper;

	public override IItemMapper<Kol, KolBusiness> MapFromDatabaseNoneAsync => Mapper;

	public KolPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<KolBusiness> GetAsync(int codeKol, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 36));
			KolBusiness result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<KolBusinessWithEmptyConstructor>(from p in (IQueryable<Kol>)c.Kol
						where p.Code == codeKol
						select p into u
						select new KolBusinessWithEmptyConstructor
						{
							ChangeStatus = EntityChangeSt.Edited,
							ContextGuid = ContextGuid,
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
							Mahiat = u.Mahiat,
							GroupGuid = u.GroupGuid
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 67);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> CountChilderenNotZeroAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("CountChilderenNotZeroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CountChilderenNotZeroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 79));
			int result;
			try
			{
				result = ((IQueryable<Moein>)acc_ModelContext.Moein).Where((Moein x) => x.KolGuid == guid && x.Status).Count();
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
				WebErrorLog.Instance.StartLog(ex2, "", "CountChilderenNotZeroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 88);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("CountChilderenNotZeroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(KolBusiness kol, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("please fill parameter updateAccount!");
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<KolBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new ArgumentException("please fill parameter updateAccount!");
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(KolBusiness kol, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 110));
				Kol newKol = await MapToDatabase.MapAsync(ContextGuid, kol, null, tranName, token);
				Kol kol2 = await ((DbContext)c).FindAsync<Kol>(new object[1] { kol.Guid });
				if (kol2 == null)
				{
					kol2 = newKol;
					if (!updateAccount)
					{
						kol2.Account = 0m;
					}
					((DbContext)c).Add<Kol>(newKol);
				}
				else
				{
					if (kol2.GroupGuid != kol.GroupGuid)
					{
						throw new InvalidOperationException($"update HesabGroupGuid from ({kol2.GroupGuid}) to ({kol.GroupGuid}) is not valid!");
					}
					if (!updateAccount)
					{
						newKol.Account = kol2.Account;
					}
					((EntityEntry)((DbContext)c).Entry<Kol>(kol2)).CurrentValues.SetValues((object)newKol);
					((DbContext)c).Update<Kol>(kol2);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 130);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 131);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<KolBusiness> lstHesabsListKol, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				if (lstHesabsListKol != null && lstHesabsListKol.Any())
				{
					foreach (KolBusiness item in lstHesabsListKol)
					{
						if (item != null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 147);
							if (ret.HasError)
							{
								return ret;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 153);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 154);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<KolBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, Guid? groupGuid, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 161));
			IEnumerable<KolBusiness> result;
			try
			{
				_ = 1;
				try
				{
					search = search.FixString().Trim();
					List<string> list = search.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<KolBusinessWithEmptyConstructor> queryable = ((IQueryable<Kol>)c.Kol).Select((Kol u) => new KolBusinessWithEmptyConstructor
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
							Mahiat = u.Mahiat,
							GroupGuid = u.GroupGuid
						});
						foreach (string item in list)
						{
							string word = item.Trim();
							if (!string.IsNullOrEmpty(word))
							{
								queryable = queryable?.Where((KolBusinessWithEmptyConstructor p) => DbFunctionsExtensions.Like(EF.Functions, p.Name, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.Code.ToString(), $"%{word}%"));
							}
						}
						if (status.HasValue)
						{
							queryable = queryable?.Where((KolBusinessWithEmptyConstructor p) => (bool?)p.Status == status);
						}
						if ((groupGuid ?? Guid.Empty) != Guid.Empty)
						{
							queryable = queryable?.Where((KolBusinessWithEmptyConstructor p) => p.GroupGuid == ((Guid?)groupGuid).Value);
						}
						result = ((!token.IsCancellationRequested) ? (await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<KolBusinessWithEmptyConstructor>(queryable, token) : null)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 205);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> NextCodeInGroupAsync(Guid groupGuid, int startCode, int endCode, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 217));
			int result;
			try
			{
				IQueryable<int> source = from p in (IQueryable<Kol>)acc_ModelContext.Kol
					where p.GroupGuid == groupGuid
					select p.Code;
				startCode++;
				int num = startCode;
				while (true)
				{
					if (num < endCode)
					{
						if (!source.Contains(num))
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
				WebErrorLog.Instance.StartLog(ex2, "", "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 231);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs");
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

	public async Task<IEnumerable<KolBusiness>> GetAllBetweenCodesAsync(long? code1, long? code2, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllBetweenCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllBetweenCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 247));
			IEnumerable<KolBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<KolBusinessWithEmptyConstructor> list = await EntityFrameworkQueryableExtensions.ToListAsync<KolBusinessWithEmptyConstructor>(((IQueryable<Kol>)c.Kol).Select((Kol u) => new KolBusinessWithEmptyConstructor
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
						Mahiat = u.Mahiat,
						GroupGuid = u.GroupGuid
					}), token);
					if (code1.HasValue)
					{
						list = list.Where((KolBusinessWithEmptyConstructor p) => p.Code >= code1.Value)?.ToList();
					}
					if (code2.HasValue)
					{
						list = list.Where((KolBusinessWithEmptyConstructor p) => p.Code <= code2.Value)?.ToList();
					}
					result = list;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllBetweenCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 280);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllBetweenCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override KolBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)dbContext.GetContext(tranName, 60000, "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 292);
			try
			{
				return ((IQueryable<Kol>)acc_ModelContext.Kol).Select((Kol u) => new KolBusinessWithEmptyConstructor
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
					Mahiat = u.Mahiat,
					GroupGuid = u.GroupGuid
				}).SingleOrDefault((KolBusinessWithEmptyConstructor p) => p.Guid == guid);
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
				WebErrorLog.Instance.StartLog(ex2, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 322);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					acc_ModelContext.Dispose("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs");
				}
			}
		}
	}

	public override async Task<KolBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 334));
			KolBusiness result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<KolBusinessWithEmptyConstructor>(((IQueryable<Kol>)c.Kol).Select((Kol u) => new KolBusinessWithEmptyConstructor
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
						Mahiat = u.Mahiat,
						GroupGuid = u.GroupGuid
					}), (Expression<Func<KolBusinessWithEmptyConstructor, bool>>)((KolBusinessWithEmptyConstructor p) => p.Guid == guid), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 364);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<byte[]> GetGardeshHesabAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetGardeshHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetGardeshHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 376));
			byte[] result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Kol>((IQueryable<Kol>)c.Kol, (Expression<Func<Kol, bool>>)((Kol x) => x.Guid == guid), token))?.GardeshHesab;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetGardeshHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 387);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetGardeshHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 405);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebKol> items, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 414));
				List<Kol> list = await Mapper.MapListAsync(ContextGuid, items, null, tranName, token);
				if (list == null || !list.Any())
				{
					return ret;
				}
				foreach (Kol item in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, token, item.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 418);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 422);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\KolPersistanceRepository.cs", 423);
			}
			return ret;
		}
	}
}
