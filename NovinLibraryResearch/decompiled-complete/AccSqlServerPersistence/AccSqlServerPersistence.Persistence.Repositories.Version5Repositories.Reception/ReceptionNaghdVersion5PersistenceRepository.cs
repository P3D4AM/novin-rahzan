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

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Reception;

public class ReceptionNaghdVersion5PersistenceRepository : IReceptionNaghdRepository, IPersistenceMasterRepository<ReceptionNaghdBusiness>, IDisposable
{
	private bool _isDisposed;

	private static ConcurrentDictionary<string, ReceptionNaghdVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, ReceptionNaghdVersion5PersistenceRepository>();

	public ContextHolder DatabaseContext { get; private set; }

	public IItemAsyncMapper<ReceptionNaghd, ReceptionNaghdBusiness> MapFromDatabase { get; }

	public IItemAsyncMapper<ReceptionNaghdBusiness, ReceptionNaghd> MapToDatabase { get; }

	public bool IsDisposed => _isDisposed;

	public ReceptionNaghdVersion5PersistenceRepository(ContextHolder dbContext)
	{
		_instance.TryAdd(dbContext.Context.Guid, this);
		DatabaseContext = dbContext;
	}

	public static ReceptionNaghdVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		ReceptionNaghdVersion5PersistenceRepository receptionNaghdVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 38);
			throw;
		}
	}

	private WebReceptionNaghd LoadReceptionNaghd(DbDataReader dr)
	{
		WebReceptionNaghd webReceptionNaghd = new WebReceptionNaghd();
		try
		{
			if (dr["ID"] != DBNull.Value)
			{
				webReceptionNaghd.Id = (int)dr["ID"];
			}
			if (dr["SandoghMoeinGuid"] != DBNull.Value)
			{
				webReceptionNaghd.MoeinGuid = (Guid)dr["SandoghMoeinGuid"];
			}
			if (dr["SandoghTafsilGuid"] != DBNull.Value)
			{
				webReceptionNaghd.TafsilGuid = (Guid)dr["SandoghTafsilGuid"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				webReceptionNaghd.Price = (decimal)dr["Price"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				webReceptionNaghd.DateM = (DateTime)dr["DateM"];
			}
			if (dr["GUID"] != DBNull.Value)
			{
				webReceptionNaghd.Guid = (Guid)dr["GUID"];
			}
			if (dr["DESCRIPTION"] != DBNull.Value)
			{
				webReceptionNaghd.Description = dr["DESCRIPTION"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadReceptionNaghd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 63);
			throw;
		}
		return webReceptionNaghd;
	}

	public async Task<List<WebReceptionNaghd>> GetAllNaghdAsync(int sanadId, string tranName = "")
	{
		List<WebReceptionNaghd> list = new List<WebReceptionNaghd>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllNaghdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 71);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ReceptionNaghd_SelectRowsByIDSanad", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllNaghdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadReceptionNaghd(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllNaghdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 82);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllNaghdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 102);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<ReceptionNaghdBusiness> items, Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<ReceptionNaghdBusiness> items, int masterId, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 114);
		try
		{
			foreach (ReceptionNaghdBusiness item in items)
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@DateM", (object)item.DateM),
					new SqlParameter("@IDSanad", (object)masterId),
					new SqlParameter("@SandoghTafsilGuid", (object)item.TafsilGuid),
					new SqlParameter("@SandoghMoeinGuid", (object)item.MoeinGuid),
					new SqlParameter("@Price", (object)item.Price),
					new SqlParameter("@Description", (object)(item.Description ?? "")),
					new SqlParameter("@GUID", (object)item.Guid)
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_ReceptionNaghd_Insert", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs"), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 129);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 134);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 135);
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(int masterId, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 148);
		try
		{
			List<WebReceptionNaghd> list = await GetAllNaghdAsync(masterId, tranName);
			if (list == null || list.Count <= 0)
			{
				return res;
			}
			foreach (WebReceptionNaghd item in list)
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID", (object)item.Id)
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_ReceptionNaghd_DeleteRow", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs"), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 156);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 161);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionNaghdVersion5PersistenceRepository.cs", 162);
		}
		return res;
	}

	public Task<List<ReceptionNaghdBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
