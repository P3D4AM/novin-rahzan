using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Global;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Global;

internal class SaveTempPersistenceRepository : PersistenceRepository<SaveTempBusiness, SaveTemp>, ISaveTempRepository, IRepository<SaveTempBusiness>, INoGuidRepository<SaveTempBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SaveTemp, SaveTempBusiness>, IItemAsyncMapper<SaveTempBusiness, SaveTemp>
	{
		public Task<SaveTemp> MapAsync(string contextGuid, SaveTempBusiness source, SaveTemp destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SaveTemp();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.Id2 = source.Id2;
				destination.Description = source.Description;
				destination.SerialedData = source.SerialedData;
				destination.DateM = source.DateM;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 151);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<SaveTempBusiness> MapAsync(string contextGuid, SaveTemp source, SaveTempBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SaveTempBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.Id2 = source.Id2;
				destination.Description = source.Description;
				destination.SerialedData = source.SerialedData;
				destination.DateM = source.DateM;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 181);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<SaveTemp>> MapListAsync(string contextGuid, IEnumerable<SaveTempBusiness> source, List<SaveTemp> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SaveTempBusiness>> MapListAsync(string contextGuid, IEnumerable<SaveTemp> source, List<SaveTempBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<SaveTemp, SaveTempBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<SaveTemp, SaveTempBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SaveTempBusiness, SaveTemp> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public SaveTempPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 42);
		}
	}

	public async Task<IEnumerable<SaveTempViewModel>> GetAllAsync(SaveTempFilter filter, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 46));
		IEnumerable<SaveTempViewModel> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<SaveTemp> queryable = (IQueryable<SaveTemp>)c.SaveTemp;
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					if (filter != null)
					{
						if (filter.DateFrom.HasValue)
						{
							queryable = queryable?.Where((SaveTemp q) => q.DateM >= filter.DateFrom);
						}
						if (filter.DateTo.HasValue)
						{
							queryable = queryable?.Where((SaveTemp q) => q.DateM <= filter.DateTo);
						}
						if (filter.Id2From > 0)
						{
							queryable = queryable?.Where((SaveTemp q) => q.Id2 >= filter.Id2From);
						}
						if (filter.Id2To > 0)
						{
							queryable = queryable?.Where((SaveTemp q) => q.Id2 <= filter.Id2To);
						}
						if (filter.Status.HasValue)
						{
							queryable = queryable?.Where((SaveTemp q) => (bool?)q.Status == filter.Status);
						}
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						var enumerable = from q in queryable.ToList()
							group q by new { q.AsGuid, q.AsType };
						if (enumerable == null || !enumerable.Any())
						{
							result = null;
						}
						else
						{
							List<SaveTempViewModel> list = enumerable.Select(q => new SaveTempViewModel
							{
								AsGuid = q.Key.AsGuid,
								AsType = q.Key.AsType,
								DateM = q.FirstOrDefault().DateM,
								Description = q.FirstOrDefault().Description,
								Guid = q.FirstOrDefault().Guid,
								Id2 = q.FirstOrDefault().Id2,
								Status = q.FirstOrDefault().Status
							})?.ToList();
							await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)c).Database, "Update Global_SaveTemp Set Status = {0}", (IEnumerable<object>)new List<object> { false }, token);
							result = ((list?.Any() ?? false) ? list : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 80);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 92));
		try
		{
			IQueryable<SaveTemp> queryable = ((IQueryable<SaveTemp>)global_ModelContext.SaveTemp).Where((SaveTemp q) => q.AsGuid == asGuid && q.AsType == asType);
			if (queryable == null || !queryable.Any())
			{
				return res;
			}
			global_ModelContext.SaveTemp.RemoveRange((IEnumerable<SaveTemp>)queryable);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 101);
			res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 102);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeByDateAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 109));
		try
		{
			IQueryable<SaveTemp> queryable = ((IQueryable<SaveTemp>)global_ModelContext.SaveTemp).Where((SaveTemp q) => q.DateM < DateTime.Now.AddDays(-15.0));
			if (queryable == null || !queryable.Any())
			{
				return res;
			}
			global_ModelContext.SaveTemp.RemoveRange((IEnumerable<SaveTemp>)queryable);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 118);
			res.AddReturnedValue(ex, "", "RemoveRangeByDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\SaveTempPersistenceRepository.cs", 119);
		}
		return res;
	}
}
