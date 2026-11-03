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
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Reception;
using Core.Repository;
using EntityCache.Business;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Pardakht;

internal class PardakhtCheckMVersion5PersistenceRepository : PersistenceMasterRepository<PardakhtCheckMBusiness, PardakhtCheckM>, IPardakhtCheckMRepository, IPersistenceMasterRepository<PardakhtCheckMBusiness>, IDisposable
{
	private static ConcurrentDictionary<string, PardakhtCheckMVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, PardakhtCheckMVersion5PersistenceRepository>();

	private ReceptionCheckVersion5PersistenceRepository _receptionCheck;

	protected ContextHolder DbContext { get; private set; }

	public override IItemAsyncMapper<PardakhtCheckM, PardakhtCheckMBusiness> MapFromDatabase { get; }

	public override IItemAsyncMapper<PardakhtCheckMBusiness, PardakhtCheckM> MapToDatabase { get; }

	public PardakhtCheckMVersion5PersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs"))
		{
			try
			{
				_instance.TryAdd(dbContext.Context.Guid, this);
				DbContext = dbContext;
				_receptionCheck = new ReceptionCheckVersion5PersistenceRepository(dbContext);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 35);
				throw;
			}
		}
	}

	public static PardakhtCheckMVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		PardakhtCheckMVersion5PersistenceRepository pardakhtCheckMVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 49);
			throw;
		}
	}

	private async Task<WebPardakhtCheckM> LoadPardakhtCheckMAsync(DbDataReader dr, CancellationToken token, string tranName)
	{
		WebPardakhtCheckM item = new WebPardakhtCheckM();
		try
		{
			if (dr["IDCheck"] != DBNull.Value)
			{
				item.IdCheck = (int)dr["IDCheck"];
				if (dr["GuidCheck"] != DBNull.Value)
				{
					item.GuidCheck = (Guid)dr["GuidCheck"];
				}
				item.Price = (await _receptionCheck.GetAsync(item.IdCheck, token, tranName)).Price;
				item.Guid = Guid.NewGuid();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadPardakhtCheckMAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 74);
			throw;
		}
		return item;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(Guid guid, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DbContext.GetContextAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 82);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@pardakhtGuid", (object)guid)
			};
			await baseModelContext.ExecuteNoneQueryAsync("sp_PardakhtCheckM_DeleteRowBySanadID", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 90);
			res.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 91);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebPardakhtCheckM item, int pardakhtId, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 99);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@IDSanad", (object)pardakhtId),
				new SqlParameter("@GuidCheck", (object)item.GuidCheck),
				new SqlParameter("@CreditMoeinGuid", (object)LocalSettingBusiness.GetSetting(base.ContextGuid).CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception),
				new SqlParameter("@CreditTafsilGuid", (object)LocalSettingBusiness.GetSetting(base.ContextGuid).CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception)
			};
			await baseModelContext.ExecuteNoneQueryAsync("sp_PardakhtCheckM_Insert", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs");
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 113);
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 114);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> UpdateCheckStatusAsync(Guid checkGuid, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await DbContext.GetContextAsync(tranName, 60000, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 123);
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@GuidCheck", (object)checkGuid)
			};
			await obj.ExecuteNoneQueryAsync("sp_ReceptionCheck_UpdateCheckStatus", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 129);
			res.AddReturnedValue(ex, "", "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 130);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<WebPardakhtCheckM> item, int pardakhtId, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			foreach (WebPardakhtCheckM checkM in item)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(checkM, pardakhtId, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 141);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await UpdateCheckStatusAsync(checkM.GuidCheck, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 143);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 148);
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 149);
		}
		return ret;
	}

	public async Task<IEnumerable<WebPardakhtCheckM>> GetAllAsync(int sanadId, CancellationToken token, string tranName = "")
	{
		List<WebPardakhtCheckM> list = new List<WebPardakhtCheckM>();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 157);
		IEnumerable<WebPardakhtCheckM> result;
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_PardakhtCheckM_SelectWithIDSanad", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					List<WebPardakhtCheckM> list2 = list;
					list2.Add(await LoadPardakhtCheckMAsync(dr, token, tranName));
				}
				dr.Close();
				IEnumerable<WebPardakhtCheckM> enumerable = list?.GroupBy((WebPardakhtCheckM customer) => customer.GuidCheck)?.Select((IGrouping<Guid, WebPardakhtCheckM> group) => group.First());
				result = enumerable;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs", 171);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtCheckMVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	public Task<PardakhtCheckMBusiness> GetAsync(Guid chequeGuid, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtCheckMBusiness> items, PardakhtBusiness masterItem, short trycount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtCheckMBusiness> items, PardakhtBusiness masterItem, EnSystem system, short trycount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<PardakhtCheckMBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<PardakhtCheckMBusiness>> GetAllWithChequeGuidAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
