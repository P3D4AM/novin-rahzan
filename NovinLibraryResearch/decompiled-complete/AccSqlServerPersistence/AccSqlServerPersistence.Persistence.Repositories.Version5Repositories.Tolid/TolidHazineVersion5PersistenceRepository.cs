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

internal class TolidHazineVersion5PersistenceRepository : ITolidHazineRepository, IPersistenceMasterRepository<TolidHazineBusiness>, IDisposable
{
	private string ContextGuid;

	public ContextHolder DatabaseContext { get; private set; }

	public TolidHazineVersion5PersistenceRepository(ContextHolder dbContext)
	{
		DatabaseContext = dbContext;
		ContextGuid = dbContext?.Context?.Guid;
	}

	public void Dispose()
	{
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<TolidHazineBusiness> items, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<TolidHazineBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		List<TolidHazineBusiness> list = new List<TolidHazineBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidHazineVersion5PersistenceRepository.cs", 78);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Tolid_Hazine_SelectRowsByOrderID", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidHazineVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidHazineVersion5PersistenceRepository.cs", 88);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidHazineVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	private TolidHazineBusiness LoadData(DbDataReader dr)
	{
		TolidHazineBusiness tolidHazineBusiness = new TolidHazineBusiness(ContextGuid);
		try
		{
			if (dr["GUID"] != DBNull.Value)
			{
				tolidHazineBusiness.Guid = (Guid)dr["GUID"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				tolidHazineBusiness.Price = (decimal)dr["Price"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				tolidHazineBusiness.Description = dr["Description"].ToString();
			}
			if (dr["Type"] != DBNull.Value)
			{
				tolidHazineBusiness.Type = (HazineType)dr["Type"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				tolidHazineBusiness.TafsilGuid = (Guid?)dr["TafsilGuid"];
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				tolidHazineBusiness.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidHazineVersion5PersistenceRepository.cs", 112);
			throw;
		}
		return tolidHazineBusiness;
	}
}
