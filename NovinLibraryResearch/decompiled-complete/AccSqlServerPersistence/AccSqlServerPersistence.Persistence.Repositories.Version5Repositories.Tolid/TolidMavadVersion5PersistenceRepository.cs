using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Tolid;

internal class TolidMavadVersion5PersistenceRepository : ITolidMavadRepository, IPersistenceMasterRepository<TolidMavadBusiness>, IDisposable
{
	private string ContextGuid;

	private PrdSerialPersistanceRepository _serialRepo;

	public ContextHolder DatabaseContext { get; private set; }

	public TolidMavadVersion5PersistenceRepository(ContextHolder dbContext, PrdSerialPersistanceRepository serial)
	{
		DatabaseContext = dbContext;
		ContextGuid = dbContext?.Context?.Guid;
		_serialRepo = serial;
	}

	public void Dispose()
	{
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<TolidMavadBusiness> items, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<TolidMavadBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		List<TolidMavadBusiness> list = new List<TolidMavadBusiness>();
		Acc_ModelContext c = (Acc_ModelContext)(await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidMavadVersion5PersistenceRepository.cs", 92));
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Tolid_Mavad_SelectRowsByOrderID", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidMavadVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<TolidMavadBusiness> list2 = list;
					list2.Add(await LoadDataAsync(dr, tranName));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidMavadVersion5PersistenceRepository.cs", 102);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidMavadVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	private async Task<TolidMavadBusiness> LoadDataAsync(DbDataReader dr, string tranName)
	{
		TolidMavadBusiness item = new TolidMavadBusiness(ContextGuid);
		try
		{
			if (dr["GUID"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["GUID"];
			}
			if (dr["Quantity"] != DBNull.Value)
			{
				item.Quantity = dr["Quantity"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["Price"] != DBNull.Value)
			{
				item.Price = (decimal)dr["Price"];
			}
			if (dr["Serial"] != DBNull.Value)
			{
				item.Serial = dr["Serial"].ToString();
			}
			if (dr["Description"] != DBNull.Value)
			{
				item.Description = dr["Description"].ToString();
			}
			if (dr["V2QT"] != DBNull.Value)
			{
				item.V2Qt = dr["V2QT"].ToString().ParseToDecimal(0m, 3);
			}
			if (dr["V2Price"] != DBNull.Value)
			{
				item.V2Price = (decimal?)dr["V2Price"];
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
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Tolid\\TolidMavadVersion5PersistenceRepository.cs", 130);
			throw;
		}
		return item;
	}
}
