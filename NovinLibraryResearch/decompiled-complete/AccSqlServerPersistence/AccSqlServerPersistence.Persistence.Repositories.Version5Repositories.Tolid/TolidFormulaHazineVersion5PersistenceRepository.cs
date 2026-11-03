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

internal class TolidFormulaHazineVersion5PersistenceRepository : ITolidFormulaHazineRepository, IPersistenceMasterRepository<TolidFormulaHazineBusiness>, IDisposable
{
	private string ContextGuid;

	public ContextHolder DatabaseContext { get; private set; }

	public TolidFormulaHazineVersion5PersistenceRepository(ContextHolder dbContext)
	{
		DatabaseContext = dbContext;
		ContextGuid = dbContext.Context.Guid;
	}

	public void Dispose()
	{
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<TolidFormulaHazineBusiness> items, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<TolidFormulaHazineBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		List<TolidFormulaHazineBusiness> list = new List<TolidFormulaHazineBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaHazineVersion5PersistenceRepository.cs", 75);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@MasterGuid", (object)masterGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Tolid_Formula_Hazine_SelectRowsByMaster", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaHazineVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaHazineVersion5PersistenceRepository.cs", 85);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaHazineVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	private TolidFormulaHazineBusiness LoadData(DbDataReader dr)
	{
		TolidFormulaHazineBusiness tolidFormulaHazineBusiness = new TolidFormulaHazineBusiness(ContextGuid);
		try
		{
			if (dr["Guid"] != DBNull.Value)
			{
				tolidFormulaHazineBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["MasterGuid"] != DBNull.Value)
			{
				tolidFormulaHazineBusiness.MasterGuid = (Guid)dr["MasterGuid"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				tolidFormulaHazineBusiness.Price = (decimal)dr["Price"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				tolidFormulaHazineBusiness.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				tolidFormulaHazineBusiness.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["Type"] != DBNull.Value)
			{
				tolidFormulaHazineBusiness.Type = (HazineType)dr["Type"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaHazineVersion5PersistenceRepository.cs", 109);
			throw;
		}
		return tolidFormulaHazineBusiness;
	}
}
