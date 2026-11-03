using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters.Khazane;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global;

internal class WagePersistenceRepository : PersistenceRepository<WageBusiness, Wage>, IWageRepository, IRepository<WageBusiness>, INoGuidRepository<WageBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<WageBusiness, Wage>, IItemAsyncMapper<Wage, WageBusiness>
	{
		public Task<Wage> MapAsync(string contextGuid, WageBusiness source, Wage destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Wage();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.Name = source.Name;
				destination.Type = source.Type;
				destination.CalculationMethod = source.CalculationMethod;
				destination.PriceOrPercent = source.PriceOrPercent;
				destination.MinTransaction = source.MinTransaction;
				destination.MaxTransaction = source.MaxTransaction;
				destination.MasterType = source.MasterType;
				destination.ServerGuid = source.ServerGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs", 163);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<WageBusiness> MapAsync(string contextGuid, Wage source, WageBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new WageBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.Name = source.Name;
				destination.Type = source.Type;
				destination.CalculationMethod = source.CalculationMethod;
				destination.PriceOrPercent = source.PriceOrPercent;
				destination.MinTransaction = source.MinTransaction;
				destination.MaxTransaction = source.MaxTransaction;
				destination.MasterType = source.MasterType;
				destination.ServerGuid = source.ServerGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs", 194);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Wage>> MapListAsync(string contextGuid, IEnumerable<WageBusiness> source, List<Wage> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<WageBusiness>> MapListAsync(string contextGuid, IEnumerable<Wage> source, List<WageBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Wage, WageBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Wage, WageBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<WageBusiness, Wage> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public WagePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs", 44);
		}
	}

	public async Task<IEnumerable<WageBusiness>> GetAllAsync(WageFilter filter, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs", 49));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs"))
		{
			IEnumerable<WageBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Wage> queryable = (IQueryable<Wage>)c.Wage;
					if (filter != null)
					{
						if (filter.MasterType.HasValue)
						{
							queryable = queryable?.Where((Wage q) => q.MasterType == filter.MasterType.Value);
						}
						if (filter.WageType != 0)
						{
							queryable = queryable?.Where((Wage q) => q.Type == filter.WageType);
						}
						if (filter.Price != 0m)
						{
							queryable = queryable?.Where((Wage q) => (q.MinTransaction <= filter.Price || q.MinTransaction == 0m) && (q.MaxTransaction >= filter.Price || q.MaxTransaction == 0m));
						}
						if (filter.Status.HasValue)
						{
							queryable = queryable?.Where((Wage q) => (bool?)q.Status == filter.Status);
						}
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<Wage, WageBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<Wage>(queryable, token), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs", 69);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WageBusiness>>> IsDuplicateCodeAsync(EnKhazaneMaster master, EnWageType typeEnteghal, decimal minPrice, decimal maxPrice, Guid guid, EnWageCalculationMethod calcMethod, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs", 80));
		using (new JaegerMonitor("IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<WageBusiness>> ret = new ReturnedSaveFuncInfoWithValue<List<WageBusiness>>();
			ReturnedSaveFuncInfoWithValue<List<WageBusiness>> result;
			try
			{
				_ = 1;
				try
				{
					if (minPrice == 0m && maxPrice == 0m)
					{
						result = ret;
						goto IL_0586;
					}
					List<Wage> list = ((IQueryable<Wage>)((DbContext)c).Set<Wage>()).Where((Wage q) => q.MasterType == master && q.Type == typeEnteghal && q.CalculationMethod == calcMethod && q.Status).ToList();
					if (list == null || list.Count <= 0)
					{
						result = ret;
						goto IL_0586;
					}
					foreach (Wage item in list)
					{
						if (((minPrice >= item.MinTransaction && minPrice <= item.MaxTransaction) || (maxPrice >= item.MinTransaction && maxPrice <= item.MaxTransaction)) && item.Guid != guid)
						{
							ReturnedSaveFuncInfoWithValue<List<WageBusiness>> returnedSaveFuncInfoWithValue = ret;
							if (returnedSaveFuncInfoWithValue.value == null)
							{
								returnedSaveFuncInfoWithValue.value = new List<WageBusiness>();
							}
							List<WageBusiness> value = ret.value;
							value.Add(await MapFromDatabase.MapAsync(ContextGuid, item, null, tranName, token));
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
					WebErrorLog.Instance.StartLog(ex2, "", "IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs", 101);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs");
				}
			}
			return ret;
			IL_0586:
			return result;
		}
	}

	public async Task<WageBusiness> GetByServerGuidAsync(Guid serverGuid, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByServerGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs", 113));
		WageBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Wage wage = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Wage>((IQueryable<Wage>)c.Wage, (Expression<Func<Wage, bool>>)((Wage q) => q.ServerGuid != null && q.ServerGuid == serverGuid), token);
				result = ((wage != null) ? (await MapFromDatabase.MapAsync(ContextGuid, wage, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetByServerGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs", 125);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetByServerGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\WagePersistenceRepository.cs");
			}
		}
		return result;
	}
}
