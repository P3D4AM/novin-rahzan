using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Tolid;

internal class TolidFormulaExportVersion5PersistenceRepository : ITolidFormulaExportRepository, IPersistenceMasterRepository<TolidFormulaExportBusiness>, IDisposable
{
	private string ContextGuid;

	public ContextHolder DatabaseContext { get; private set; }

	public TolidFormulaExportVersion5PersistenceRepository(ContextHolder dbContext)
	{
		DatabaseContext = dbContext;
		ContextGuid = dbContext.Context.Guid;
	}

	public void Dispose()
	{
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<TolidFormulaExportBusiness> items, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<TolidFormulaExportBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		List<TolidFormulaExportBusiness> list = new List<TolidFormulaExportBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaExportVersion5PersistenceRepository.cs", 78);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@MasterGuid", (object)masterGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Tolid_Formula_Export_SelectRowsByMaster", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaExportVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<TolidFormulaExportBusiness> list2 = list;
					list2.Add(await LoadDataAsync(dr, token, tranName));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaExportVersion5PersistenceRepository.cs", 88);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaExportVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	private async Task<TolidFormulaExportBusiness> LoadDataAsync(DbDataReader dr, CancellationToken token, string tranName)
	{
		TolidFormulaExportBusiness item = new TolidFormulaExportBusiness(ContextGuid);
		try
		{
			if (dr["Guid"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["Guid"];
			}
			if (dr["MasterGuid"] != DBNull.Value)
			{
				item.MasterGuid = (Guid)dr["MasterGuid"];
			}
			if (dr["Quantity"] != DBNull.Value)
			{
				item.Quantity = (decimal)(double)dr["Quantity"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				item.Description = dr["Description"].ToString();
			}
			if (dr["ProductGuid"] != DBNull.Value)
			{
				await item.SetProductGuidAsync((Guid)dr["ProductGuid"], EnSystem.Mapper, token, tranName);
			}
			if (dr["DepotGuid"] != DBNull.Value)
			{
				item.DepotGuid = (Guid)dr["DepotGuid"];
			}
			if (dr["VGuid"] != DBNull.Value)
			{
				item.V2Guid = (Guid?)dr["VGuid"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaExportVersion5PersistenceRepository.cs", 113);
			throw;
		}
		return item;
	}
}
