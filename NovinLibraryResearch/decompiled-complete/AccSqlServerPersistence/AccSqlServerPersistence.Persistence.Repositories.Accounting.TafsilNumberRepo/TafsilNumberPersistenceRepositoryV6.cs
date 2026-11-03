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
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.TafsilNumberRepo;

internal class TafsilNumberPersistenceRepositoryV6 : PersistenceNoGuidRepository<TafsilNumberBusiness, TafsilNumber>, ITafsilNumberRepository, INoGuidRepository<TafsilNumberBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TafsilNumberBusiness, TafsilNumber>, IItemAsyncMapper<TafsilNumber, TafsilNumberBusiness>
	{
		public Task<TafsilNumber> MapAsync(string contextGuid, TafsilNumberBusiness source, TafsilNumber destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TafsilNumber();
				}
				destination.TafsilGuid = source.TafsilGuid;
				destination.OriginalNumber = source.OriginalNumber;
				destination.CalcedNumber = source.CalcedNumber;
				destination.Prefix = source.Prefix;
				destination.Title = source.Title;
				destination.Ordinal = source.Ordinal;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 310);
				throw;
			}
		}

		public Task<TafsilNumberBusiness> MapAsync(string contextGuid, TafsilNumber source, TafsilNumberBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TafsilNumberBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.TafsilGuid = source.TafsilGuid;
				destination.OriginalNumber = source.OriginalNumber;
				destination.CalcedNumber = source.CalcedNumber;
				destination.Prefix = source.Prefix;
				destination.Title = source.Title;
				destination.Ordinal = source.Ordinal;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 337);
				throw;
			}
		}

		public Task<List<TafsilNumber>> MapListAsync(string contextGuid, IEnumerable<TafsilNumberBusiness> source, List<TafsilNumber> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TafsilNumberBusiness>> MapListAsync(string contextGuid, IEnumerable<TafsilNumber> source, List<TafsilNumberBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<TafsilNumber, TafsilNumberBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TafsilNumberBusiness, TafsilNumber> MapToDatabase => Mapper;

	public TafsilNumberPersistenceRepositoryV6(ContextHolder context)
		: base(context)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 38);
		}
	}

	public async Task<List<TafsilNumberBusiness>> GetAllAsync(Guid tafsilGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 42));
		List<TafsilNumberBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				IQueryable<TafsilNumber> queryable = ((IQueryable<TafsilNumber>)c.TafsilNumber).Where((TafsilNumber q) => q.TafsilGuid == tafsilGuid);
				List<TafsilNumber> source = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<TafsilNumber>(queryable, token) : null);
				result = await MapFromDatabase.MapListAsync(base.ContextGuid, source, null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 52);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs");
			}
		}
		return result;
	}

	public async Task<List<TafsilNumberBusiness>> GetInfoAsync(long number, bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 63));
		using (new JaegerMonitor("GetInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs"))
		{
			List<TafsilNumberBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<TafsilNumber> source = await EntityFrameworkQueryableExtensions.ToListAsync<TafsilNumber>((IQueryable<TafsilNumber>)EntityFrameworkQueryableExtensions.Include<TafsilNumber, Tafsil>(((IQueryable<TafsilNumber>)c.TafsilNumber).Where((TafsilNumber x) => x.OriginalNumber == number && (status == null || (bool?)x.TafsilItem.Status == status)), (Expression<Func<TafsilNumber, Tafsil>>)((TafsilNumber x) => x.TafsilItem)), token);
					result = await MapFromDatabase.MapListAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 74);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<string>> GetTitleListAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetTitleListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 85));
		List<string> result;
		try
		{
			_ = 1;
			try
			{
				DbSet<TafsilNumber> tafsilNumber = c.TafsilNumber;
				object obj;
				if (tafsilNumber == null)
				{
					obj = null;
				}
				else
				{
					IQueryable<string> queryable = ((IQueryable<TafsilNumber>)tafsilNumber).Select((TafsilNumber q) => q.Title);
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<string> queryable2 = queryable.Distinct();
						if (queryable2 == null)
						{
							obj = null;
						}
						else
						{
							IQueryable<string> queryable3 = queryable2.Where((string q) => !string.IsNullOrEmpty(q));
							obj = ((queryable3 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<string>(queryable3, token) : null);
						}
					}
				}
				result = await (Task<List<string>>)obj;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetTitleListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 94);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetTitleListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs");
			}
		}
		return result;
	}

	public async Task<List<TafsilNumberBusiness>> GetInfoByCalcNumberAsync(long number, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetInfoByCalcNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 105));
		using (new JaegerMonitor("GetInfoByCalcNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs"))
		{
			List<TafsilNumberBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (!(((IQueryable<TafsilNumber>)c.TafsilNumber)?.Any() ?? false))
					{
						result = null;
					}
					else
					{
						IQueryable<TafsilNumber> queryable = ((IQueryable<TafsilNumber>)c.TafsilNumber).Where((TafsilNumber x) => x.CalcedNumber == number);
						List<TafsilNumber> source = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<TafsilNumber>(queryable, token) : null);
						result = await MapFromDatabase.MapListAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetInfoByCalcNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 118);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetInfoByCalcNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(List<Guid> tafsilGuids, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 133));
				List<TafsilNumberBusiness> obj = new List<TafsilNumberBusiness>();
				foreach (Guid item in tafsilGuids)
				{
					TafsilNumber tafsilNumber = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TafsilNumber>((IQueryable<TafsilNumber>)dbcontext.TafsilNumber, (Expression<Func<TafsilNumber, bool>>)((TafsilNumber w) => w.TafsilGuid == item), token);
					if (tafsilNumber != null)
					{
						List<TafsilNumberBusiness> list = obj;
						list.Add(await MapFromDatabase.MapAsync(base.ContextGuid, tafsilNumber, null, tranName, token));
					}
				}
				if (obj.Count <= 0)
				{
					return ret;
				}
				dbcontext.TafsilNumber?.RemoveRange((IEnumerable<TafsilNumber>)(await MapToDatabase.MapListAsync(base.ContextGuid, obj, null, "", token)));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 146);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 147);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid tafsilGuid, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 160));
				IQueryable<TafsilNumber> queryable = ((IQueryable<TafsilNumber>)dbcontext.TafsilNumber).Where((TafsilNumber w) => w.TafsilGuid == tafsilGuid);
				List<TafsilNumber> list = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<TafsilNumber>(queryable, token) : null);
				if (list.Count <= 0)
				{
					return ret;
				}
				dbcontext.TafsilNumber.RemoveRange((IEnumerable<TafsilNumber>)list);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 167);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 168);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid tafsilGuid, long Number, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 180));
				TafsilNumber tafsilNumber = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TafsilNumber>((IQueryable<TafsilNumber>)context.TafsilNumber, (Expression<Func<TafsilNumber, bool>>)((TafsilNumber w) => w.TafsilGuid == tafsilGuid && w.OriginalNumber == Number), token);
				if (tafsilNumber == null)
				{
					return ret;
				}
				context.TafsilNumber.Remove(tafsilNumber);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 188);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 189);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TafsilNumberBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			foreach (TafsilNumberBusiness item in items)
			{
				if (item.Ordinal <= 0)
				{
					item.Ordinal = (short)((items?.Max((TafsilNumberBusiness p) => p.Ordinal) ?? (-1)) + 1);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 202);
				if (res.HasError)
				{
					return res;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 208);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 209);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TafsilNumberBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext dbContext1 = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 220));
				TafsilNumber item2 = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbContext1.AddOrUpdateAsync(item2, item.TafsilGuid, item.OriginalNumber), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 222);
				return new ReturnedSaveFuncInfo();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 228);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 229);
			}
			return ret;
		}
	}

	public async Task<List<TafsilNumberReportBusiness>> GetAllReportAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 236));
			List<TafsilNumberReportBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<TafsilNumberReportBusinessWituotCtor> queryable = from q in (IQueryable<TafsilNumber>)EntityFrameworkQueryableExtensions.Include<TafsilNumber, Tafsil>((IQueryable<TafsilNumber>)c.TafsilNumber, (Expression<Func<TafsilNumber, Tafsil>>)((TafsilNumber q) => q.TafsilItem))
						where q.TafsilGuid == q.TafsilItem.Guid
						select new TafsilNumberReportBusinessWituotCtor
						{
							Name = q.TafsilItem.Name,
							Title = q.Title,
							Guid = q.TafsilItem.Guid,
							TafsilGuid = q.TafsilItem.Guid,
							OriginalNumber = q.OriginalNumber
						};
					result = ((IEnumerable<TafsilNumberReportBusinessWituotCtor>)(await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<TafsilNumberReportBusinessWituotCtor>(queryable, token) : null))).Select((Func<TafsilNumberReportBusinessWituotCtor, TafsilNumberReportBusiness>)((TafsilNumberReportBusinessWituotCtor p) => p)).ToList();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 254);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<TafsilNumberBusiness>> GetAllAsync(List<Guid> tafsilGuids, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 262));
			List<TafsilNumberBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<TafsilNumberBusinessWituotCtor> queryable = from q in (IQueryable<TafsilNumber>)c.TafsilNumber
						where tafsilGuids.Contains(q.TafsilGuid)
						select new TafsilNumberBusinessWituotCtor
						{
							ContextGuid = ContextGuid,
							Title = q.Title,
							TafsilGuid = q.TafsilGuid,
							OriginalNumber = q.OriginalNumber
						};
					result = ((IEnumerable<TafsilNumberBusinessWituotCtor>)(await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<TafsilNumberBusinessWituotCtor>(queryable, token) : null))).Select((Func<TafsilNumberBusinessWituotCtor, TafsilNumberBusiness>)((TafsilNumberBusinessWituotCtor p) => p)).ToList();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs", 280);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TafsilNumberRepo\\TafsilNumberPersistenceRepositoryV6.cs");
				}
			}
			return result;
		}
	}
}
