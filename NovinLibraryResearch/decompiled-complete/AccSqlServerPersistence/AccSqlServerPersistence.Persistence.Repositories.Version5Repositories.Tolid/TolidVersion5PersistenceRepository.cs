using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using EntityCache.Business;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Tolid;

internal class TolidVersion5PersistenceRepository : ITolidRepository, IEntityRepository<TolidBusiness, TolidReportBusiness>, IEditableDelivery, IDisposable
{
	private ContextHolder dbContext;

	private TolidHazineVersion5PersistenceRepository _hazine;

	private TolidMavadVersion5PersistenceRepository _mavad;

	private TolidExportVersion5PersistenceRepository _export;

	private PrdSerialPersistanceRepository _serialRepo;

	public WebContextHolder Context => dbContext.Context;

	public string ContextGuid => dbContext.Context.Guid;

	public Task ClearCachedAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		TolidBusiness tolidBusiness = await GetAsync(guid, token, tranName);
		tolidBusiness.SanadNo = sanadNo;
		return await SaveAsync(tolidBusiness, 1, token, tranName);
	}

	internal TolidVersion5PersistenceRepository(ContextHolder context)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs"))
		{
			try
			{
				dbContext = context;
				_serialRepo = new PrdSerialPersistanceRepository(context);
				_hazine = new TolidHazineVersion5PersistenceRepository(context);
				_mavad = new TolidMavadVersion5PersistenceRepository(context, _serialRepo);
				_export = new TolidExportVersion5PersistenceRepository(context, _serialRepo);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 54);
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

	public Task<TolidReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(TolidBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<TolidBusiness>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task Add2CacheAsync(TolidBusiness item, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task ClearCachedItemAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	private async Task<ReturnedSaveFuncInfo> UpdateVSanadDetailAsync(OrderMode mode, Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TolidBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<TolidBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		return GetAllAsync(null, token, tranName);
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

	private async Task<ReturnedSaveFuncInfo> RemoveAsync_(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<TolidBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 233));
		TolidBusiness item = null;
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Tolid_SelectRow", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					item = await LoadDataAsync(c, dr, isLoadDets: true, token, tranName);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 244);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs");
			}
		}
		return item;
	}

	public Task<TolidBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public async Task<IEnumerable<TolidBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 257));
		List<TolidBusiness> list = new List<TolidBusiness>();
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Tolid_SelectAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<TolidBusiness> list2 = list;
					list2.Add(await LoadDataAsync(c, dr, isLoadDets: false, token));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 267);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs");
			}
		}
		return list?.OrderBy((TolidBusiness q) => q.DateM)?.ToList();
	}

	private async Task<TolidBusiness> LoadDataAsync(Acc_ModelContext c, DbDataReader dr, bool isLoadDets, CancellationToken token, string tranName = "")
	{
		_ = 8;
		try
		{
			TolidBusiness item = await TolidBusiness.GenerateNewAsync(ContextGuid, EnSystem.None, token);
			if (dr["GUID"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["GUID"];
			}
			if (dr["ID2"] != DBNull.Value)
			{
				item.SetId2((int)dr["ID2"]);
			}
			if (dr["DateM"] != DBNull.Value)
			{
				item.DateM = (DateTime)dr["DateM"];
			}
			if (dr["SanadNO"] != DBNull.Value)
			{
				item.SanadNo = (int)dr["SanadNO"];
			}
			if (dr["SanadST"] != DBNull.Value)
			{
				item.SanadSt = (SanadSt)dr["SanadST"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				item.Description = dr["Description"].ToString();
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				item.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["MavadTafsilGuid"] != DBNull.Value)
			{
				item.MavadTafsilGuid = (Guid?)dr["MavadTafsilGuid"];
			}
			if (dr["MavadMoeinGuid"] != DBNull.Value)
			{
				item.MavadMoeinGuid = (Guid)dr["MavadMoeinGuid"];
			}
			if (dr["MahsolTafsilGuid"] != DBNull.Value)
			{
				item.MahsolTafsilGuid = (Guid?)dr["MahsolTafsilGuid"];
			}
			if (dr["MahsolMoeinGuid"] != DBNull.Value)
			{
				item.MahsolMoeinGuid = (Guid)dr["MahsolMoeinGuid"];
			}
			if (isLoadDets)
			{
				List<TolidExportBusiness> exportList = await _export.GetAllAsync(item.Guid, token, tranName);
				List<TolidHazineBusiness> hazineList = await _hazine.GetAllAsync(item.Guid, token, tranName);
				List<TolidMavadBusiness> mavadList = await _mavad.GetAllAsync(item.Guid, token, tranName);
				if (exportList?.Any() ?? false)
				{
					await item.AddExportListAsync(EnSystem.Mapper, await TolidExportMapper.Instance.MapListAsync(ContextGuid, exportList, new List<OrderDetailBusiness>(), "", token));
				}
				if (mavadList?.Any() ?? false)
				{
					await item.AddMavadListAsync(EnSystem.Mapper, await TolidMavadMapper.Instance.MapListAsync(ContextGuid, mavadList, new List<OrderDetailBusiness>(), "", token));
				}
				if (hazineList != null && hazineList.Count > 0)
				{
					await item.AddHazineListAsync(hazineList, EnSystem.Mapper);
				}
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 316);
			throw;
		}
	}

	private TolidReportBusiness LoadReportAsync(DbDataReader dr, CancellationToken token)
	{
		TolidReportBusiness tolidReportBusiness = new TolidReportBusiness();
		try
		{
			if (dr["GUID"] != DBNull.Value)
			{
				tolidReportBusiness.Guid = (Guid)dr["GUID"];
			}
			if (dr["ID2"] != DBNull.Value)
			{
				tolidReportBusiness.Id2 = (int)dr["ID2"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				tolidReportBusiness.DateM = (DateTime)dr["DateM"];
			}
			if (dr["SanadNO"] != DBNull.Value)
			{
				tolidReportBusiness.SanadNo = (int)dr["SanadNO"];
			}
			if (dr["SanadST"] != DBNull.Value)
			{
				tolidReportBusiness.SanadSt = (SanadSt)dr["SanadST"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				tolidReportBusiness.Description = dr["Description"].ToString();
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				tolidReportBusiness.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["MavadTafsilGuid"] != DBNull.Value)
			{
				tolidReportBusiness.MavadTafsilGuid = (Guid?)dr["MavadTafsilGuid"];
			}
			if (dr["MavadMoeinGuid"] != DBNull.Value)
			{
				tolidReportBusiness.MavadMoeinGuid = (Guid)dr["MavadMoeinGuid"];
			}
			if (dr["MahsolTafsilGuid"] != DBNull.Value)
			{
				tolidReportBusiness.MahsolTafsilGuid = (Guid?)dr["MahsolTafsilGuid"];
			}
			if (dr["MahsolMoeinGuid"] != DBNull.Value)
			{
				tolidReportBusiness.MahsolMoeinGuid = (Guid)dr["MahsolMoeinGuid"];
			}
			if (dr["SUMMavad"] != DBNull.Value)
			{
				tolidReportBusiness.SumMavad = (decimal)dr["SUMMavad"];
			}
			if (dr["SUMMahsol"] != DBNull.Value)
			{
				tolidReportBusiness.SumMahsol = (decimal)dr["SUMMahsol"];
			}
			if (dr["SUMHazine"] != DBNull.Value)
			{
				tolidReportBusiness.SumHazine = (decimal)dr["SUMHazine"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 342);
			throw;
		}
		return tolidReportBusiness;
	}

	public async Task<int> NextId2Async(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 349);
		int res = 0;
		try
		{
			_ = 1;
			try
			{
				res = await c.ExecuteScalarAsync<int>("sp_Tolid_NextID2", null, tranName, token, 30, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 357);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<IEnumerable<TolidReportBusiness>> GetAllReportAsync(TolidFilter filter, CancellationToken token, string tranName)
	{
		List<TolidReportBusiness> list = new List<TolidReportBusiness>();
		BaseModelContext baseModelContext = await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 370);
		try
		{
			using (DbDataReader dr = await baseModelContext.ExecuteReaderAsync("sp_Tolid_SelectAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs"))
			{
				while (await dr.ReadAsync())
				{
					list.Add(LoadReportAsync(dr, token));
				}
				dr.Close();
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidVersion5PersistenceRepository.cs", 379);
			throw;
		}
	}

	public Task<TolidBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public TolidBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}
}
