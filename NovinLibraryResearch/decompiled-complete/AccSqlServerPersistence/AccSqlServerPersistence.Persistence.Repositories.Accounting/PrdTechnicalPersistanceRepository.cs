using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

public class PrdTechnicalPersistanceRepository : PersistenceRepository<PrdTechnicalBusiness, PrdTechnical>, IPrdTechnicalRepository, IRepository<PrdTechnicalBusiness>, INoGuidRepository<PrdTechnicalBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdTechnicalBusiness, PrdTechnical>, IItemAsyncMapper<PrdTechnical, PrdTechnicalBusiness>
	{
		public Task<PrdTechnical> MapAsync(string contextGuid, PrdTechnicalBusiness source, PrdTechnical destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdTechnical();
				}
				destination.Code = source.Code;
				destination.Name = source.Name;
				destination.TechnicalType = source.TechnicalType;
				destination.ProductGuid = source.ProductGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 178);
				throw;
			}
		}

		public Task<PrdTechnicalBusiness> MapAsync(string contextGuid, PrdTechnical source, PrdTechnicalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdTechnicalBusiness(contextGuid);
				}
				destination.Code = source.Code;
				destination.Name = source.Name;
				destination.TechnicalType = source.TechnicalType;
				destination.ProductGuid = source.ProductGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 204);
				throw;
			}
		}

		public Task<List<PrdTechnical>> MapListAsync(string contextGuid, IEnumerable<PrdTechnicalBusiness> source, List<PrdTechnical> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdTechnicalBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdTechnical> source, List<PrdTechnicalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<PrdTechnical, PrdTechnicalBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdTechnicalBusiness, PrdTechnical> MapToDatabase => Mapper;

	public override IItemMapper<PrdTechnical, PrdTechnicalBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public PrdTechnicalPersistanceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 40);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(PrdTechnicalBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 50);
				PrdTechnical prdTechnical = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				if (item.Guid == Guid.Empty)
				{
					throw new Exception($"Guid is empty. item type:{item.GetType()}");
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(prdTechnical, prdTechnical.ProductGuid, prdTechnical.TechnicalType), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 54);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 58);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 59);
			}
			return ret;
		}
	}

	public async Task<PrdTechnicalBusiness> GetAsync(Guid guid, short technicalType, CancellationToken token, string tranName)
	{
		Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 65));
		PrdTechnicalBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				PrdTechnical prdTechnical = await context.PrdTechnicals.FindAsync(new object[2] { guid, technicalType });
				result = ((prdTechnical != null) ? (await MapFromDatabase.MapAsync(ContextGuid, prdTechnical, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 77);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await context.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<PrdTechnicalBusiness>> GetAllAsync(short technicalType, bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext context = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 88));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs"))
		{
			IEnumerable<PrdTechnicalBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<PrdTechnical> queryable = ((IQueryable<PrdTechnical>)context.PrdTechnicals).Where((PrdTechnical q) => q.TechnicalType == technicalType);
					if (status.HasValue)
					{
						queryable = queryable.Where((PrdTechnical p) => p.Status == ((bool?)status).Value);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IEnumerable<PrdTechnical> source = queryable?.ToList()?.GroupBy((PrdTechnical q) => q.Name)?.Select((IGrouping<string, PrdTechnical> s) => s.First());
						result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 102);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await context.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<PrdTechnicalBusiness>> GetAllAsync(Guid prdGuid, bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 113));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs"))
		{
			IEnumerable<PrdTechnicalBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<PrdTechnical> queryable = ((IQueryable<PrdTechnical>)c.PrdTechnicals).Where((PrdTechnical q) => q.ProductGuid == prdGuid);
					if (status.HasValue)
					{
						queryable = queryable.Where((PrdTechnical p) => p.Status == ((bool?)status).Value);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<PrdTechnical, PrdTechnicalBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<PrdTechnical>(queryable, token), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 126);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveByProductGuidAsync(Guid prdGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (prdGuid == Guid.Empty)
			{
				throw new ArgumentException("ProductGuid is Empty");
			}
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveByProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 141));
			List<PrdTechnical> list = await EntityFrameworkQueryableExtensions.ToListAsync<PrdTechnical>(((IQueryable<PrdTechnical>)c.PrdTechnicals).Where((PrdTechnical q) => q.ProductGuid == prdGuid), token);
			if (list == null || !list.Any())
			{
				return res;
			}
			c.PrdTechnicals.RemoveRange((IEnumerable<PrdTechnical>)list);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveByProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 148);
			res.AddReturnedValue(ex, "", "RemoveByProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdTechnicalPersistanceRepository.cs", 149);
		}
		return res;
	}
}
