using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Sanad;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Pardakht;

public class PardakhtVersion5PersistenceRepository : IPardakhtRepository, IEntityRepository<PardakhtBusiness, PardakhtReportBusiness>, IEditableDelivery, IDisposable
{
	private bool _isDisposed;

	private static ConcurrentDictionary<string, PardakhtVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, PardakhtVersion5PersistenceRepository>();

	private PardakhtBankVersion5PersistenceRepository pardakhtBankV5Repo;

	private PardakhtCheckMVersion5PersistenceRepository pardakhtCheckMV5Repo;

	private PardakhtCheckShVersion5PersistenceRepository pardakhtCheckShV5Repo;

	private SanadDetailV5PersistenceRepository pardakhtEnteghalShV5Repo;

	private PardakhtNaghdVersion5PersistenceRepository pardakhtNaghdV5Repo;

	private static SemaphoreSlim _readDataSemaphore = new SemaphoreSlim(1, 1);

	protected ContextHolder DbContext { get; private set; }

	public WebContextHolder Context => DbContext.Context;

	public string ContextGuid => Context.Guid;

	public bool IsDisposed => _isDisposed;

	public Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	internal PardakhtVersion5PersistenceRepository(ContextHolder context)
	{
		try
		{
			_instance.TryAdd(context.Context.Guid, this);
			DbContext = context;
			pardakhtBankV5Repo = new PardakhtBankVersion5PersistenceRepository(context);
			pardakhtCheckMV5Repo = new PardakhtCheckMVersion5PersistenceRepository(context);
			pardakhtCheckShV5Repo = new PardakhtCheckShVersion5PersistenceRepository(context);
			pardakhtEnteghalShV5Repo = new SanadDetailV5PersistenceRepository(context);
			pardakhtNaghdV5Repo = new PardakhtNaghdVersion5PersistenceRepository(context);
			DbContext = context;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 55);
			throw;
		}
	}

	public static PardakhtVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		PardakhtVersion5PersistenceRepository pardakhtVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 70);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 88);
			throw;
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		await DbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 101);
		try
		{
			int id = await Guid_To_IdAsync(item, token, tranName);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await DeleteDetsAsync(id, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 105);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await DeleteAsync(item, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 106);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 110);
			res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 111);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> DeleteDetsAsync(int id, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DbContext.GetContextAsync(tranName, 60000, "DeleteDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 118);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@SanadID", (object)id),
				new SqlParameter("@SanadType", (object)Document.Pardakht)
			};
			await baseModelContext.ExecuteNoneQueryAsync("sp_SanadDet_DeleteRows", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "DeleteDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 130);
			res.AddReturnedValue(ex, "", "DeleteDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 131);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> DeleteAsync(Guid pardakhtGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 138);
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await PardakhtBankVersion5PersistenceRepository.GetInstance(ContextGuid).DeleteAsync(pardakhtGuid, tranName), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 141);
			List<SqlParameter> list = new List<SqlParameter>
			{
				new SqlParameter("@PardakhtGuid", (object)pardakhtGuid)
			};
			List<SqlParameter> list2 = list;
			list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
			list.Add(new SqlParameter("@Date", (object)DateManagment.DateM));
			await c.ExecuteNoneQueryAsync("sp_Pardakht_DeleteRowBySanadID", list, tranName, token, "", CommandType.StoredProcedure, 30, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 152);
			res.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 153);
		}
		return res;
	}

	public async Task<PardakhtBusiness> GetAsync(Guid guid, CancellationToken token, string tranName = "")
	{
		PardakhtBusiness res = null;
		WebPardakht web = null;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 161);
		try
		{
			int num;
			_ = num - 1;
			_ = 3;
			PardakhtBusiness result;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
				using (DbDataReader dr = await c.ExecuteReaderAsync("sp_Pardakht_SelectRowByGuid", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs"))
				{
					if (await dr.ReadAsync())
					{
						web = await LoadDataPardakhtAsync(dr, token, tranName);
					}
					dr.Close();
					res = await PardakhtMapper.Instance.MapAsync(Context.Guid, web, null, "", token);
					if (res == null)
					{
						result = null;
					}
					else
					{
						if (res.CheckM.Any() || res.CheckSh.Any() || res.Bank.Any() || res.Naghd.Any() || res.Enteghal.Any())
						{
							goto end_IL_0205;
						}
						result = null;
					}
					goto end_IL_00e5;
					end_IL_0205:;
				}
				goto end_IL_00ce;
				end_IL_00e5:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 177);
				throw;
			}
			return result;
			end_IL_00ce:;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<PardakhtReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(PardakhtBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			WebPardakht webPardakht = await PardakhtMapper.Instance.MapAsync(ContextGuid, item, new WebPardakht(), tranName, token);
			if (webPardakht == null)
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(webPardakht, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 198);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 202);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 203);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> SaveAsync(WebPardakht item, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		try
		{
			PardakhtBusiness pardakhtBusiness = await GetAsync(item.Guid, token, tranName);
			if (pardakhtBusiness != null)
			{
				_ = pardakhtBusiness.Guid != Guid.Empty;
			}
			if (item.Guid == Guid.Empty)
			{
				item.Guid = Guid.NewGuid();
			}
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await SaveAsync_(item, token, tranName);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 218);
			if (res.HasError)
			{
				return res;
			}
			item.Id = returnedSaveFuncInfoWithValue.value;
			res.value = returnedSaveFuncInfoWithValue.value;
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue2 = res;
			returnedSaveFuncInfoWithValue2.AddReturnedValue(await RemoveDetsAsync(item.Guid, item.Id, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 222);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfoWithValue2 = res;
			returnedSaveFuncInfoWithValue2.AddReturnedValue(await SaveDetsAsync(item, item.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 224);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 228);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 229);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<int>> SaveAsync_(WebPardakht item, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		BaseModelContext baseModelContext = await DbContext.GetContextAsync(tranName, 60000, "SaveAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 236);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@ID2", (object)item.Id2),
				new SqlParameter("@SanadST", (object)item.SanadSt),
				new SqlParameter("@SanadNO", (object)item.SanadNumber),
				new SqlParameter("@MoeinGuid", (object)item.MoeinGuid),
				new SqlParameter("@TafsilGuid", (object)item.TafsilGuid),
				new SqlParameter("@CashierGuid", (object)item.CashierGuid),
				new SqlParameter("@Description", (object)(item.Description ?? "")),
				new SqlParameter("@Statuse", (object)(short)item.Statuse),
				new SqlParameter("@date", (object)item.Date),
				new SqlParameter("@Sum", (object)item.Sum),
				new SqlParameter("@GUID", (object)item.Guid),
				new SqlParameter("@Sum_Naghd", (object)item.SumNaghd),
				new SqlParameter("@Sum_Havale", (object)item.SumBank),
				new SqlParameter("@Sum_CheckM", (object)item.SumCheckM),
				new SqlParameter("@Sum_CheckSh", (object)item.SumCheckSh),
				new SqlParameter("@Sum_Enteghal", (object)item.SumEnteghal),
				new SqlParameter("@ServerID", (object)item.ReferencedId),
				new SqlParameter("@DeviceID", (object)item.DeviceId),
				new SqlParameter("@ServerSource", (object)(short)item.ServerStatus),
				new SqlParameter("@Ip", (object)(item.Ip ?? ""))
			};
			res.value = (int)(await baseModelContext.ExecuteScalarAsync<object>("sp_Pardakht_Save", parameters, tranName, token, 30, "SaveAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs"));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 267);
			res.AddReturnedValue(ex, "", "SaveAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 268);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> RemoveDetsAsync(Guid guid, int pardakhtId, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await pardakhtNaghdV5Repo.DeleteAsync(guid, tranName), ReturnedState.None, "RemoveDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 277);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await pardakhtBankV5Repo.DeleteAsync(guid, tranName), ReturnedState.None, "RemoveDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 279);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await pardakhtCheckMV5Repo.DeleteAsync(guid, tranName), ReturnedState.None, "RemoveDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 281);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await pardakhtEnteghalShV5Repo.RemoveSanadDetsAsync(tranName, guid, Document.Pardakht), ReturnedState.None, "RemoveDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 283);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 287);
			res.AddReturnedValue(ex, "", "RemoveDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 288);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> SaveDetsAsync(WebPardakht item, Guid pardakhtGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 311);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SaveDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 312);
			return returnedSaveFuncInfo;
		}
	}

	private async Task<ReturnedSaveFuncInfo> AddSanadDets(WebPardakht item, Guid pardakhtGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddSanadDets", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 343);
			res.AddReturnedValue(ex, "", "AddSanadDets", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 344);
			return res;
		}
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<int> Guid_To_IdAsync(Guid guid, CancellationToken token, string tranName = "")
	{
		int res = 0;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 356);
		try
		{
			_ = 1;
			try
			{
				if (guid == Guid.Empty)
				{
					return 0;
				}
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Pardakht_Guid_To_ID", parameters, tranName, token, 30, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
				res = ((obj != null) ? ((int)obj) : 0);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 368);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<PardakhtBusiness>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		return Task.FromResult<List<PardakhtBusiness>>(null);
	}

	public Task Add2CacheAsync(PardakhtBusiness item, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task ClearCachedItemAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task ClearCachedAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName)
	{
		Guid guid = Guid.Empty;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 402);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID2", (object)id2)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Pardakht_ID2_TO_Guid", parameters, tranName, default(CancellationToken), 30, "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
				if (obj != null)
				{
					guid = (Guid)obj;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 411);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			}
		}
		return guid;
	}

	public async Task<int> NextId2Async(int? statusId, CancellationToken token, string tranName)
	{
		int res = 0;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 424);
		try
		{
			_ = 1;
			try
			{
				res = await c.ExecuteScalarAsync<int>("sp_Pardakht_NextID2", null, tranName, token, 30, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 431);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<PardakhtBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<WebPardakht> GetAsync(int id, CancellationToken token, string tranName = "")
	{
		WebPardakht res = null;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 449);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Pardakht_SelectRow", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = await LoadDataPardakhtAsync(dr, token, tranName);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 460);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	private async Task<WebPardakht> LoadDataPardakhtAsync(DbDataReader dr, CancellationToken token, string tranName = "")
	{
		WebPardakht item = new WebPardakht();
		try
		{
			await _readDataSemaphore.WaitAsync();
			if (dr["ID"] != DBNull.Value)
			{
				item.Id = (int)dr["ID"];
			}
			if (dr["ID2"] != DBNull.Value)
			{
				item.Id2 = (int)dr["ID2"];
			}
			if (dr["SanadSt"] != DBNull.Value)
			{
				item.SanadSt = (SanadSt)dr["SanadSt"];
			}
			if (dr["SanadNo"] != DBNull.Value)
			{
				item.SanadNumber = (int)dr["SanadNo"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				item.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				item.FullName = dr["TafsilName"].ToString();
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				item.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				item.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				item.Description = dr["Description"].ToString();
			}
			if (dr["Statuse"] != DBNull.Value)
			{
				switch ((short)dr["Statuse"])
				{
				case 0:
					item.Statuse = PardakhtSt.None;
					break;
				case 1:
					item.Statuse = PardakhtSt.Usual;
					break;
				case 2:
					item.Statuse = PardakhtSt.Deleted;
					break;
				}
			}
			if (dr["date"] != DBNull.Value)
			{
				item.Date = (DateTime)dr["date"];
			}
			if (dr["DateSabt"] != DBNull.Value)
			{
				item.DateSabt = (DateTime)dr["DateSabt"];
			}
			if (dr["GUID"] != DBNull.Value)
			{
				item.Guid = (Guid)dr["GUID"];
			}
			WebPardakht webPardakht = item;
			if (webPardakht.Bank == null)
			{
				webPardakht.Bank = new List<WebPardakhtBank>();
			}
			webPardakht = item;
			if (webPardakht.CheckSh == null)
			{
				webPardakht.CheckSh = new List<WebPardakhtCheckSh>();
			}
			webPardakht = item;
			if (webPardakht.Naghd == null)
			{
				webPardakht.Naghd = new List<WebPardakhtNaghd>();
			}
			webPardakht = item;
			if (webPardakht.CheckM == null)
			{
				webPardakht.CheckM = new List<WebPardakhtCheckM>();
			}
			webPardakht = item;
			if (webPardakht.Enteghal == null)
			{
				webPardakht.Enteghal = new List<WebSanad_Det>();
			}
			PardakhtBankVersion5PersistenceRepository instance = PardakhtBankVersion5PersistenceRepository.GetInstance(ContextGuid);
			List<WebPardakhtBank> bank = item.Bank;
			bank.AddRange(await instance.GetAllAsync(item.Id, default(CancellationToken), tranName));
			List<WebPardakhtCheckSh> collection = await PardakhtCheckShVersion5PersistenceRepository.GetInstance(ContextGuid).GetAllWithSanadIdAsync(item.Id, tranName);
			item.CheckSh.AddRange(collection);
			PardakhtNaghdVersion5PersistenceRepository instance2 = PardakhtNaghdVersion5PersistenceRepository.GetInstance(ContextGuid);
			List<WebPardakhtNaghd> naghd = item.Naghd;
			naghd.AddRange(await instance2.GetAllAsync(item.Id, tranName));
			PardakhtCheckMVersion5PersistenceRepository instance3 = PardakhtCheckMVersion5PersistenceRepository.GetInstance(ContextGuid);
			List<WebPardakhtCheckM> checkM = item.CheckM;
			checkM.AddRange(await instance3.GetAllAsync(item.Id, token, tranName));
			List<WebSanad_Det> collection2 = await SanadDetailV5PersistenceRepository.GetInstance(ContextGuid).GetListAsync(item.Guid, Document.Pardakht, item.MoeinGuid, item.TafsilGuid, tranName);
			item.Enteghal.AddRange(collection2);
			if (item.TafsilGuid == Guid.Empty)
			{
				MoeinBusiness moein = await MoeinBusiness.GetAsync(ContextGuid, item.MoeinGuid, tranName);
				if (moein == null)
				{
					throw new Exception("حساب معین طرف حساب انتخاب شده معتبر نمی باشد".GetTranslate());
				}
				IEnumerable<TafsilBusiness> enumerable = await TafsilBusiness.GetAllAsync(ContextGuid, EnSystem.Transfer, new TafsilFilter
				{
					NameSearchString = moein.Name,
					NameSearchLocation = SearchLocation.Equal
				}, token, tranName, "LoadDataPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
				if (enumerable != null && enumerable.Any())
				{
					item.TafsilGuid = enumerable.FirstOrDefault().Guid;
				}
				else
				{
					TafsilBusiness tafsil = new TafsilBusiness(ContextGuid)
					{
						ChangeStatus = EntityChangeSt.New,
						ContextGuid = ContextGuid,
						DateM = DateTime.Now,
						Description = "حساب معین با نام".GetTranslate() + " " + moein.Name + " " + " در ورژن 5 وجود داشت که در برگه پرداخت شماره".GetTranslate() + " " + $" {item.Id2}" + " " + " استفاده شده بود ".GetTranslate() + "\r\n" + "برای انتقال از ورژن 5 به ورژن 6 حساب تفصیلی با همین نام ایجاد و به برگه جاری متصل شد".GetTranslate(),
						Guid = Guid.NewGuid(),
						HType = HesabType.Hesab,
						Modified = DateTime.Now,
						Name = moein.Name,
						ServerStatus = ServerStatusForClient.None,
						Status = true,
						System = true
					};
					List<MoeinTafsilGroupRelationsBusiness> list = (await MoeinTafsilGroupRelationsBusiness.GetAllAsync(ContextGuid, moein.Guid, null, token, tranName))?.ToList();
					if (list == null || list.Count <= 0 || list.Count >= 2)
					{
						long minCode = await TafsilGroupBusiness.NextMinCodeAsync(ContextGuid, token, tranName);
						TafsilGroupBusiness tafsilGroup = await TafsilGroupBusiness.GetAsync(ContextGuid, moein.Name, EnSystem.Mapper, token, tranName);
						if (tafsilGroup == null)
						{
							tafsilGroup = new TafsilGroupBusiness(ContextGuid)
							{
								ChangeStatus = EntityChangeSt.New,
								CodeMax = minCode + 50,
								CodeMin = minCode,
								Description = "حساب معین با نام".GetTranslate() + "  " + moein.Name + " " + " در ورژن 5 وجود داشت که در برگه پرداخت شماره".GetTranslate() + " " + $" {item.Id2}" + " " + " استفاده شده بود".GetTranslate() + "\r\n" + "برای انتقال از ورژن 5 به ورژن 6 حساب تفصیلی و گروه تفصیلی با همین نام ایجاد و به گروه تفصیلی جاری متصل شد".GetTranslate(),
								Guid = Guid.NewGuid(),
								Modified = DateTime.Now,
								MoeinAmaniDigaranNazdMaGuid = moein.Guid,
								MoeinAmaniMaNazdDigaranGuid = moein.Guid,
								MoeinBackBuyGuid = moein.Guid,
								MoeinBackSellGuid = moein.Guid,
								MoeinBazaryabBackSell1 = moein.Guid,
								MoeinBazaryabBackSell2 = moein.Guid,
								MoeinBazaryabBackSell3 = moein.Guid,
								MoeinBazaryabBackSell4 = moein.Guid,
								MoeinBazaryabSell1 = moein.Guid,
								MoeinBazaryabSell2 = moein.Guid,
								MoeinBazaryabSell3 = moein.Guid,
								MoeinBazaryabSell4 = moein.Guid,
								MoeinBuyGuid = moein.Guid,
								MoeinPardakhtGuid = moein.Guid,
								MoeinReceptionGuid = moein.Guid,
								MoeinSellGuid = moein.Guid,
								Name = moein.Name,
								ServerStatus = ServerStatusForClient.None,
								Status = true,
								Tbl = "",
								Type = EnTafsilGroupType.Related
							};
						}
						tafsilGroup.SetMoeinGuid(moein.Guid);
						(await tafsilGroup.SaveAsync(EnSystem.Transfer, send2Server: false, null, Priority.Low, awaitOnSemaphore: true, await4send: false, isRemoveWithGuid: false, 10, "", token, 0)).throwExceptionIfError();
						tafsil.TafsilGroupGuid = tafsilGroup.Guid;
					}
					else if (list.Count == 1)
					{
						Guid tafsilGroupGuid = list.First().TafsilGroupGuid;
						tafsil.TafsilGroupGuid = tafsilGroupGuid;
					}
					tafsil.Code = (await TafsilBusiness.NextCodeInGroupAsync(ContextGuid, EnSystem.Transfer, tafsil.TafsilGroupGuid, token, tranName)).value;
					item.TafsilGuid = tafsil.Guid;
					(await tafsil.SaveAsync(EnSystem.Transfer, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, token, updateAccount: true, awaitOnSemaphore: true, 10, send2Server: false, await4sent: false, 10)).throwExceptionIfError();
				}
			}
			if (item.Sum == 0m && item.SumBank == 0m && item.SumNaghd == 0m && item.SumCheckM == 0m && item.SumCheckSh == 0m && item.SumEnteghal == 0m)
			{
				return null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 605);
			throw;
		}
		finally
		{
			_readDataSemaphore.Release();
		}
		return item;
	}

	private PardakhtReportBusiness LoadPardakhtReport(DbDataReader dr)
	{
		PardakhtReportBusiness pardakhtReportBusiness = new PardakhtReportBusiness(ContextGuid);
		try
		{
			if (dr["GUID"] != DBNull.Value)
			{
				pardakhtReportBusiness.Guid = (Guid)dr["GUID"];
			}
			if (dr["ID2"] != DBNull.Value)
			{
				pardakhtReportBusiness.Id2 = (int)dr["ID2"];
			}
			if (dr["SanadStatus"] != DBNull.Value)
			{
				pardakhtReportBusiness.SanadSt = (short)(int)dr["SanadStatus"];
			}
			if (dr["SanadNo"] != DBNull.Value)
			{
				pardakhtReportBusiness.SanadNo = (int)dr["SanadNo"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				pardakhtReportBusiness.Description = dr["Description"].ToString();
			}
			if (dr["date"] != DBNull.Value)
			{
				pardakhtReportBusiness.DateM = (DateTime)dr["date"];
			}
			if (dr["ServerID"] != DBNull.Value)
			{
				pardakhtReportBusiness.HostRefrenced = dr["ServerID"].ToString();
			}
			if (dr["ServerSource"] != DBNull.Value)
			{
				pardakhtReportBusiness.Source = (ENSource)dr["ServerSource"];
			}
			if (dr["IP"] != DBNull.Value)
			{
				pardakhtReportBusiness.Ip = dr["IP"].ToString();
			}
			if (dr["DateSabt"] != DBNull.Value)
			{
				pardakhtReportBusiness.DateSabt = (DateTime)dr["DateSabt"];
			}
			if (dr["Sum_Naghd"] != DBNull.Value)
			{
				pardakhtReportBusiness.SumNaghd = (decimal)dr["Sum_Naghd"];
			}
			if (dr["Sum_Havale"] != DBNull.Value)
			{
				pardakhtReportBusiness.SumHavale = (decimal)dr["Sum_Havale"];
			}
			if (dr["Sum_CheckM"] != DBNull.Value)
			{
				pardakhtReportBusiness.SumChequeM = (decimal)dr["Sum_CheckM"];
			}
			if (dr["Sum_CheckSH"] != DBNull.Value)
			{
				pardakhtReportBusiness.SumChequeSh = (decimal)dr["Sum_CheckSH"];
			}
			if (dr["Sum_Enteghal"] != DBNull.Value)
			{
				pardakhtReportBusiness.SumEnteghal = (decimal)dr["Sum_Enteghal"];
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				pardakhtReportBusiness.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				pardakhtReportBusiness.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				pardakhtReportBusiness.CashierGuid = (Guid)dr["CashierGuid"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadPardakhtReport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 637);
			throw;
		}
		return pardakhtReportBusiness;
	}

	public async Task<IEnumerable<PardakhtReportBusiness>> GetAllAsync(KhazaneFilter filter, CancellationToken token, string tranName)
	{
		List<PardakhtReportBusiness> list = new List<PardakhtReportBusiness>();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 645);
		try
		{
			int num = 0;
			if (filter.Id2 != null && filter.Id2.Any())
			{
				num = (await StimulQueryBusiness.SaveAsync(ContextGuid, filter.Id2, token, tranName))?.value ?? 0;
			}
			List<SqlParameter> list2 = new List<SqlParameter>
			{
				new SqlParameter("@RefrencedId", (object)num),
				new SqlParameter("@Num1", (object)filter.Id2From),
				new SqlParameter("@Num2", (object)filter.Id2To),
				new SqlParameter("@Sum1", (object)filter.SumFrom),
				new SqlParameter("@Sum2", (object)filter.SumTo)
			};
			if (filter.Id2 != null && filter.Id2.Any())
			{
				list2.Add(new SqlParameter("@Numbers", (object)string.Join(",", filter.Id2)));
			}
			if (filter.HesabType.HasValue)
			{
				list2.Add(new SqlParameter("@hType", (object)filter.HesabType));
			}
			if (filter.Description != "%%")
			{
				list2.Add(new SqlParameter("@desc", (object)filter.Description));
			}
			if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
			{
				list2.Add(new SqlParameter("@TafsilGuid", (object)filter.TafsilGuid));
			}
			if (filter.CashierGuid.HasValue && filter.CashierGuid != Guid.Empty)
			{
				list2.Add(new SqlParameter("@CashierGuid", (object)filter.CashierGuid));
			}
			if (filter.DateFrom.HasValue)
			{
				list2.Add(new SqlParameter("@Date1", (object)filter.DateFrom));
			}
			if (filter.DateTo.HasValue)
			{
				list2.Add(new SqlParameter("@Date2", (object)filter.DateTo));
			}
			using DbDataReader dr = await c.ExecuteReaderAsync("sp_Pardakht_GetAllFromVPardakht", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			while (await dr.ReadAsync())
			{
				list.Add(LoadPardakhtReport(dr));
			}
			dr.Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 676);
			throw;
		}
		return list?.OrderBy((PardakhtReportBusiness q) => q.DateM)?.ToList();
	}

	public async Task<Guid> Id_To_GuidAsync(int id, CancellationToken token, string tranName = "")
	{
		Guid guid = Guid.Empty;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 684);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@id", (object)id)
				};
				guid = await c.ExecuteScalarAsync<Guid>("sp_Pardakht_ID_To_Guid", parameters, tranName, token, 30, "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 692);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			}
		}
		return guid;
	}

	public async Task<List<Guid>> GetNotTransferedAsync(CancellationToken token, string tranName)
	{
		List<Guid> res = new List<Guid>();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 705);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Pardakht_SelectNotTransfered", null, tranName, token, CommandType.StoredProcedure, 30, "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					res.Add((Guid)dr["Guid"]);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 715);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 725);
			try
			{
				_ = 1;
				try
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@Guid", (object)filter.ObjectGuid),
						new SqlParameter("@TransferDate", (object)filter.TransferDate)
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("SP_Pardakht_UpdateTransferDate", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs"), ReturnedState.None, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 733);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 737);
					res.AddReturnedValue(ex, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs", 738);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Pardakht\\PardakhtVersion5PersistenceRepository.cs");
				}
			}
			return res;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(PardakhtBusiness item, EnSystem system, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}

	public Task<List<ENSource>> GetAllExistingSourceAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid guid, int statusId, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
