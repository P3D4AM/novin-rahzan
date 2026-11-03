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

public class PardakhtBankVersion5PersistenceRepository : IPardakhtBankRepository, IPersistenceMasterRepository<PardakhtBankBusiness>, IDisposable
{
	private static ConcurrentDictionary<string, PardakhtBankVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, PardakhtBankVersion5PersistenceRepository>();

	private bool _isDisposed;

	public WebContextHolder Context => DbContext.Context;

	protected ContextHolder DbContext { get; private set; }

	public IItemAsyncMapper<PardakhtBank, PardakhtBankBusiness> MapFromDatabase { get; }

	public IItemAsyncMapper<PardakhtBankBusiness, PardakhtBank> MapToDatabase { get; }

	public bool IsDisposed => _isDisposed;

	public PardakhtBankVersion5PersistenceRepository(ContextHolder dbContext)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs"))
		{
			try
			{
				_instance.TryAdd(dbContext.Context.Guid, this);
				DbContext = dbContext;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 34);
				throw;
			}
		}
	}

	public static PardakhtBankVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		PardakhtBankVersion5PersistenceRepository pardakhtBankVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 49);
			throw;
		}
	}

	private WebPardakhtBank LoadData(DbDataReader dr)
	{
		WebPardakhtBank webPardakhtBank = new WebPardakhtBank();
		try
		{
			if (dr["ID"] != DBNull.Value)
			{
				webPardakhtBank.Id = (int)dr["ID"];
			}
			if (dr["PardakhtID"] != DBNull.Value)
			{
				webPardakhtBank.MasterId = (int)dr["PardakhtID"];
			}
			if (dr["BankMoeinGuid"] != DBNull.Value)
			{
				webPardakhtBank.MoeinGuid = (Guid)dr["BankMoeinGuid"];
			}
			if (dr["BankTafsilGuid"] != DBNull.Value)
			{
				webPardakhtBank.TafsilGuid = (Guid)dr["BankTafsilGuid"];
			}
			if (dr["Price"] != DBNull.Value)
			{
				webPardakhtBank.Price = (decimal)dr["Price"];
			}
			if (dr["Guid"] != DBNull.Value)
			{
				webPardakhtBank.Guid = Guid.Parse(dr["guid"].ToString());
			}
			if (dr["DateM"] != DBNull.Value)
			{
				webPardakhtBank.DateM = (DateTime)dr["DateM"];
			}
			if (dr["Number"] != DBNull.Value)
			{
				webPardakhtBank.Number = dr["Number"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 73);
			throw;
		}
		return webPardakhtBank;
	}

	public async Task<List<WebPardakhtBank>> GetAllAsync(int pardakhtId, CancellationToken token, string tranName = "")
	{
		List<WebPardakhtBank> ret = new List<WebPardakhtBank>();
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 83);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			List<WebPardakhtBank> result;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@PardakhtID", (object)pardakhtId)
				};
				using (DbDataReader dr = await c.ExecuteReaderAsync("sp_PardakhtBank_SelectRows", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs"))
				{
					res.AddReturnedValue(token, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 88);
					if (res.HasError)
					{
						result = ret;
					}
					else
					{
						while (await dr.ReadAsync())
						{
							ret.Add(LoadData(dr));
						}
						res.AddReturnedValue(token, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 92);
						dr.Close();
						if (!res.HasError)
						{
							goto end_IL_019b;
						}
						result = ret;
					}
					goto end_IL_00e0;
					end_IL_019b:;
				}
				goto end_IL_00cb;
				end_IL_00e0:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 98);
				throw;
			}
			return result;
			end_IL_00cb:;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs");
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebPardakhtBank item, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 111);
		try
		{
			if (item.Guid == Guid.Empty)
			{
				item.Guid = Guid.NewGuid();
			}
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@PardakhtID", (object)item.MasterId),
				new SqlParameter("@BankTafsilGuid", (object)item.TafsilGuid),
				new SqlParameter("@BankMoeinGuid", (object)item.MoeinGuid),
				new SqlParameter("@Price", (object)item.Price),
				new SqlParameter("@DateM", (object)item.DateM),
				new SqlParameter("@DateSH", (object)item.DateSh),
				new SqlParameter("@Number", (object)item.Number),
				new SqlParameter("@Guid", (object)item.Guid)
			};
			await baseModelContext.ExecuteNoneQueryAsync("sp_PardakhtBank_Insert", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs");
			return ret;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 130);
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 131);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(List<WebPardakhtBank> item, int pardakhtId, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			foreach (WebPardakhtBank item2 in item)
			{
				item2.MasterId = pardakhtId;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item2, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 144);
				if (res.HasError)
				{
					return res;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 150);
			throw;
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(Guid pardakhtGuid, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DbContext.GetContextAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 159);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@PardakhtGuid", (object)pardakhtGuid)
			};
			await baseModelContext.ExecuteNoneQueryAsync("sp_PardakhtBank_DeleteRows", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs");
			return ret;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 167);
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 168);
			throw;
		}
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				DbContext?.Dispose();
				DbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtBankVersion5PersistenceRepository.cs", 186);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtBankBusiness> items, Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<PardakhtBankBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
