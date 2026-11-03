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
using Core.Repository;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Pardakht;

public class PardakhtCheckShVersion5PersistenceRepository : IPardakhtCheckShRepository, IPersistenceMasterRepository<PardakhtCheckShBusiness>, IDisposable
{
	private static ConcurrentDictionary<string, PardakhtCheckShVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, PardakhtCheckShVersion5PersistenceRepository>();

	protected ContextHolder DbContext { get; private set; }

	public WebContextHolder Context => DbContext.Context;

	public string ContextGuid => Context.Guid;

	public IItemAsyncMapper<PardakhtCheckSh, PardakhtCheckShBusiness> MapFromDatabase { get; }

	public IItemAsyncMapper<PardakhtCheckShBusiness, PardakhtCheckSh> MapToDatabase { get; }

	public PardakhtCheckShVersion5PersistenceRepository(ContextHolder dbContext)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs"))
		{
			try
			{
				_instance.TryAdd(dbContext.Context.Guid, this);
				DbContext = dbContext;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 42);
				throw;
			}
		}
	}

	public static PardakhtCheckShVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		PardakhtCheckShVersion5PersistenceRepository pardakhtCheckShVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 56);
			throw;
		}
	}

	public async Task<List<PardakhtCheckShBusiness>> GetAllAsync(Guid guid, string number, string tranName)
	{
		List<WebPardakhtCheckSh> res = new List<WebPardakhtCheckSh>();
		BaseModelContext baseModelContext = await DbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 66);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@Guid", (object)guid),
				new SqlParameter("@number", (object)(number ?? ""))
			};
			using DbDataReader dr = await baseModelContext.ExecuteReaderAsync("sp_PardakhtCheckSH_GetAllByNumber", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
			while (await dr.ReadAsync())
			{
				res.Add(LoadData(dr));
			}
			dr.Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 80);
			throw;
		}
		return await PardakhtCheckShMapper.Instance.MapListAsync(ContextGuid, res, null, tranName);
	}

	public Task<List<PardakhtCheckShBusiness>> GetAllAsync(string search, CancellationToken token, short? Status, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient serverStatus, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 93);
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
				await c.ExecuteNoneQueryAsync("sp_ViewCheckSh_UpdateServerStatus", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 105);
				res.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 106);
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<PardakhtCheckShBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		WebPardakhtCheckSh res = null;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 119);
		try
		{
			int num = await Guid_To_IdAsync(guid, tranName);
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@ID", (object)num)
			};
			using DbDataReader dr = await c.ExecuteReaderAsync("sp_PardakhtCheckSH_SelectRow", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
			if (await dr.ReadAsync())
			{
				res = LoadData(dr);
			}
			dr.Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 130);
			throw;
		}
		return await PardakhtCheckShMapper.Instance.MapAsync(ContextGuid, res, null, tranName, token);
	}

	private WebPardakhtCheckSh LoadData(DbDataReader dr)
	{
		WebPardakhtCheckSh webPardakhtCheckSh = new WebPardakhtCheckSh();
		try
		{
			if (dr["ID"] != DBNull.Value)
			{
				webPardakhtCheckSh.Id = (int)dr["ID"];
			}
			if (dr["IDSanad"] != DBNull.Value)
			{
				webPardakhtCheckSh.IDSanad = (int)dr["IDSanad"];
			}
			if (dr["Aval2re"] != DBNull.Value)
			{
				webPardakhtCheckSh.Aval2re = (bool)dr["Aval2re"];
			}
			if (dr["Number"] != DBNull.Value)
			{
				webPardakhtCheckSh.Number = dr["Number"].ToString();
			}
			if (dr["BankTafsilGuid"] != DBNull.Value)
			{
				webPardakhtCheckSh.BankTafsilGuid = (Guid)dr["BankTafsilGuid"];
			}
			if (dr["CreditMoeinGuid"] != DBNull.Value)
			{
				webPardakhtCheckSh.CreditMoeinGuid = (Guid)dr["CreditMoeinGuid"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				webPardakhtCheckSh.Price = (decimal)dr["Price"];
			}
			if (dr["Statuse"] != DBNull.Value)
			{
				webPardakhtCheckSh.Status = (short)dr["Statuse"];
			}
			if (dr["DateSarresid"] != DBNull.Value)
			{
				webPardakhtCheckSh.DateSaresid = (DateTime)dr["DateSarresid"];
			}
			if (dr["DESCRIPTION"] != DBNull.Value)
			{
				webPardakhtCheckSh.Description = dr["DESCRIPTION"].ToString();
			}
			if (dr["Modified"] != DBNull.Value)
			{
				webPardakhtCheckSh.Modified = (DateTime)dr["Modified"];
			}
			if (dr["GUID"] != DBNull.Value)
			{
				webPardakhtCheckSh.Guid = (Guid)dr["GUID"];
			}
			if (dr["PardakhtDate"] != DBNull.Value)
			{
				webPardakhtCheckSh.PardakhtDate = (DateTime)dr["PardakhtDate"];
			}
			if (dr["MasterGuid"] != DBNull.Value)
			{
				webPardakhtCheckSh.MasterGuid = (Guid)dr["MasterGuid"];
			}
			if (webPardakhtCheckSh.Aval2re)
			{
				if (dr["GirandehTafsilGuid"] != DBNull.Value)
				{
					webPardakhtCheckSh.A2re_TafsilGuid = (Guid)dr["GirandehTafsilGuid"];
				}
				if (dr["GirandehMoeinGuid"] != DBNull.Value)
				{
					webPardakhtCheckSh.A2re_MoeinGuid = (Guid)dr["GirandehMoeinGuid"];
				}
			}
			else
			{
				if (dr["GirandehTafsilGuid"] != DBNull.Value)
				{
					webPardakhtCheckSh.DebitTafsilGuid = (Guid)dr["GirandehTafsilGuid"];
				}
				if (dr["GirandehMoeinGuid"] != DBNull.Value)
				{
					webPardakhtCheckSh.DebitMoeinGuid = (Guid)dr["GirandehMoeinGuid"];
				}
			}
			if (dr["A2reDebitMoeinGuid"] != DBNull.Value)
			{
				webPardakhtCheckSh.DebitA2reMoeinGuid = (Guid)dr["A2reDebitMoeinGuid"];
			}
			if (dr["A2reDebitTafsilGuid"] != DBNull.Value)
			{
				webPardakhtCheckSh.DebitA2reTafsiGuid = (Guid)dr["A2reDebitTafsilGuid"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 170);
			throw;
		}
		return webPardakhtCheckSh;
	}

	public async Task<WebPardakhtCheckSh> GetAsync(int id, string connectionString, string tranName = "")
	{
		WebPardakhtCheckSh res = null;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 179);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID", (object)id)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_PardakhtCheckSH_SelectRow", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = LoadData(dr);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 189);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<List<WebPardakhtCheckSh>> GetAllWithSanadIdAsync(int sanadId, string tranName = "")
	{
		List<WebPardakhtCheckSh> list = new List<WebPardakhtCheckSh>();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAllWithSanadIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 203);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_PardakhtCheckSH_SelectWithIDSanad", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllWithSanadIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllWithSanadIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 214);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllWithSanadIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
			}
		}
		return list;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<WebPardakhtCheckSh> item, int? pardakhtId, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			foreach (WebPardakhtCheckSh item2 in item)
			{
				item2.IDSanad = pardakhtId;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item2, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 233);
				if (res.HasError)
				{
					return res;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 239);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 240);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebPardakhtCheckSh item, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 248);
		try
		{
			PardakhtCheckShBusiness obj = await GetAsync(item.Guid, default(CancellationToken), tranName);
			if (item.Aval2re && item.DebitMoeinGuid == Guid.Empty)
			{
				item.DebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein15;
			}
			List<SqlParameter> list = new List<SqlParameter>
			{
				new SqlParameter("@Number", (object)item.Number),
				new SqlParameter("@BankTafsilGuid", (object)item.BankTafsilGuid),
				new SqlParameter("@Price", (object)item.Price),
				new SqlParameter("@DateSarresid", (object)item.DateSaresid),
				new SqlParameter("@PardakhtDate", (object)item.PardakhtDate),
				new SqlParameter("@DebitMoeinGuid", (object)item.DebitMoeinGuid),
				new SqlParameter("@DebitTafsilGuid", (object)item.DebitTafsilGuid),
				new SqlParameter("@CreditMoeinGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.CheckSHCoding.Debit_MoeinGuid_Pardakht),
				new SqlParameter("@CreditTafsilGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.CheckSHCoding.Debit_TafsilGuid_Pardakht)
			};
			if (item.A2re_MoeinGuid.HasValue)
			{
				list.Add(new SqlParameter("@A2reMoeinGuid", (object)item.A2re_MoeinGuid));
			}
			if (item.A2re_TafsilGuid.HasValue)
			{
				list.Add(new SqlParameter("@A2reTafsilGuid", (object)item.A2re_TafsilGuid));
			}
			string spName;
			if (obj == null)
			{
				list.Add(new SqlParameter("@GUID", (object)item.Guid));
				if (item.IDSanad.HasValue)
				{
					list.Add(new SqlParameter("@IDSanad", (object)item.IDSanad));
				}
				list.Add(new SqlParameter("@Aval2re", (object)item.Aval2re));
				spName = "[sp_PardakhtCheckSH_Insert]";
			}
			else
			{
				list.Add(new SqlParameter("@Guid", (object)item.Guid));
				spName = "[sp_PardakhtCheckSH_Update]";
			}
			await c.ExecuteNoneQueryAsync(spName, list, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 286);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 287);
		}
		return ret;
	}

	public void Dispose()
	{
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtCheckShBusiness> items, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			PardakhtCheckShBusiness item = items.FirstOrDefault();
			if ((!item.A2re_MoeinGuid.HasValue || item.A2re_MoeinGuid.Value == Guid.Empty) && item.A2re_TafsilGuid.HasValue)
			{
				TafsilGroupBusiness tafsilGroupBusiness = (await ((await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, item.A2re_TafsilGuid.Value, "", token, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs")) ?? throw new NullReferenceException("Tafsil is null!!")).GetTafsilGroupAsync(EnSystem.Mapper, token, tranName)) ?? throw new NullReferenceException("tafsilGroup is null!!");
				item.A2re_MoeinGuid = tafsilGroupBusiness.MoeinPardakhtGuid;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(await PardakhtCheckShMapper.Instance.MapListAsync(ContextGuid, items, new List<WebPardakhtCheckSh>(), tranName, token), null, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 310);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 314);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 315);
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<PardakhtCheckShBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<int> Guid_To_IdAsync(Guid guid, string tranName = "")
	{
		int id = 0;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 333);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@AsGuid", (object)guid),
					new SqlParameter("@AsType", (object)(short)102)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Guid_To_Id", parameters, tranName, default(CancellationToken), 30, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
				if (obj != null)
				{
					id = obj.ToString().ParseToInt();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs", 347);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckShVersion5PersistenceRepository.cs");
			}
		}
		return id;
	}

	public Task<IEnumerable<PardakhtCheckShReportBusiness>> GetAllAsync(PardakhtChequeFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
