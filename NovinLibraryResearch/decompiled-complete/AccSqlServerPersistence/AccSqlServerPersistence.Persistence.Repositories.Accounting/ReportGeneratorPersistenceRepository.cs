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
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ReportGeneratorPersistenceRepository : PersistenceRepository<ReportGeneratorBusiness, ReportGenerator>, IReportGeneratorRepository, IRepository<ReportGeneratorBusiness>, INoGuidRepository<ReportGeneratorBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ReportGeneratorBusiness, ReportGenerator>, IItemAsyncMapper<ReportGenerator, ReportGeneratorBusiness>
	{
		public Task<ReportGenerator> MapAsync(string contextGuid, ReportGeneratorBusiness source, ReportGenerator destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReportGenerator();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.Description = source.Description;
				destination.Rpt = source.Rpt;
				destination.Version = source.Version;
				destination.LastUpdate = source.LastUpdate;
				destination.Type = source.Type;
				destination.Document = source.Document;
				destination.Status = source.Status;
				destination.MaliYearSanadType = source.MaliYearSanadType;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 180);
				throw;
			}
		}

		public Task<ReportGeneratorBusiness> MapAsync(string contextGuid, ReportGenerator source, ReportGeneratorBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReportGeneratorBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.Description = source.Description;
				destination.Rpt = source.Rpt;
				destination.Version = source.Version;
				destination.LastUpdate = source.LastUpdate;
				destination.Type = source.Type;
				destination.Document = source.Document;
				destination.Status = source.Status;
				destination.MaliYearSanadType = source.MaliYearSanadType;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 212);
				throw;
			}
		}

		public Task<List<ReportGenerator>> MapListAsync(string contextGuid, IEnumerable<ReportGeneratorBusiness> source, List<ReportGenerator> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReportGeneratorBusiness>> MapListAsync(string contextGuid, IEnumerable<ReportGenerator> source, List<ReportGeneratorBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private ReportGeneratorDetPersistenceRepository _detailRepository;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public override IItemAsyncMapper<ReportGenerator, ReportGeneratorBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReportGeneratorBusiness, ReportGenerator> MapToDatabase => Mapper;

	public override IItemMapper<ReportGenerator, ReportGeneratorBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ReportGeneratorPersistenceRepository(ContextHolder db, ReportGeneratorDetPersistenceRepository detailRepository)
		: base(db)
	{
		_detailRepository = detailRepository;
	}

	public override ReportGeneratorBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ReportGeneratorBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReportGeneratorBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			ReportGeneratorBusiness ret = await base.GetAsync(guid, token, tranName);
			if (ret != null)
			{
				ReportGeneratorBusiness reportGeneratorBusiness = ret;
				reportGeneratorBusiness.DetList = await _detailRepository.GetAllAsync(guid, token, tranName);
			}
			return ret;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 51);
			throw;
		}
	}

	public new async Task<IEnumerable<ReportGeneratorBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		try
		{
			IEnumerable<ReportGeneratorBusiness> enumerable = await GetAllAsync(token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			if (status.HasValue)
			{
				enumerable = enumerable?.Where((ReportGeneratorBusiness q) => q.Status == status.Value);
			}
			return enumerable;
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 68);
			throw;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await _detailRepository.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 77);
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await base.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 78);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 82);
			ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 83);
		}
		return ret;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ReportGeneratorBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await base.SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 92);
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await _detailRepository.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 93);
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await _detailRepository.SaveAsync(item.DetList, item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 94);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 98);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 99);
		}
		return ret;
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_detailRepository?.Dispose();
				_detailRepository = null;
				dbContext = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 118);
		}
	}

	public async Task<List<ReportGeneratorBusiness>> GetAllAsync(ReportGeneratorFilter filter, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 122));
		List<ReportGeneratorBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ReportGenerator> source = (IQueryable<ReportGenerator>)c.ReportGenerator;
				if (filter != null)
				{
					if (filter.TypeList?.Any() ?? false)
					{
						source = source.Where((ReportGenerator x) => x.Type != null && filter.TypeList.Contains(x.Type.Value));
					}
					if (filter.MaliYearSanadType.HasValue)
					{
						source = source.Where((ReportGenerator x) => x.MaliYearSanadType == filter.MaliYearSanadType.Value);
					}
					if (filter.Status.HasValue)
					{
						source = source.Where((ReportGenerator x) => x.Status == filter.Status.Value);
					}
				}
				result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs", 140);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReportGeneratorPersistenceRepository.cs");
			}
		}
		return result;
	}
}
