using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Details;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Reception;

public class ReceptionCheckVersion5PersistenceRepository : IReceptionCheckRepository, IPersistenceMasterRepository<ReceptionCheckBusiness>, IDisposable
{
	private static ConcurrentDictionary<string, ReceptionCheckVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, ReceptionCheckVersion5PersistenceRepository>();

	protected ContextHolder DatabaseContext { get; private set; }

	public WebContextHolder Context => DatabaseContext.Context;

	public string ContextGuid => Context.Guid;

	public IItemAsyncMapper<ReceptionCheck, ReceptionCheckBusiness> MapFromDatabase { get; }

	public IItemAsyncMapper<ReceptionCheckBusiness, ReceptionCheck> MapToDatabase { get; }

	public ReceptionCheckVersion5PersistenceRepository(ContextHolder dbContext)
	{
		_instance.TryAdd(dbContext.Context.Guid, this);
		DatabaseContext = dbContext;
	}

	public static ReceptionCheckVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		ReceptionCheckVersion5PersistenceRepository receptionCheckVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 44);
			throw;
		}
	}

	public async Task<string> NextPoshtNomreAsync(int checkCount, CancellationToken token, string tranName)
	{
		string res = "";
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "NextPoshtNomreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 55);
		try
		{
			_ = 1;
			try
			{
				object obj = await c.ExecuteScalarAsync<object>("sp_ReceptionCheck_NextPoshtNomre", null, tranName, default(CancellationToken), 30, "NextPoshtNomreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
				if (obj != null)
				{
					res = obj.ToString();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NextPoshtNomreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 63);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("NextPoshtNomreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<IEnumerable<ReceptionCheckBusiness>> GetAllAsync(string search, CancellationToken token, ENUMCheck? status, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReceptionCheckBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		WebReceptionCheck res = new WebReceptionCheck();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 81);
		try
		{
			int num;
			_ = num - 1;
			_ = 3;
			try
			{
				List<SqlParameter> list = new List<SqlParameter>();
				List<SqlParameter> list2 = list;
				list2.Add(new SqlParameter("@ID", (object)(await Guid_To_IdAsync(guid, tranName))));
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ReceptionCheck_SelectRow", list, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = await LoadReceptionCheckAsync(dr, token, tranName);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 92);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
		}
		return await ReceptionCheckMapper.Instance.MapAsync(ContextGuid, res, null, tranName, token);
	}

	public async Task<IEnumerable<ReceptionCheckBusiness>> GetAllAsync(ENUMCheck? status, CancellationToken token, string tranName)
	{
		List<WebReceptionCheck> res = new List<WebReceptionCheck>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 105);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> list = new List<SqlParameter>();
				if (status.HasValue)
				{
					list.Add(new SqlParameter("@CheckStatus", (object)(short)status.Value));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ReceptionCheck_GetAll", list, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<WebReceptionCheck> list2 = res;
					list2.Add(await LoadReceptionCheckAsync(dr, token, tranName));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 117);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
		}
		return await ReceptionCheckMapper.Instance.MapListAsync(ContextGuid, res, null, tranName, token);
	}

	public async Task<IEnumerable<ReceptionCheckBusiness>> GetAllAsync(string number, ENUMCheck? status, CancellationToken token, string tranName)
	{
		List<WebReceptionCheck> res = new List<WebReceptionCheck>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 130);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@number", (object)number)
				};
				if (status.HasValue)
				{
					list.Add(new SqlParameter("@st", (object)(short)status.Value));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ReceptionCheck_GetByNumber", list, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<WebReceptionCheck> list2 = res;
					list2.Add(await LoadReceptionCheckAsync(dr, token, tranName));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 142);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
		}
		return await ReceptionCheckMapper.Instance.MapListAsync(ContextGuid, res, null, tranName, token);
	}

	private async Task<WebReceptionCheck> LoadReceptionCheckAsync(DbDataReader dr, CancellationToken token, string tranName)
	{
		WebReceptionCheck item = new WebReceptionCheck();
		try
		{
			if (dr["ID"] != DBNull.Value)
			{
				item.Id = (int)dr["ID"];
			}
			if (dr["IDSanad"] != DBNull.Value)
			{
				item.SanadId = (int)dr["IDSanad"];
			}
			if (dr["Aval2re"] != DBNull.Value)
			{
				item.Aval2re = (bool)dr["Aval2re"];
			}
			if (dr["Number"] != DBNull.Value)
			{
				item.Number = dr["Number"].ToString();
			}
			if (dr["BankName"] != DBNull.Value)
			{
				item.BankName = dr["BankName"].ToString();
			}
			if (dr["Price"] != DBNull.Value)
			{
				item.Price = (decimal)dr["Price"];
			}
			if (dr["Personal"] != DBNull.Value)
			{
				item.Personal = (bool)dr["Personal"];
			}
			if (dr["Date"] != DBNull.Value)
			{
				item.Date = (DateTime)dr["Date"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				item.Status = (ENUMCheck)(short)dr["Status"];
			}
			if (dr["HesabNum"] != DBNull.Value)
			{
				item.HesabNum = dr["HesabNum"].ToString();
			}
			if (dr["PoshtNomreh"] != DBNull.Value)
			{
				item.PoshtNomreh = dr["PoshtNomreh"].ToString().ParseToLong(0L);
			}
			if (dr["DESCRIPTION"] != DBNull.Value)
			{
				item.Description = dr["DESCRIPTION"].ToString();
			}
			if (dr["Modified"] != DBNull.Value)
			{
				item.Modified = (DateTime)dr["Modified"];
			}
			if (dr["GUID"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["GUID"];
			}
			if (dr["A2reReceptionDate"] != DBNull.Value)
			{
				item.A2re_ReceptionDate = (DateTime)dr["A2reReceptionDate"];
			}
			if (dr["A2reTafsilGuid"] != DBNull.Value)
			{
				item.A2re_PardazandeTafsilGuid = (Guid)dr["A2reTafsilGuid"];
			}
			if (dr["A2reMoeinGuid"] != DBNull.Value)
			{
				item.A2re_PardazandeMoeinGuid = (Guid)dr["A2reMoeinGuid"];
			}
			if (dr["DebitTafsilGuid"] != DBNull.Value)
			{
				item.DebitTafsilGuid = (Guid)dr["DebitTafsilGuid"];
			}
			if (dr["DebitMoeinGuid"] != DBNull.Value)
			{
				item.DebitMoeinGuid = (Guid)dr["DebitMoeinGuid"];
			}
			ReceptionVersion5PersistenceRepository instance = ReceptionVersion5PersistenceRepository.GetInstance(ContextGuid);
			WebReceptionCheck webReceptionCheck = item;
			webReceptionCheck.MasterGuid = await instance.Id_To_GuidAsync(item.SanadId, token, tranName);
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadReceptionCheckAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 182);
			throw;
		}
	}

	public async Task<List<WebReceptionCheck>> GetAllChecksAsync(int sanadId, CancellationToken token, string tranName = "")
	{
		List<WebReceptionCheck> list = new List<WebReceptionCheck>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllChecksAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 191);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID", (object)sanadId)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ReceptionCheck_SelectRowsWithIDSanad", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllChecksAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<WebReceptionCheck> list2 = list;
					list2.Add(await LoadReceptionCheckAsync(dr, token, tranName));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllChecksAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 202);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllChecksAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	public async Task<WebReceptionCheck> GetAsync(int id, CancellationToken token, string tranName = "")
	{
		WebReceptionCheck res = null;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 216);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID", (object)id)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ReceptionCheck_SelectRow", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = await LoadReceptionCheckAsync(dr, token, tranName);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 227);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public void Dispose()
	{
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<ReceptionCheckBusiness> items, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			ReceptionBusiness master = await ReceptionBusiness.GetAsync(ContextGuid, masterGuid, EnSystem.None, null, fromDatabase: true, token, tranName);
			if (master == null)
			{
				string contextGuid = ContextGuid;
				Guid? guid = masterGuid;
				master = await ReceptionBusiness.GenerateNewWithDefaultsAsync(contextGuid, EnSystem.None, null, "", guid, null, token);
			}
			if (items.FirstOrDefault().A2re_PardazandeTafsilGuid.HasValue)
			{
				await master.SetTafsilGuidAsync(Guid.Empty, EnSystem.None, token, tranName);
				master.MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein15;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(items, master, null, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 256);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 260);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 261);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient serverStatus, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 269);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid),
					new SqlParameter("@st", (object)(short)serverStatus)
				};
				await c.ExecuteNoneQueryAsync("sp_ViewCheckM_UpdateServerStatus", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 281);
				res.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 282);
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<ReceptionCheckBusiness> items, ReceptionBusiness master, int? masterId, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 295);
		try
		{
			foreach (ReceptionCheckBusiness item in items)
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@Number", (object)item.Number),
					new SqlParameter("@BankName", (object)item.BankName),
					new SqlParameter("@Price", (object)item.Price),
					new SqlParameter("@Personal", (object)item.Personal),
					new SqlParameter("@Date", (object)item.Date),
					new SqlParameter("@HesabNum", (object)item.HesabNum),
					new SqlParameter("@PoshtNomreh", (object)item.PoshtNomreh),
					new SqlParameter("@Description", (object)item.Description),
					new SqlParameter("@CreditMoeinGuid", (object)master.MoeinGuid),
					new SqlParameter("@DebitMoeinGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception),
					new SqlParameter("@DebitTafsilGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception),
					new SqlParameter("@Aval2re", (object)item.Aval2re),
					new SqlParameter("@GUID", (object)item.Guid),
					new SqlParameter("@CashierGuid", (object)master.UserGuid)
				};
				if (master.TafsilGuid != Guid.Empty)
				{
					list.Add(new SqlParameter("@CreditTafsilGuid", (object)master.TafsilGuid));
				}
				if (masterId.HasValue)
				{
					list.Add(new SqlParameter("@IDSanad", (object)masterId));
				}
				if (item.A2re_ReceptionDate.HasValue)
				{
					list.Add(new SqlParameter("@A2re_ReceptionDate", (object)item.A2re_ReceptionDate));
				}
				if (item.A2re_PardazandeMoeinGuid.HasValue)
				{
					list.Add(new SqlParameter("@A2reMoeinGuid", (object)item.A2re_PardazandeMoeinGuid));
				}
				if (item.A2re_PardazandeTafsilGuid.HasValue)
				{
					list.Add(new SqlParameter("@A2reTafsilGuid", (object)item.A2re_PardazandeTafsilGuid));
				}
				await c.ExecuteNoneQueryAsync("sp_ReceptionCheck_Save", list, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 329);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 330);
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<ReceptionCheckBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<int> Guid_To_IdAsync(Guid guid, string tranName = "")
	{
		int id = 0;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 344);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@AsGuid", (object)guid),
					new SqlParameter("@AsType", (object)(short)49)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Guid_To_Id", parameters, tranName, default(CancellationToken), 30, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
				if (obj != null)
				{
					id = obj.ToString().ParseToInt();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs", 357);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionCheckVersion5PersistenceRepository.cs");
			}
		}
		return id;
	}

	public Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllAsync(ReceptionChequeFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReceptionCheckBusiness> GetAsync(string number, CancellationToken token, ENUMCheck? status, string tranName)
	{
		throw new NotImplementedException();
	}
}
