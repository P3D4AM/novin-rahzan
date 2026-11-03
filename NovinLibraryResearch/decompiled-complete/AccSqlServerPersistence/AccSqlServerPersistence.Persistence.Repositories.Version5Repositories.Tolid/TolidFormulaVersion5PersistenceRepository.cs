using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Tolid;

internal class TolidFormulaVersion5PersistenceRepository : ITolidFormulaRepository, IRepository<TolidFormulaBusiness>, INoGuidRepository<TolidFormulaBusiness>, IDisposable
{
	private ContextHolder dbContext;

	private TolidFormulaHazineVersion5PersistenceRepository _hazine;

	private TolidFormulaMavadVersion5PersistenceRepository _mavad;

	private TolidFormulaExportVersion5PersistenceRepository _export;

	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => dbContext.Context.Guid;

	internal TolidFormulaVersion5PersistenceRepository(ContextHolder context)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs"))
		{
			try
			{
				dbContext = context;
				_hazine = new TolidFormulaHazineVersion5PersistenceRepository(context);
				_mavad = new TolidFormulaMavadVersion5PersistenceRepository(context);
				_export = new TolidFormulaExportVersion5PersistenceRepository(context);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs", 37);
				throw;
			}
		}
	}

	public void Dispose()
	{
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(TolidFormulaBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TolidFormulaBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<TolidFormulaBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		List<TolidFormulaBusiness> list = new List<TolidFormulaBusiness>();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs", 99);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Tolid_formula_SelectAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync(token))
				{
					List<TolidFormulaBusiness> list2 = list;
					list2.Add(await LoadDataAsync(dr, isLoadDets: false));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs", 109);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	private async Task<TolidFormulaBusiness> LoadDataAsync(DbDataReader dr, bool isLoadDets, string tranName = "")
	{
		TolidFormulaBusiness item = new TolidFormulaBusiness(ContextGuid);
		try
		{
			if (dr["Guid"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["Guid"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				item.DateM = (DateTime)dr["DateM"];
			}
			if (dr["Title"] != DBNull.Value)
			{
				item.Title = dr["Title"].ToString();
			}
			if (dr["Description"] != DBNull.Value)
			{
				item.Description = dr["Description"].ToString();
			}
			if (isLoadDets)
			{
				List<TolidFormulaExportBusiness> exportList = await _export.GetAllAsync(item.Guid, default(CancellationToken), tranName);
				List<TolidFormulaMavadBusiness> mavadList = await _mavad.GetAllAsync(item.Guid, default(CancellationToken), tranName);
				List<TolidFormulaHazineBusiness> hazineList = await _hazine.GetAllAsync(item.Guid, default(CancellationToken), tranName);
				if (exportList != null && exportList.Count > 0)
				{
					TolidFormulaBusiness tolidFormulaBusiness = item;
					await tolidFormulaBusiness.AddExportListAsync(EnSystem.None, await TolidExportMapper.Instance.MapListAsync(ContextGuid, exportList, new List<OrderDetailBusiness>(), tranName));
				}
				if (mavadList != null && mavadList.Count > 0)
				{
					TolidFormulaBusiness tolidFormulaBusiness = item;
					await tolidFormulaBusiness.AddMavadListAsync(EnSystem.None, await TolidMavadMapper.Instance.MapListAsync(ContextGuid, mavadList, new List<OrderDetailBusiness>(), tranName));
				}
				if (hazineList != null && hazineList.Count > 0)
				{
					await item.AddHazineListAsync(hazineList, EnSystem.None);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs", 144);
			throw;
		}
		return item;
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
		throw new NotImplementedException();
	}

	private async Task<ReturnedSaveFuncInfo> RemoveAsync_(Guid guid, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<TolidFormulaBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		TolidFormulaBusiness res = null;
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs", 201);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Tolid_formula_SelectRow", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = await LoadDataAsync(dr, isLoadDets: true, tranName);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs", 211);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<TolidFormulaBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public Task<IEnumerable<TolidFormulaBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		return GetAllAsync(token, tranName);
	}

	public async Task<List<TolidFormulaBusiness>> GetAllByProductAsync(Guid prdGuid, CancellationToken token, string tranName)
	{
		List<TolidFormulaBusiness> res = new List<TolidFormulaBusiness>();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllByProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs", 226);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@prdGuid", (object)prdGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("TolidFormulaExport_GetFormulaGuid_ByPrdGuid", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllByProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<TolidFormulaBusiness> list = res;
					list.Add(await LoadDataAsync(dr, isLoadDets: true, tranName));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllByProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs", 237);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllByProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public TolidFormulaBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}
}
