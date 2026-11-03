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

internal class TolidFormulaMavadVersion5PersistenceRepository : ITolidFormulaMavadRepository, IPersistenceMasterRepository<TolidFormulaMavadBusiness>, IDisposable
{
	private string ContextGuid;

	public ContextHolder DatabaseContext { get; private set; }

	public TolidFormulaMavadVersion5PersistenceRepository(ContextHolder dbContext)
	{
		DatabaseContext = dbContext;
		ContextGuid = dbContext.Context.Guid;
	}

	public void Dispose()
	{
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<TolidFormulaMavadBusiness> items, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<TolidFormulaMavadBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		List<TolidFormulaMavadBusiness> list = new List<TolidFormulaMavadBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaMavadVersion5PersistenceRepository.cs", 78);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Tolid_Formula_Mavad_SelectRowsByMaster", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaMavadVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<TolidFormulaMavadBusiness> list2 = list;
					list2.Add(await LoadDataAsync(dr, tranName));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaMavadVersion5PersistenceRepository.cs", 88);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaMavadVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	private async Task<TolidFormulaMavadBusiness> LoadDataAsync(DbDataReader dr, string tranName)
	{
		TolidFormulaMavadBusiness item = new TolidFormulaMavadBusiness(ContextGuid);
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
				await item.SetProductGuid((Guid)dr["ProductGuid"], EnSystem.Mapper, tranName);
			}
			if (dr["DepotGuid"] != DBNull.Value)
			{
				await item.SetDepotGuid((Guid)dr["DepotGuid"], EnSystem.Mapper, tranName);
			}
			if (dr["VGuid"] != DBNull.Value)
			{
				item.V2Guid = (Guid?)dr["VGuid"];
			}
			if (dr["Serial"] != DBNull.Value)
			{
				item.Serial = dr["Serial"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidFormulaMavadVersion5PersistenceRepository.cs", 114);
			throw;
		}
		return item;
	}
}
