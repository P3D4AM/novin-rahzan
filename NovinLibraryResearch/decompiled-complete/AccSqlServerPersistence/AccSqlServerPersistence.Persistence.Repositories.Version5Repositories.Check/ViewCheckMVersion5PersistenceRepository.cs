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
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters.Configs;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Check;

public class ViewCheckMVersion5PersistenceRepository : IViewCheckMRepository, ISyncRepository<ViewCheckMBusiness>, IEditableDelivery, IRepository<ViewCheckMBusiness>, INoGuidRepository<ViewCheckMBusiness>, IDisposable
{
	private bool _isDisposed;

	private static ConcurrentDictionary<string, ViewCheckMVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, ViewCheckMVersion5PersistenceRepository>();

	public bool IsDisposed => _isDisposed;

	public ContextHolder DatabaseContext { get; private set; }

	public WebContextHolder Context => DatabaseContext.Context;

	public string ContextGuid => Context?.Guid;

	public ViewCheckMVersion5PersistenceRepository(ContextHolder dbContext)
	{
		_instance.TryAdd(dbContext.Context.Guid, this);
		DatabaseContext = dbContext;
	}

	public static ViewCheckMVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		ViewCheckMVersion5PersistenceRepository viewCheckMVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 49);
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
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 63);
			throw;
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(ViewCheckMBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ViewCheckMBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<ViewCheckMBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		return GetAllAsync(null, token, tranName);
	}

	public async Task<ViewCheckMBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		ViewCheckMBusiness res = null;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 96);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ViewCheckM_Get", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = LoadData(dr);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 106);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<ViewCheckMBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public Task<IEnumerable<ViewCheckMBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		return GetAllAsync(null, null, null, null, token, tranName);
	}

	public async Task<IEnumerable<ViewCheckMBusiness>> GetAllAsync(ENUMCheck? status, Guid? pardazandeGuid, DateTime? dateSarresid1, DateTime? dateSarresid2, CancellationToken token, string tranName)
	{
		List<ViewCheckMBusiness> list = new List<ViewCheckMBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 122);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> list2 = new List<SqlParameter>();
				if (status.HasValue)
				{
					list2.Add(new SqlParameter("@status", (object)(short)status.Value));
				}
				if (pardazandeGuid.HasValue && pardazandeGuid != Guid.Empty)
				{
					list2.Add(new SqlParameter("@pardazandeGuid", (object)pardazandeGuid));
				}
				if (dateSarresid1.HasValue)
				{
					list2.Add(new SqlParameter("@d1", (object)dateSarresid1));
				}
				if (dateSarresid2.HasValue)
				{
					list2.Add(new SqlParameter("@d2", (object)dateSarresid2));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ViewCheckM_GetAll", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 138);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
		}
		return list?.OrderBy((ViewCheckMBusiness q) => q.DateCheckM)?.ToList();
	}

	public async Task<bool> CheckMA2re_ISEnteghaliAsync(Guid guid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "CheckMA2re_ISEnteghaliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 150);
		bool result;
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@guid", (object)guid)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_ReceptionCheckA2re_GetIsEnteghali", parameters, tranName, token, 30, "CheckMA2re_ISEnteghaliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
				result = obj != null && !string.IsNullOrEmpty(obj.ToString()) && (bool)obj;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckMA2re_ISEnteghaliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 161);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("CheckMA2re_ISEnteghaliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> VagozarKardanAsync(List<ViewCheckMBusiness> checkList, Guid guid, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 175);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.Mojod))
				{
					Guid moeinGuid = Guid.Empty;
					TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, guid, tranName, token, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
					if (tafsilBusiness != null)
					{
						TafsilGroupBusiness tafsilGroupBusiness = await tafsilBusiness.GetTafsilGroupAsync(EnSystem.Mapper, token, tranName);
						if (tafsilGroupBusiness != null)
						{
							moeinGuid = await tafsilGroupBusiness.GetMoeinGuidAsync();
						}
					}
					CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@GuidCheck", (object)item.Guid),
						new SqlParameter("@BankTafsilGuid", (object)guid),
						new SqlParameter("@BankMoeinGuid", (object)moeinGuid),
						new SqlParameter("@Date", (object)dateM),
						new SqlParameter("@CashierGuid", (object)cashierBusiness.Guid),
						new SqlParameter("@SanadST", (object)(short)1),
						new SqlParameter("@DebitMoeinGuid", (object)ParentDefaults.MoeinCoding.CLSMoein29978)
					};
					if (sanadNo <= 0)
					{
						SqlParameter val = new SqlParameter("@SanadNO", (object)sanadNo);
						((DbParameter)val).Direction = ParameterDirection.Output;
						SqlParameter item2 = val;
						list.Add(item2);
					}
					else
					{
						list.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					}
					ReturnedSaveFuncInfoWithValue<object> returnedSaveFuncInfoWithValue = await c.ExecuteNoneQueryAsync("sp_BankCheck_Insert", list, tranName, token, "@SanadNo", CommandType.StoredProcedure, 30, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
					if (returnedSaveFuncInfoWithValue.value != null)
					{
						sanadNo = (int)returnedSaveFuncInfoWithValue.value;
					}
					res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 209);
				}
				res.value = sanadNo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 216);
				res.AddReturnedValue(ex, "", "VagozarKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 217);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> BargashtZadanAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 226);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item in checkList.Where(delegate(ViewCheckMBusiness q)
				{
					ENUMCheck status = q.Status;
					return (status == ENUMCheck.Mojod || (uint)(status - 3) <= 1u) ? true : false;
				}))
				{
					CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@GuidCheck", (object)item.Guid),
						new SqlParameter("@SanadST", (object)(short)1),
						new SqlParameter("@Date", (object)dateM),
						new SqlParameter("@CashierGuid", (object)cashierBusiness.Guid)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@DebitMoeinGuid", (object)(await item.GetPardazandeMoeinGuid(EnSystem.Mapper, token, tranName))));
					list.Add(new SqlParameter("@DebitTafsilGuid", (object)item.PardazandeTafsilGuid));
					List<SqlParameter> p = list;
					switch (item.Status)
					{
					case ENUMCheck.Mojod:
						p.Add(new SqlParameter("@CreditMoeinGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception));
						p.Add(new SqlParameter("@CreditTafsilGuid", (object)LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception));
						break;
					case ENUMCheck.VagozarShode:
						p.Add(new SqlParameter("@CreditMoeinGuid", (object)ParentDefaults.MoeinCoding.CLSMoein29978));
						break;
					case ENUMCheck.KharjShode:
						list = p;
						list.Add(new SqlParameter("@CreditMoeinGuid", (object)(await PardakhtBusiness.GetAsync(ContextGuid, item.PardakhtGuid, fromDatabase: true, token)).MoeinGuid));
						list = p;
						list.Add(new SqlParameter("@CreditTafsilGuid", (object)(await PardakhtBusiness.GetAsync(ContextGuid, item.PardakhtGuid, fromDatabase: true, token)).TafsilGuid));
						break;
					}
					if (sanadNo <= 0)
					{
						SqlParameter val = new SqlParameter("@SanadNO", (object)sanadNo);
						((DbParameter)val).Direction = ParameterDirection.Output;
						SqlParameter item2 = val;
						p.Add(item2);
					}
					else
					{
						p.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					}
					ReturnedSaveFuncInfoWithValue<object> returnedSaveFuncInfoWithValue = await c.ExecuteNoneQueryAsync("sp_CheckMBargasht_Insert", p, tranName, token, "@SanadNo", CommandType.StoredProcedure, 30, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
					if (returnedSaveFuncInfoWithValue.value != null)
					{
						sanadNo = (int)returnedSaveFuncInfoWithValue.value;
					}
					res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 265);
				}
				res.value = sanadNo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 272);
				res.AddReturnedValue(ex, "", "BargashtZadanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 273);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> NaghdKardanAsync(List<ViewCheckMBusiness> checkList, Guid guid, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 282);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				Guid tafsilGroupGuid = (await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, guid, "", token, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs")).TafsilGroupGuid;
				Guid? sandoghMoeinGuid = (await MoeinTafsilGroupRelationsBusiness.GetAllAsync(ContextGuid, null, tafsilGroupGuid, token))?.FirstOrDefault()?.MoeinGuid;
				if (!sandoghMoeinGuid.HasValue || sandoghMoeinGuid == Guid.Empty)
				{
					sandoghMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein7;
				}
				foreach (ViewCheckMBusiness item in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.Mojod))
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@sandoghTafsilGuid", (object)guid),
						new SqlParameter("@sandoghMoeinGuid", (object)sandoghMoeinGuid),
						new SqlParameter("@GuidCheck", (object)item.Guid),
						new SqlParameter("@Date", (object)dateM),
						new SqlParameter("@SanadST", (object)(short)1)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					list.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					List<SqlParameter> parameters = list;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_SandoghCheck_Insert", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"), ReturnedState.None, "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 303);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 308);
				res.AddReturnedValue(ex, "", "NaghdKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 309);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> EbtalAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 318);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.Mojod))
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@Guid", (object)item.Guid)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					List<SqlParameter> parameters = list;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_CheckM_Ebtal", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 329);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 334);
				res.AddReturnedValue(ex, "", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 335);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> VosolKardanAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 345);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item2 in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.VagozarShode))
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@GuidCheck", (object)item2.Guid),
						new SqlParameter("@SanadST", (object)(short)1)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					list.Add(new SqlParameter("@Date", (object)dateM));
					list.Add(new SqlParameter("@CreditMoeinGuid", (object)ParentDefaults.MoeinCoding.CLSMoein29978));
					List<SqlParameter> list3 = list;
					if (sanadNo <= 0)
					{
						SqlParameter val = new SqlParameter("@SanadNO", (object)sanadNo);
						((DbParameter)val).Direction = ParameterDirection.Output;
						SqlParameter item = val;
						list3.Add(item);
					}
					else
					{
						list3.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					}
					ReturnedSaveFuncInfoWithValue<object> returnedSaveFuncInfoWithValue = await c.ExecuteNoneQueryAsync("sp_CheckeVosol_Insert", list3, tranName, token, "@SanadNo", CommandType.StoredProcedure, 30, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
					if (returnedSaveFuncInfoWithValue.value != null)
					{
						sanadNo = (int)returnedSaveFuncInfoWithValue.value;
					}
					res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 367);
				}
				res.value = sanadNo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 373);
				res.AddReturnedValue(ex, "", "VosolKardanAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 374);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> OdatBargashtiAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 383);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.Bargashti))
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@GuidCheck", (object)item.Guid),
						new SqlParameter("@SanadST", (object)(short)1),
						new SqlParameter("@Date", (object)dateM)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					list.Add(new SqlParameter("@ISOdat", (object)true));
					list.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					List<SqlParameter> parameters = list;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_CheckMBargasht_DeleteRow", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"), ReturnedState.None, "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 398);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 403);
				res.AddReturnedValue(ex, "", "OdatBargashtiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 404);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> Bargashty2GhablAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("Bargashty2GhablAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Bargashty2GhablAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 413);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item2 in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.Bargashti))
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@GuidCheck", (object)item2.Guid),
						new SqlParameter("@SanadST", (object)(short)1),
						new SqlParameter("@Date", (object)dateM)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					List<SqlParameter> list3 = list;
					if (sanadNo <= 0)
					{
						SqlParameter val = new SqlParameter("@SanadNO", (object)sanadNo);
						((DbParameter)val).Direction = ParameterDirection.Output;
						SqlParameter item = val;
						list3.Add(item);
					}
					else
					{
						list3.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					}
					ReturnedSaveFuncInfoWithValue<object> returnedSaveFuncInfoWithValue = await c.ExecuteNoneQueryAsync("sp_CheckMBargasht_DeleteRow", list3, tranName, token, "@SanadNo", CommandType.StoredProcedure, 30, "Bargashty2GhablAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
					if (returnedSaveFuncInfoWithValue.value != null)
					{
						sanadNo = (int)returnedSaveFuncInfoWithValue.value;
					}
					res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Bargashty2GhablAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 435);
				}
				res.value = sanadNo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Bargashty2GhablAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 441);
				res.AddReturnedValue(ex, "", "Bargashty2GhablAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 442);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> Vagozar2MojoodAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("Vagozar2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Vagozar2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 451);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item2 in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.VagozarShode))
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@GuidCheck", (object)item2.Guid),
						new SqlParameter("@SanadST", (object)(short)1),
						new SqlParameter("@DateM", (object)dateM)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					List<SqlParameter> list3 = list;
					if (sanadNo <= 0)
					{
						SqlParameter val = new SqlParameter("@SanadNO", (object)sanadNo);
						((DbParameter)val).Direction = ParameterDirection.Output;
						SqlParameter item = val;
						list3.Add(item);
					}
					else
					{
						list3.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					}
					ReturnedSaveFuncInfoWithValue<object> returnedSaveFuncInfoWithValue = await c.ExecuteNoneQueryAsync("sp_BankCheck_DeleteRow", list3, tranName, token, "@SanadNo", CommandType.StoredProcedure, 30, "Vagozar2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
					if (returnedSaveFuncInfoWithValue.value != null)
					{
						sanadNo = (int)returnedSaveFuncInfoWithValue.value;
					}
					res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Vagozar2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 473);
				}
				res.value = sanadNo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Vagozar2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 479);
				res.AddReturnedValue(ex, "", "Vagozar2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 480);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> Naghd2MojoodAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("Naghd2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Naghd2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 489);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item2 in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.NaghdShode))
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@GuidCheck", (object)item2.Guid),
						new SqlParameter("@SanadST", (object)(short)1),
						new SqlParameter("@Date", (object)dateM)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					List<SqlParameter> list3 = list;
					if (sanadNo <= 0)
					{
						SqlParameter val = new SqlParameter("@SanadNO", (object)sanadNo);
						((DbParameter)val).Direction = ParameterDirection.Output;
						SqlParameter item = val;
						list3.Add(item);
					}
					else
					{
						list3.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					}
					ReturnedSaveFuncInfoWithValue<object> returnedSaveFuncInfoWithValue = await c.ExecuteNoneQueryAsync("sp_SandoghCheck_DeleteRow", list3, tranName, token, "@SanadNo", CommandType.StoredProcedure, 30, "Naghd2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
					if (returnedSaveFuncInfoWithValue.value != null)
					{
						sanadNo = (int)returnedSaveFuncInfoWithValue.value;
					}
					res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Naghd2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 511);
				}
				res.value = sanadNo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Naghd2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 517);
				res.AddReturnedValue(ex, "", "Naghd2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 518);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Batel2MojoodAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("Batel2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Batel2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 527);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.BatelShode))
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@Guid", (object)item.Guid)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					List<SqlParameter> parameters = list;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_CheckM_Ebtal2Mojod", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "Batel2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"), ReturnedState.None, "Batel2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 538);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Batel2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 543);
				res.AddReturnedValue(ex, "", "Batel2MojoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 544);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> Vosol2VagozarAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("Vosol2VagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Vosol2VagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 553);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item2 in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.VosolShode))
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@GuidCheck", (object)item2.Guid),
						new SqlParameter("@SanadST", (object)(short)1),
						new SqlParameter("@Date", (object)dateM)
					};
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					List<SqlParameter> list3 = list;
					if (sanadNo <= 0)
					{
						SqlParameter val = new SqlParameter("@SanadNO", (object)sanadNo);
						((DbParameter)val).Direction = ParameterDirection.Output;
						SqlParameter item = val;
						list3.Add(item);
					}
					else
					{
						list3.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					}
					ReturnedSaveFuncInfoWithValue<object> returnedSaveFuncInfoWithValue = await c.ExecuteNoneQueryAsync("sp_CheckeVosol_DeleteRow", list3, tranName, token, "@SanadNo", CommandType.StoredProcedure, 30, "Vosol2VagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
					if (returnedSaveFuncInfoWithValue.value != null)
					{
						sanadNo = (int)returnedSaveFuncInfoWithValue.value;
					}
					res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Vosol2VagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 575);
				}
				res.value = sanadNo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Vosol2VagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 581);
				res.AddReturnedValue(ex, "", "Vosol2VagozarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 582);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> ElamTahvilAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 591);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckMBusiness item in checkList.Where((ViewCheckMBusiness q) => q.Status == ENUMCheck.BatelShode))
				{
					int num = await Guid_To_IdAsync(item.Guid, tranName);
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@IDCheck", (object)num)
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_ReceptionCheck_ElamTahvil", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"), ReturnedState.None, "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 599);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 604);
				res.AddReturnedValue(ex, "", "ElamTahvilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 605);
			}
			return res;
		}
	}

	private ViewCheckMBusiness LoadData(DbDataReader dr)
	{
		ViewCheckMBusiness viewCheckMBusiness = new ViewCheckMBusiness(ContextGuid);
		try
		{
			if (dr["Guid"] != DBNull.Value)
			{
				viewCheckMBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["BankName"] != DBNull.Value)
			{
				viewCheckMBusiness.BankName = dr["BankName"].ToString();
			}
			if (dr["Status"] != DBNull.Value)
			{
				viewCheckMBusiness.Status = (ENUMCheck)(int)dr["Status"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				viewCheckMBusiness.Description = dr["Description"].ToString();
			}
			if (dr["Number"] != DBNull.Value)
			{
				viewCheckMBusiness.Number = dr["Number"].ToString();
			}
			if (dr["Price"] != DBNull.Value)
			{
				viewCheckMBusiness.Price = (decimal)dr["Price"];
			}
			if (dr["Aval2re"] != DBNull.Value)
			{
				viewCheckMBusiness.Aval2re = (bool)dr["Aval2re"];
			}
			if (dr["DateCheckM"] != DBNull.Value)
			{
				viewCheckMBusiness.DateCheckM = (DateTime)dr["DateCheckM"];
			}
			if (dr["Personal"] != DBNull.Value)
			{
				viewCheckMBusiness.Personal = (bool)dr["Personal"];
			}
			if (dr["HesabNum"] != DBNull.Value)
			{
				viewCheckMBusiness.HesabNum = dr["HesabNum"].ToString();
			}
			if (dr["PoshtNomreh"] != DBNull.Value)
			{
				viewCheckMBusiness.PoshtNomreh = dr["PoshtNomreh"].ToString().ParseToLong(0L);
			}
			if (dr["Place"] != DBNull.Value)
			{
				viewCheckMBusiness.Place = dr["Place"].ToString();
			}
			if (dr["ReceptionGuid"] != DBNull.Value)
			{
				viewCheckMBusiness.GuidSanad = (Guid)dr["ReceptionGuid"];
			}
			if (dr["DateBargashtORVosolM"] != DBNull.Value)
			{
				viewCheckMBusiness.DateBargashtORVosolM = (DateTime)dr["DateBargashtORVosolM"];
			}
			if (dr["DateSanadM"] != DBNull.Value)
			{
				viewCheckMBusiness.DateSanadM = (DateTime)dr["DateSanadM"];
			}
			if (dr["DateToPlace"] != DBNull.Value)
			{
				viewCheckMBusiness.DateToPlace = dr["DateToPlace"].ToString();
			}
			if (dr["PardazandeTafsilGuid"] != DBNull.Value)
			{
				viewCheckMBusiness.PardazandeTafsilGuid = (Guid)dr["PardazandeTafsilGuid"];
			}
			if (dr["DeliveryId"] != DBNull.Value)
			{
				viewCheckMBusiness.DeliveryId = dr["DeliveryId"].ToString().ParseToInt();
			}
			if (dr["ServerStatus"] != DBNull.Value)
			{
				viewCheckMBusiness.ServerStatus = (ServerStatusForClient)(short)dr["ServerStatus"];
			}
			if (dr["PardakhtGuid"] != DBNull.Value)
			{
				viewCheckMBusiness.PardakhtGuid = (Guid)dr["PardakhtGuid"];
			}
			if (dr["CustomerFullName"] != DBNull.Value)
			{
				viewCheckMBusiness.CustomerFullName = dr["CustomerFullName"].ToString();
			}
			if (dr["IsEnteghali"] != DBNull.Value)
			{
				viewCheckMBusiness.IsEnteghali = (bool)dr["IsEnteghali"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 639);
			throw;
		}
		return viewCheckMBusiness;
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		if (del == null)
		{
			throw new ArgumentNullException("Delivery is null!! del");
		}
		using (new JaegerMonitor("SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await DatabaseContext.GetContextAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 652));
				ReceptionCheck receptionCheck = ((IQueryable<ReceptionCheck>)acc_ModelContext.ReceptionCheck).FirstOrDefault((ReceptionCheck q) => q.Guid == del.Guid);
				if (receptionCheck == null)
				{
					return ret;
				}
				receptionCheck.ServerStatus = (del.DeliveryStatus ? ServerStatusForClient.Delivered : ServerStatusForClient.DeliveryError);
				((EntityEntry)((DbContext)acc_ModelContext).Entry<ReceptionCheck>(receptionCheck)).State = (EntityState)3;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 658);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 661);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 662);
			}
			return ret;
		}
	}

	public async Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		List<ViewCheckMBusiness> list = new List<ViewCheckMBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 669);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ViewCheckM_GetAllNotSent", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 679);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
		}
		return list?.Select((ViewCheckMBusiness q) => q.Guid)?.ToList();
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await DatabaseContext.GetContextAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 697));
				ReceptionCheck receptionCheck = ((IQueryable<ReceptionCheck>)acc_ModelContext.ReceptionCheck).FirstOrDefault((ReceptionCheck q) => q.Guid == item.Guid);
				if (receptionCheck == null)
				{
					return ret;
				}
				receptionCheck.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				((EntityEntry)((DbContext)acc_ModelContext).Entry<ReceptionCheck>(receptionCheck)).State = (EntityState)3;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 703);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 706);
				ret.AddReturnedValue(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 707);
			}
			return ret;
		}
	}

	public async Task<Guid> Id_To_GuidAsync(int id, string tranName = "")
	{
		Guid guid = Guid.Empty;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 714);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@id", (object)id)
				};
				guid = await c.ExecuteScalarAsync<Guid>("sp_ViewCheckM_ID_To_Guid", parameters, tranName, default(CancellationToken), 30, "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 722);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
		}
		return guid;
	}

	public async Task<int> Guid_To_IdAsync(Guid guid, string tranName = "")
	{
		int id = 0;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 735);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
				id = await c.ExecuteScalarAsync<int>("sp_ViewCheckM_Guid_To_Id", parameters, tranName, default(CancellationToken), 30, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 743);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
		}
		return id;
	}

	public ViewCheckMBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<Guid> GetBankVagozarGuidAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetBankVagozarGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 758);
		Guid result;
		try
		{
			_ = 1;
			try
			{
				Guid guid = Guid.Empty;
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ChequeGuid", (object)chequeGuid)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_GetBankChequeGuid", parameters, tranName, token, 30, "GetBankVagozarGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
				if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
				{
					guid = (Guid)obj;
				}
				result = guid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetBankVagozarGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 770);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetBankVagozarGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(ViewChequeMForceUpdateConfig config, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 782);
		try
		{
			List<SqlParameter> list = new List<SqlParameter>
			{
				new SqlParameter("@ChequeGuid", (object)config.ChequeGuid),
				new SqlParameter("@Status", (object)config.Status)
			};
			if ((config?.DebitTafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				list.Add(new SqlParameter("@DebitTafsilGuid", (object)config.DebitTafsilGuid));
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_Acc_ReceptionCheck_ForceUpdateChequeStatus", list, tranName, token, "", CommandType.StoredProcedure, 30, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs"), ReturnedState.None, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 793);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 797);
			res.AddReturnedValue(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 798);
		}
		return res;
	}

	public async Task<IEnumerable<ChequeMSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 805);
		IEnumerable<ChequeMSummaryViewModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ChequeMSummaryViewModel> list = new List<ChequeMSummaryViewModel>();
				List<SqlParameter> list2 = new List<SqlParameter>();
				if (date1.HasValue)
				{
					list2.Add(new SqlParameter("@date1", (object)date1));
				}
				if (date2.HasValue)
				{
					list2.Add(new SqlParameter("@date2", (object)date2));
				}
				DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_ReceptionCheck_Summary", list2, tranName, token, CommandType.StoredProcedure, 30, "GetSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataSummary(dr));
				}
				dr.Close();
				result = list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 820);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	private ChequeMSummaryViewModel LoadDataSummary(DbDataReader dr)
	{
		try
		{
			ChequeMSummaryViewModel chequeMSummaryViewModel = new ChequeMSummaryViewModel();
			if (dr["Status"] != DBNull.Value)
			{
				chequeMSummaryViewModel.Status = (ENUMCheck)dr["Status"];
			}
			if (dr["ChequeCount"] != DBNull.Value)
			{
				chequeMSummaryViewModel.Count = (int)dr["ChequeCount"];
			}
			if (dr["ChequeSum"] != DBNull.Value)
			{
				chequeMSummaryViewModel.Sum = (decimal)dr["ChequeSum"];
			}
			return chequeMSummaryViewModel;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataSummary", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckMVersion5PersistenceRepository.cs", 841);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}
}
