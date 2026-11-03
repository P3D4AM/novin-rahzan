using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Global;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Global;

internal class StimulQueryPersistenceRepository : PersistenceRepository<StimulQueryBusiness, StimulQuery>, IStimulQueryRepository, IRepository<StimulQueryBusiness>, INoGuidRepository<StimulQueryBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<StimulQueryBusiness, StimulQuery>, IItemAsyncMapper<StimulQuery, StimulQueryBusiness>
	{
		public Task<StimulQuery> MapAsync(string contextGuid, StimulQueryBusiness source, StimulQuery destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new StimulQuery();
				}
				destination.Guid = source.Guid;
				destination.Modified = DateTime.Now;
				destination.RefrencedId = source.RefrencedId;
				destination.Order = source.Order;
				destination.Date = DateTime.Now;
				destination.Id = source.Id;
				destination.SecondKey = source.SecondKey;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 157);
				throw;
			}
		}

		public Task<StimulQueryBusiness> MapAsync(string contextGuid, StimulQuery source, StimulQueryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new StimulQueryBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.RefrencedId = source.RefrencedId;
				destination.Order = source.Order;
				destination.Date = source.Date;
				destination.Id = source.Id;
				destination.SecondKey = source.SecondKey;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 184);
				throw;
			}
		}

		public Task<List<StimulQuery>> MapListAsync(string contextGuid, IEnumerable<StimulQueryBusiness> source, List<StimulQuery> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<StimulQueryBusiness>> MapListAsync(string contextGuid, IEnumerable<StimulQuery> source, List<StimulQueryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private DateTime _lastTruncate = DateTime.MinValue;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<StimulQuery, StimulQueryBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<StimulQuery, StimulQueryBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<StimulQueryBusiness, StimulQuery> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public StimulQueryPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 42);
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> GetNextRefrenced(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
		using (new JaegerMonitor("GetNextRefrenced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextRefrenced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 48));
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					if (_lastTruncate.AddHours(6.0) < DateTime.Now)
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await c.ExecuteNoneQueryAsync($"delete from stimulQuery where date<'{DateTime.Now.AddHours(-6.0)}'", null, tranName, token, "", CommandType.Text, 30, "GetNextRefrenced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs"), ReturnedState.None, "GetNextRefrenced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 53);
						_lastTruncate = DateTime.Now;
					}
					if (await EntityFrameworkQueryableExtensions.CountAsync<StimulQuery>((IQueryable<StimulQuery>)c.StimulQuery, token) <= 0)
					{
						ret.value = 1;
						return ret;
					}
					ret.value = await EntityFrameworkQueryableExtensions.MaxAsync<StimulQuery, int>((IQueryable<StimulQuery>)c.StimulQuery, (Expression<Func<StimulQuery, int>>)((StimulQuery q) => q.RefrencedId), token) + 1;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetNextRefrenced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 67);
					ret.AddReturnedValue(ex, "", "GetNextRefrenced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 68);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextRefrenced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs");
				}
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(StimulQueryBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 85));
				DateTime modified = (item.Date = DateTime.Now);
				item.Modified = modified;
				StimulQuery item2 = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item2, item.RefrencedId, item.Order), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 88);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 92);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 93);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<StimulQueryBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 104));
				await ((DbContext)c).AddRangeAsync((IEnumerable<object>)(await MapToDatabase.MapListAsync(ContextGuid, items, null, tranName, token)), token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 110);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 111);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> TruncateAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs"))
		{
			Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 119));
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await global_ModelContext.ExecuteNoneQueryAsync("Truncate Table StimulQuery", null, tranName, token, "", CommandType.Text, 30, "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs"), ReturnedState.None, "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 122);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 126);
				res.AddReturnedValue(ex, "", "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\StimulQueryPersistenceRepository.cs", 127);
			}
			return res;
		}
	}
}
