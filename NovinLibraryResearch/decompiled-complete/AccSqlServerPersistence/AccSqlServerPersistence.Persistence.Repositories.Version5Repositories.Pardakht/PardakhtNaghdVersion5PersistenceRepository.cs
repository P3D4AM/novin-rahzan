using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Details;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Pardakht;

internal class PardakhtNaghdVersion5PersistenceRepository : PersistenceMasterRepository<PardakhtNaghdBusiness, PardakhtNaghd>, IPardakhtNaghdRepository, IPersistenceMasterRepository<PardakhtNaghdBusiness>, IDisposable
{
	private static ConcurrentDictionary<string, PardakhtNaghdVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, PardakhtNaghdVersion5PersistenceRepository>();

	public ContextHolder DatabaseContext { get; private set; }

	public override IItemAsyncMapper<PardakhtNaghd, PardakhtNaghdBusiness> MapFromDatabase { get; }

	public override IItemAsyncMapper<PardakhtNaghdBusiness, PardakhtNaghd> MapToDatabase { get; }

	public PardakhtNaghdVersion5PersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
		_instance.TryAdd(dbContext.Context.Guid, this);
		DatabaseContext = dbContext;
	}

	public static PardakhtNaghdVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		PardakhtNaghdVersion5PersistenceRepository pardakhtNaghdVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 35);
			throw;
		}
	}

	private WebPardakhtNaghd LoadPardakhtNaghd(DbDataReader dr)
	{
		WebPardakhtNaghd webPardakhtNaghd = new WebPardakhtNaghd();
		try
		{
			if (dr["ID"] != DBNull.Value)
			{
				webPardakhtNaghd.ID = (int)dr["ID"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				webPardakhtNaghd.Price = (decimal)dr["Price"];
			}
			if (dr["Guid"] != DBNull.Value)
			{
				webPardakhtNaghd.Guid = (Guid)dr["Guid"];
			}
			if (dr["SandoghTafsilGuid"] != DBNull.Value)
			{
				webPardakhtNaghd.TafsilGuid = (Guid)dr["SandoghTafsilGuid"];
			}
			if (dr["SandoghMoeinGuid"] != DBNull.Value)
			{
				webPardakhtNaghd.MoeinGuid = (Guid)dr["SandoghMoeinGuid"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadPardakhtNaghd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 56);
			throw;
		}
		return webPardakhtNaghd;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(Guid guid, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 64);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@PardakhtGuid", (object)guid)
			};
			await baseModelContext.ExecuteNoneQueryAsync("sp_PardakhtNaghd_DeleteRowBySanadID", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 72);
			res.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 73);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebPardakhtNaghd item, int pardakhtId, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 80);
		try
		{
			if (item.Guid == Guid.Empty)
			{
				item.Guid = Guid.NewGuid();
			}
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@IDSanad", (object)pardakhtId),
				new SqlParameter("@SandoghMoeinGuid", (object)item.MoeinGuid),
				new SqlParameter("@SandoghTafsilGuid", (object)item.TafsilGuid),
				new SqlParameter("@Price", (object)item.Price),
				new SqlParameter("@GUID", (object)item.Guid)
			};
			await baseModelContext.ExecuteNoneQueryAsync("sp_PardakhtNaghd_Insert", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs");
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 96);
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 97);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<WebPardakhtNaghd> item, int pardakhtId, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			foreach (WebPardakhtNaghd item2 in item)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item2, pardakhtId, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 108);
				if (ret.HasError)
				{
					return ret;
				}
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 114);
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 115);
		}
		return ret;
	}

	public async Task<IEnumerable<WebPardakhtNaghd>> GetAllAsync(int sanadId, string tranName = "")
	{
		List<WebPardakhtNaghd> list = new List<WebPardakhtNaghd>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 123);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID", (object)sanadId)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_PardakhtNaghd_SelectWithIDSanad", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					WebPardakhtNaghd webPardakhtNaghd = LoadPardakhtNaghd(dr);
					if (webPardakhtNaghd.Guid == Guid.Empty)
					{
						webPardakhtNaghd.Guid = Guid.NewGuid();
					}
					list.Add(webPardakhtNaghd);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs", 138);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtNaghdVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}
}
