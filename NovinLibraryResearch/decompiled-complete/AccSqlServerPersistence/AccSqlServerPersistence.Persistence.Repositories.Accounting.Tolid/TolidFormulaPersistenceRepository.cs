using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;

internal class TolidFormulaPersistenceRepository : PersistenceRepository<TolidFormulaBusiness, TolidFormula>, ITolidFormulaRepository, IRepository<TolidFormulaBusiness>, INoGuidRepository<TolidFormulaBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidFormulaBusiness, TolidFormula>, IItemAsyncMapper<TolidFormula, TolidFormulaBusiness>
	{
		public Task<TolidFormula> MapAsync(string contextGuid, TolidFormulaBusiness source, TolidFormula destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs"))
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
						destination = new TolidFormula();
					}
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Title = source.Title;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 255);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<TolidFormulaBusiness> MapAsync(string contextGuid, TolidFormula source, TolidFormulaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs"))
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
						destination = new TolidFormulaBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Title = source.Title;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 282);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<TolidFormula>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaBusiness> source, List<TolidFormula> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidFormulaBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormula> source, List<TolidFormulaBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private TolidFormulaExportPersistenceRepository _export;

	private TolidFormulaHazinePersistenceRepository _hazine;

	private TolidFormulaMavadPersistenceRepository _mavad;

	public new bool IsDisposed { get; set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TolidFormula, TolidFormulaBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidFormulaBusiness, TolidFormula> MapToDatabase => Mapper;

	public override IItemMapper<TolidFormula, TolidFormulaBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus => false;

	public override bool HasModified => true;

	internal TolidFormulaPersistenceRepository(ContextHolder context)
		: base(context)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs"))
		{
			try
			{
				dbContext = context;
				_mavad = new TolidFormulaMavadPersistenceRepository(context);
				_hazine = new TolidFormulaHazinePersistenceRepository(context);
				_export = new TolidFormulaExportPersistenceRepository(context);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 45);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				dbContext?.Dispose();
				dbContext = null;
				_export?.Dispose();
				_export = null;
				_hazine?.Dispose();
				_hazine = null;
				_mavad?.Dispose();
				_mavad = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 67);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TolidFormulaBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 72);
		try
		{
			TolidFormula tolidFormula = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(tolidFormula, tolidFormula.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 76);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _mavad.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 80);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _hazine.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 81);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _export.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 82);
			if (res.HasError)
			{
				return res;
			}
			OrderBusiness orderBusiness = await item.GetMavadListAsync(EnSystem.Mapper, tranName, token);
			if (orderBusiness != null && orderBusiness.ListKala.Count > 0)
			{
				returnedSaveFuncInfo = res;
				TolidFormulaMavadPersistenceRepository mavad = _mavad;
				returnedSaveFuncInfo.AddReturnedValue(await mavad.SaveAsync(await TolidMavadMapper.Instance.MapListAsync(ContextGuid, orderBusiness.ListKala, new List<TolidFormulaMavadBusiness>(), "", token), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 87);
			}
			OrderBusiness orderBusiness2 = await item.GetExportListAsync(EnSystem.Mapper, tranName, token);
			if ((orderBusiness2?.ListKala?.Any()).GetValueOrDefault())
			{
				returnedSaveFuncInfo = res;
				TolidFormulaExportPersistenceRepository export = _export;
				returnedSaveFuncInfo.AddReturnedValue(await export.SaveAsync(await TolidExportMapper.Instance.MapListAsync(ContextGuid, orderBusiness2.ListKala, new List<TolidFormulaExportBusiness>(), tranName, token), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 91);
			}
			if (item.HazineList != null && item.HazineList.Count > 0)
			{
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _hazine.SaveAsync(item.HazineList as List<TolidFormulaHazineBusiness>, item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 93);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 97);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 98);
		}
		return res;
	}

	public override async Task<IEnumerable<TolidFormulaBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 104));
		IEnumerable<TolidFormulaBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<TolidFormula> tolidFormulas = (IQueryable<TolidFormula>)c.TolidFormulas;
				result = ((tolidFormulas != null && tolidFormulas.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, tolidFormulas?.ToList(), null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 129);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 143));
			TolidFormula obj = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TolidFormula>((IQueryable<TolidFormula>)dbcontext.TolidFormulas, (Expression<Func<TolidFormula, bool>>)((TolidFormula p) => p.Guid == guid), token);
			if (obj == null)
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _export.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 147);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _hazine.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 149);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _mavad.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 151);
			if (res.HasError)
			{
				return res;
			}
			dbcontext.TolidFormulas.Remove(obj);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 158);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 159);
		}
		return res;
	}

	public override async Task<TolidFormulaBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 165));
		TolidFormulaBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 9;
			try
			{
				TolidFormula tolidFormula = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<TolidFormula>((IQueryable<TolidFormula>)c.TolidFormulas, (Expression<Func<TolidFormula, bool>>)((TolidFormula q) => q.Guid == guid), token);
				if (tolidFormula == null)
				{
					result = null;
				}
				else
				{
					TolidFormulaBusiness busList = await MapFromDatabase.MapAsync(ContextGuid, tolidFormula, null, tranName, token);
					List<TolidFormulaExportBusiness> exportList = await _export.GetAllAsync(guid, token, tranName);
					List<TolidFormulaMavadBusiness> mavadList = await _mavad.GetAllAsync(guid, token, tranName);
					List<TolidFormulaHazineBusiness> hazineList = await _hazine.GetAllAsync(guid, token, tranName);
					if (exportList?.Any() ?? false)
					{
						await busList.AddExportListAsync(EnSystem.Mapper, await TolidExportMapper.Instance.MapListAsync(ContextGuid, exportList, new List<OrderDetailBusiness>(), tranName, token), clearDets: false, tranName, token);
					}
					if (mavadList?.Any() ?? false)
					{
						await busList.AddMavadListAsync(EnSystem.Mapper, await TolidMavadMapper.Instance.MapListAsync(ContextGuid, mavadList, new List<OrderDetailBusiness>(), tranName, token), clearDets: false, tranName, token);
					}
					if (hazineList?.Any() ?? false)
					{
						await busList.AddHazineListAsync(hazineList, EnSystem.Mapper);
					}
					result = busList;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 192);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override Task<TolidFormulaBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public override Task<IEnumerable<TolidFormulaBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		return GetAllAsync(token, tranName);
	}

	public async Task<List<TolidFormulaBusiness>> GetAllByProductAsync(Guid prdGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllByProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 205));
		List<TolidFormulaBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<TolidFormula> tolidFormulas = (IQueryable<TolidFormula>)c.TolidFormulas;
				if (tolidFormulas == null || !tolidFormulas.Any())
				{
					result = null;
				}
				else
				{
					IQueryable<TolidFormulaExport> exportList = ((IQueryable<TolidFormulaExport>)c.TolidFormulaExports).Where((TolidFormulaExport q) => q.ProductGuid == prdGuid);
					if (!(exportList?.Any() ?? false))
					{
						result = null;
					}
					else
					{
						tolidFormulas = tolidFormulas?.Where((TolidFormula q) => exportList.Select((TolidFormulaExport q) => q.MasterGuid).Contains(q.Guid));
						result = await MapFromDatabase.MapListAsync(ContextGuid, tolidFormulas, null, "", token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllByProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs", 222);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllByProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaPersistenceRepository.cs");
			}
		}
		return result;
	}
}
