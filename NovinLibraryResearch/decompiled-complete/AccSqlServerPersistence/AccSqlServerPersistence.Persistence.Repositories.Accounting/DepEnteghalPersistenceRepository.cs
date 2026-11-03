using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class DepEnteghalPersistenceRepository : IDepEnteghalRepository, IRepository<DepEnteghalBusiness>, INoGuidRepository<DepEnteghalBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DepEnteghalBusiness, DepEnteghal>, IItemAsyncMapper<DepEnteghal, DepEnteghalBusiness>
	{
		public async Task<DepEnteghal> MapAsync(string contextGuid, DepEnteghalBusiness source, DepEnteghal destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
			{
				_ = 3;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new DepEnteghal();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.UserGuid = source.UserGuid;
					destination.No = source.No;
					destination.Description = source.Description;
					destination.DateM = source.DateM;
					destination.ModifiedM = source.ModifiedM;
					destination.MashinNo = source.MashinNo;
					destination.GirandeName = source.GirandeName;
					destination.RanandehName = source.RanandehName;
					destination.StatusId = source.StatusId;
					DepEnteghal depEnteghal = destination;
					depEnteghal.DebitMoeinGuid = await source.GetDebitMoeinGuidAsync();
					depEnteghal = destination;
					depEnteghal.DebitTafsilGuid = await source.GetDebitTafsilGuidAsync();
					depEnteghal = destination;
					depEnteghal.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync();
					depEnteghal = destination;
					depEnteghal.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync();
					destination.SanadNo = source.SanadNo;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 372);
					throw;
				}
			}
		}

		public Task<DepEnteghalBusiness> MapAsync(string contextGuid, DepEnteghal source, DepEnteghalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
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
						destination = new DepEnteghalBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.UserGuid = source.UserGuid;
					destination.No = source.No;
					destination.Description = source.Description;
					destination.DateM = source.DateM;
					destination.ModifiedM = source.ModifiedM;
					destination.MashinNo = source.MashinNo;
					destination.GirandeName = source.GirandeName;
					destination.RanandehName = source.RanandehName;
					destination.StatusId = source.StatusId;
					destination.DebitMoeinGuid = source.DebitMoeinGuid;
					destination.DebitTafsilGuid = source.DebitTafsilGuid;
					destination.CreditMoeinGuid = source.CreditMoeinGuid;
					destination.CreditTafsilGuid = source.CreditTafsilGuid;
					destination.SanadNo = source.SanadNo;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 410);
					throw;
				}
			}
		}

		public Task<List<DepEnteghal>> MapListAsync(string contextGuid, IEnumerable<DepEnteghalBusiness> source, List<DepEnteghal> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DepEnteghalBusiness>> MapListAsync(string contextGuid, IEnumerable<DepEnteghal> source, List<DepEnteghalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private DepEnteghalDetPersistenceRepository depEnteghalDet;

	private ContextHolder dbContext;

	private bool _isDisposed;

	public IItemAsyncMapper<DepEnteghal, DepEnteghalBusiness> MapFromDatabase { get; set; } = new mapper();


	public IItemAsyncMapper<DepEnteghalBusiness, DepEnteghal> MapToDatabase { get; set; } = new mapper();


	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => dbContext.Context.Guid;

	public DepEnteghalPersistenceRepository(ContextHolder db)
	{
		depEnteghalDet = new DepEnteghalDetPersistenceRepository(db);
		dbContext = db;
	}

	public async Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		int ret = 0;
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 41));
		try
		{
			ret = ((!status.HasValue) ? ((IQueryable<DepEnteghal>)acc_ModelContext.DepEnteghals).Count() : ((!status.Value) ? ((IQueryable<DepEnteghal>)acc_ModelContext.DepEnteghals).Count((DepEnteghal q) => !q.Status) : ((IQueryable<DepEnteghal>)acc_ModelContext.DepEnteghals).Count((DepEnteghal q) => q.Status)));
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
			WebErrorLog.Instance.StartLog(ex2, "", "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 52);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs");
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(DepEnteghalBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 69));
				DepEnteghal depEnteghal = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(depEnteghal, depEnteghal.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 71);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await depEnteghalDet.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 72);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await depEnteghalDet.SaveAsync(item.LstEnteghalDet?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 73);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 78);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 79);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<DepEnteghalBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<DepEnteghalBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 87));
			IEnumerable<DepEnteghalBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<DepEnteghal> depEnteghals = (IQueryable<DepEnteghal>)c.DepEnteghals;
					List<DepEnteghalBusiness> query = await MapFromDatabase.MapListAsync(ContextGuid, depEnteghals.ToList(), null, tranName, token);
					foreach (DepEnteghalBusiness item in query)
					{
						DepEnteghalBusiness depEnteghalBusiness = item;
						depEnteghalBusiness.AddRangeToDetList(await depEnteghalDet.GetAllAsync(item.Guid, token, tranName), clearList: true);
					}
					result = query;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 101);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<DepEnteghalBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public async Task<IEnumerable<DepEnteghalBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 114));
			IEnumerable<DepEnteghalBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					IQueryable<DepEnteghal> queryable = (IQueryable<DepEnteghal>)c.DepEnteghals;
					if (status.HasValue)
					{
						queryable = queryable.Where((DepEnteghal q) => (bool?)q.Status == status);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<DepEnteghal, DepEnteghalBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						List<DepEnteghalBusiness> query = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<DepEnteghal>(queryable, token), null, tranName, token);
						foreach (DepEnteghalBusiness item in query)
						{
							DepEnteghalBusiness depEnteghalBusiness = item;
							depEnteghalBusiness.AddRangeToDetList(await depEnteghalDet.GetAllAsync(item.Guid, token, tranName), clearList: true);
						}
						result = query;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 129);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 147));
				DepEnteghal obj = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<DepEnteghal>((IQueryable<DepEnteghal>)dbcontext.DepEnteghals, (Expression<Func<DepEnteghal, bool>>)((DepEnteghal x) => x.Guid == guid), token);
				await depEnteghalDet.RemoveAsync(guid, tryCount, token, tranName);
				dbcontext.DepEnteghals.Remove(obj);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 155);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 156);
			}
			return ret;
		}
	}

	public async Task<DepEnteghalBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 163));
			DepEnteghalBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					DepEnteghal depEnteghal = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<DepEnteghal>((IQueryable<DepEnteghal>)c.DepEnteghals, (Expression<Func<DepEnteghal, bool>>)((DepEnteghal x) => x.Guid == guid), token);
					if (depEnteghal == null)
					{
						result = null;
					}
					else
					{
						DepEnteghalBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, depEnteghal, null, tranName, token);
						DepEnteghalBusiness depEnteghalBusiness = ret;
						depEnteghalBusiness.AddRangeToDetList(await depEnteghalDet.GetAllAsync(guid, token, tranName), clearList: true);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 177);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<DepEnteghalBusiness> GetAsync(int no, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 189));
			DepEnteghalBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					DepEnteghal depEnteghal = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<DepEnteghal>((IQueryable<DepEnteghal>)c.DepEnteghals, (Expression<Func<DepEnteghal, bool>>)((DepEnteghal x) => x.No == no), token);
					if (depEnteghal == null)
					{
						result = null;
					}
					else
					{
						DepEnteghalBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, depEnteghal, null, tranName, token);
						DepEnteghalBusiness depEnteghalBusiness = ret;
						depEnteghalBusiness.AddRangeToDetList(await depEnteghalDet.GetAllAsync(ret.Guid, token, tranName), clearList: true);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 203);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> SelectLastIdGAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SelectLastIdGAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 214));
		using (new JaegerMonitor("SelectLastIdGAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			int result;
			try
			{
				int i = (((IQueryable<DepEnteghal>)acc_ModelContext.DepEnteghals).Where((DepEnteghal p) => p.No > 0)?.Select((DepEnteghal q) => q.No)?.MinOrDefault(1)).Value;
				for (HashSet<int> hashSet = ((IQueryable<DepEnteghal>)acc_ModelContext.DepEnteghals).Select((DepEnteghal p) => p.No).ToHashSet(); hashSet.Contains(i); i++)
				{
				}
				result = i;
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
				WebErrorLog.Instance.StartLog(ex3, "", "SelectLastIdGAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 230);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("SelectLastIdGAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> CountQtByMasterGuidAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("CountQtByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CountQtByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 242));
			int result;
			try
			{
				result = ((IQueryable<DepEnteghalDet>)acc_ModelContext.DepEnteghalDets)?.Count((DepEnteghalDet p) => p.MasterGuid == masterGuid) ?? 0;
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
				WebErrorLog.Instance.StartLog(ex2, "", "CountQtByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 251);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("CountQtByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<DepEnteghalReportBusiness>> GetAllAsync(DepEnteghalFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 262));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			IEnumerable<DepEnteghalReportBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<DepEnteghal> queryable = ((IEnumerable<DepEnteghal>)EntityFrameworkQueryableExtensions.Include<DepEnteghal, ICollection<DepEnteghalDet>>((IQueryable<DepEnteghal>)((DbContext)c).Set<DepEnteghal>(), (Expression<Func<DepEnteghal, ICollection<DepEnteghalDet>>>)((DepEnteghal p) => p.DepEnteghalDets))).AsQueryable();
					if (filter.Status.HasValue)
					{
						queryable = queryable.Where((DepEnteghal p) => p.Status == filter.Status.Value);
					}
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable?.Where((DepEnteghal q) => q.DateM >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable?.Where((DepEnteghal q) => q.DateM <= filter.DateTo);
					}
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						ClsCache cache = ClsCache.GetCache(ContextGuid);
						result = await EntityFrameworkQueryableExtensions.ToListAsync<DepEnteghalReportBusiness>(queryable.Select((DepEnteghal q) => new DepEnteghalReportBusiness
						{
							Date = q.DateM,
							Description = q.Description,
							DriverName = q.RanandehName,
							Guid = q.Guid,
							Number = q.No,
							ReceiverName = q.GirandeName,
							SumPrice = Math.Round(q.DepEnteghalDets.Sum((DepEnteghalDet p) => p.Price * p.Quantity), cache.MultiLanguageConfig.DigitForRound),
							SumQuantity = (double)q.DepEnteghalDets.Sum((DepEnteghalDet p) => p.Quantity)
						}), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 291);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<DepEnteghalBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 310);
		}
	}

	public DepEnteghalBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 321));
				DepEnteghal depEnteghal = await dbcontext.DepEnteghals.FindAsync(new object[1] { guid });
				depEnteghal.SanadNo = sanadNo;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(depEnteghal, guid), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 324);
				return ret;
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
				WebErrorLog.Instance.StartLog(ex3, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalPersistenceRepository.cs", 331);
				throw;
			}
		}
	}
}
