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
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Check;

public class ViewCheckShVersion5PersistenceRepository : IViewCheckShRepository, ISyncRepository<ViewCheckShBusiness>, IEditableDelivery, IRepository<ViewCheckShBusiness>, INoGuidRepository<ViewCheckShBusiness>, IDisposable
{
	private bool _isDisposed;

	private static ConcurrentDictionary<string, ViewCheckShVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, ViewCheckShVersion5PersistenceRepository>();

	public bool IsDisposed => _isDisposed;

	public ContextHolder DatabaseContext { get; private set; }

	public WebContextHolder Context => DatabaseContext.Context;

	public string ContextGuid => Context?.Guid;

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 33);
		}
	}

	public ViewCheckShVersion5PersistenceRepository(ContextHolder dbContext)
	{
		_instance.TryAdd(dbContext.Context.Guid, this);
		DatabaseContext = dbContext;
	}

	public static ViewCheckShVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		ViewCheckShVersion5PersistenceRepository viewCheckShVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 55);
			throw;
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(ViewCheckShBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ViewCheckShBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<ViewCheckShBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		return GetAllAsync(null, token, tranName);
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

	public async Task<ViewCheckShBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		ViewCheckShBusiness res = null;
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 95);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ViewCheckSh_Get", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					res = LoadData(dr);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 105);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<ViewCheckShBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		return GetAsync(guid, token, tranName);
	}

	public async Task<IEnumerable<ViewCheckShBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		List<ViewCheckShBusiness> list = new List<ViewCheckShBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 120);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ViewCheckSh_GetAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 129);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
			}
		}
		return list?.OrderBy((ViewCheckShBusiness q) => q.DateSarresidM)?.ToList();
	}

	private ViewCheckShBusiness LoadData(DbDataReader dr)
	{
		ViewCheckShBusiness viewCheckShBusiness = new ViewCheckShBusiness(ContextGuid);
		try
		{
			if (dr["Guid"] != DBNull.Value)
			{
				viewCheckShBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Number"] != DBNull.Value)
			{
				viewCheckShBusiness.Number = dr["Number"].ToString();
			}
			if (dr["Price"] != DBNull.Value)
			{
				viewCheckShBusiness.Price = (decimal)dr["Price"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				viewCheckShBusiness.ChequeStatus = (ENCheckSHStatus)(short)dr["Status"];
			}
			if (dr["BankName"] != DBNull.Value)
			{
				viewCheckShBusiness.BankName = dr["BankName"].ToString();
			}
			if (dr["CustomerFullName"] != DBNull.Value)
			{
				viewCheckShBusiness.CustomerFullName = dr["CustomerFullName"].ToString();
			}
			if (dr["PardakhtDate"] != DBNull.Value)
			{
				viewCheckShBusiness.PardakhtDate = dr["PardakhtDate"].ToString();
			}
			if (dr["masterGuid"] != DBNull.Value)
			{
				viewCheckShBusiness.GuidSanad = (Guid)dr["masterGuid"];
			}
			if (dr["Aval2re"] != DBNull.Value)
			{
				viewCheckShBusiness.Aval2re = (bool)dr["Aval2re"];
			}
			if (dr["DateSarresidM"] != DBNull.Value)
			{
				viewCheckShBusiness.DateSarresidM = (DateTime)dr["DateSarresidM"];
			}
			if (dr["PardakhtDesc"] != DBNull.Value)
			{
				viewCheckShBusiness.PardakhtDesc = dr["PardakhtDesc"].ToString();
			}
			if (dr["DeliveryId"] != DBNull.Value)
			{
				viewCheckShBusiness.DeliveryId = (int)dr["DeliveryId"];
			}
			if (dr["ServerStatus"] != DBNull.Value)
			{
				viewCheckShBusiness.ServerStatus = (ServerStatusForClient)(short)dr["ServerStatus"];
			}
			if (dr["BankGuid"] != DBNull.Value)
			{
				viewCheckShBusiness.BankGuid = (Guid)dr["BankGuid"];
			}
			if (dr["A2reMoeinGuid"] != DBNull.Value)
			{
				viewCheckShBusiness.A2reMoeinGuid = (Guid)dr["A2reMoeinGuid"];
			}
			if (dr["A2reTafsilGuid"] != DBNull.Value)
			{
				viewCheckShBusiness.A2reTafsilGuid = (Guid)dr["A2reTafsilGuid"];
			}
			if (dr["CreditMoeinGuid"] != DBNull.Value)
			{
				viewCheckShBusiness.CreditMoeinGuid = (Guid)dr["CreditMoeinGuid"];
			}
			if (dr["CreditTafsilGuid"] != DBNull.Value)
			{
				viewCheckShBusiness.CreditTafsilGuid = (Guid)dr["CreditTafsilGuid"];
			}
			if (dr["IsEnteghali"] != DBNull.Value)
			{
				viewCheckShBusiness.IsEnteghali = (bool)dr["IsEnteghali"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 166);
			throw;
		}
		return viewCheckShBusiness;
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
		using (new JaegerMonitor("SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await DatabaseContext.GetContextAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 179));
				PardakhtCheckSh pardakhtCheckSh = ((IQueryable<PardakhtCheckSh>)acc_ModelContext.PardakhtCheckSh).FirstOrDefault((PardakhtCheckSh q) => q.Guid == del.Guid);
				if (pardakhtCheckSh == null)
				{
					return ret;
				}
				pardakhtCheckSh.ServerStatus = (del.DeliveryStatus ? ServerStatusForClient.Delivered : ServerStatusForClient.DeliveryError);
				((EntityEntry)((DbContext)acc_ModelContext).Entry<PardakhtCheckSh>(pardakhtCheckSh)).State = (EntityState)3;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 185);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 188);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 189);
			}
			return ret;
		}
	}

	public async Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		List<ViewCheckShBusiness> list = new List<ViewCheckShBusiness>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 196);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ViewCheckSh_GetAllNotSent", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 205);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
			}
		}
		return list?.Select((ViewCheckShBusiness q) => q.Guid)?.ToList();
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await DatabaseContext.GetContextAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 223));
				PardakhtCheckSh pardakhtCheckSh = ((IQueryable<PardakhtCheckSh>)acc_ModelContext.PardakhtCheckSh).FirstOrDefault((PardakhtCheckSh q) => q.Guid == item.Guid);
				if (pardakhtCheckSh == null)
				{
					return ret;
				}
				pardakhtCheckSh.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				((EntityEntry)((DbContext)acc_ModelContext).Entry<PardakhtCheckSh>(pardakhtCheckSh)).State = (EntityState)3;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 229);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 232);
				ret.AddReturnedValue(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 233);
			}
			return ret;
		}
	}

	public async Task<IEnumerable<ViewCheckShBusiness>> GetAllAsync(ENCheckSHStatus? status, DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		try
		{
			IEnumerable<ViewCheckShBusiness> enumerable = await GetAllAsync(token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			if (status.HasValue)
			{
				enumerable = enumerable?.Where((ViewCheckShBusiness q) => q.ChequeStatus == status);
			}
			if (date1.HasValue)
			{
				enumerable = enumerable?.Where((ViewCheckShBusiness q) => q.DateSarresidM >= date1.Value.Date);
			}
			if (date2.HasValue)
			{
				enumerable = enumerable?.Where((ViewCheckShBusiness q) => q.DateSarresidM <= date2.Value.Date);
			}
			return enumerable;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 250);
			throw;
		}
	}

	public async Task<bool> CheckSHA2re_ISEnteghaliAsync(Guid guid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "CheckSHA2re_ISEnteghaliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 256);
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
				object obj = await c.ExecuteScalarAsync<object>("sp_PardakhtCheckA2re_GetIsEnteghali", parameters, tranName, token, 30, "CheckSHA2re_ISEnteghaliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
				result = obj != null && !string.IsNullOrEmpty(obj.ToString()) && (bool)obj;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckSHA2re_ISEnteghaliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 267);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("CheckSHA2re_ISEnteghaliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> ElamPardakhtAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 281);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckShBusiness item2 in checkList.Where((ViewCheckShBusiness item) => item.ChequeStatus == ENCheckSHStatus.Pardakhtani))
				{
					List<SqlParameter> list = new List<SqlParameter>();
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid)).Guid));
					list.Add(new SqlParameter("@Date", (object)dateM));
					list.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					list.Add(new SqlParameter("@SanadST", (object)(short)1));
					list.Add(new SqlParameter("@BankMoeinGuid", (object)ParentDefaults.MoeinCoding.CLSMoein4));
					list.Add(new SqlParameter("@CheckGuid", (object)item2.Guid));
					List<SqlParameter> parameters = list;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_CheckSHPardakht_Insert", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"), ReturnedState.None, "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 296);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 301);
				res.AddReturnedValue(ex, "", "ElamPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 302);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> EbtalAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 311);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckShBusiness item2 in checkList.Where((ViewCheckShBusiness item) => item.ChequeStatus == ENCheckSHStatus.Pardakhtani))
				{
					List<SqlParameter> list = new List<SqlParameter>();
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					list.Add(new SqlParameter("@Date", (object)dateM));
					list.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					list.Add(new SqlParameter("@SanadST", SqlDbType.BigInt));
					list.Add(new SqlParameter("@CheckGuid", (object)item2.Guid));
					List<SqlParameter> parameters = list;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_CheckSHEbtal_Insert", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"), ReturnedState.None, "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 325);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 330);
				res.AddReturnedValue(ex, "", "EbtalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 331);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> BargashtAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("BargashtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "BargashtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 340);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckShBusiness item2 in checkList.Where((ViewCheckShBusiness item) => item.ChequeStatus == ENCheckSHStatus.Pardakhtani))
				{
					List<SqlParameter> list = new List<SqlParameter>();
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					list.Add(new SqlParameter("@Date", (object)dateM));
					list.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					list.Add(new SqlParameter("@SanadST", (object)(short)1));
					list.Add(new SqlParameter("@CheckGuid", (object)item2.Guid));
					List<SqlParameter> parameters = list;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_CheckSHBargasht_Insert", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "BargashtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"), ReturnedState.None, "BargashtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 354);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BargashtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 359);
				res.AddReturnedValue(ex, "", "BargashtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 360);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> PardakhtShode2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("PardakhtShode2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "PardakhtShode2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 369);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckShBusiness item2 in checkList.Where((ViewCheckShBusiness item) => item.ChequeStatus == ENCheckSHStatus.VosolShode))
				{
					List<SqlParameter> list = new List<SqlParameter>();
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					list.Add(new SqlParameter("@DateM", (object)dateM));
					list.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					list.Add(new SqlParameter("@BankMoeinGuid", (object)ParentDefaults.MoeinCoding.CLSMoein4));
					list.Add(new SqlParameter("@Guid", (object)item2.Guid));
					List<SqlParameter> parameters = list;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_CheckSHPardakht_DeleteRow", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "PardakhtShode2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"), ReturnedState.None, "PardakhtShode2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 383);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PardakhtShode2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 388);
				res.AddReturnedValue(ex, "", "PardakhtShode2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 389);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Bargashty2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("Bargashty2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Bargashty2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 398);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckShBusiness item2 in checkList.Where((ViewCheckShBusiness item) => item.ChequeStatus == ENCheckSHStatus.BargashtShode))
				{
					List<SqlParameter> list = new List<SqlParameter>();
					List<SqlParameter> list2 = list;
					list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid));
					list.Add(new SqlParameter("@Date", (object)dateM));
					list.Add(new SqlParameter("@SanadNO", (object)sanadNo));
					list.Add(new SqlParameter("@SanadST", (object)(short)1));
					list.Add(new SqlParameter("@Guid", (object)item2.Guid));
					List<SqlParameter> parameters = list;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_CheckSHBargasht_DeleteRow", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "Bargashty2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"), ReturnedState.None, "Bargashty2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 412);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Bargashty2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 417);
				res.AddReturnedValue(ex, "", "Bargashty2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 418);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Ebtal2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName is null!! tranName");
		}
		using (new JaegerMonitor("Ebtal2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"))
		{
			BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "Ebtal2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 427);
			try
			{
				if (!(checkList?.Any() ?? false))
				{
					return res;
				}
				foreach (ViewCheckShBusiness item in checkList.Where((ViewCheckShBusiness item) => item.ChequeStatus == ENCheckSHStatus.BatelShode))
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@Guid", (object)item.Guid)
					};
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_CheckSHEbtal_DeleteRow", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "Ebtal2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"), ReturnedState.None, "Ebtal2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 434);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Ebtal2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 439);
				res.AddReturnedValue(ex, "", "Ebtal2PardakhtaniAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 440);
			}
			return res;
		}
	}

	public ViewCheckShBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(Guid chequeGuid, ENCheckSHStatus status, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 451);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@ChequeGuid", (object)chequeGuid),
				new SqlParameter("@Status", (object)status)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_Acc_PardakhtCheckSh_ForceUpdateChequeStatus", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs"), ReturnedState.None, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 459);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 463);
			res.AddReturnedValue(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 464);
		}
		return res;
	}

	public async Task<IEnumerable<ChequeSHSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 471);
		IEnumerable<ChequeSHSummaryViewModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ChequeSHSummaryViewModel> list = new List<ChequeSHSummaryViewModel>();
				List<SqlParameter> list2 = new List<SqlParameter>();
				if (date1.HasValue)
				{
					list2.Add(new SqlParameter("@date1", (object)date1));
				}
				if (date2.HasValue)
				{
					list2.Add(new SqlParameter("@date2", (object)date2));
				}
				DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_PardakhtCheck_Summary", list2, tranName, token, CommandType.StoredProcedure, 30, "GetSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataSummary(dr));
				}
				dr.Close();
				result = list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 486);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs");
			}
		}
		return result;
	}

	private ChequeSHSummaryViewModel LoadDataSummary(DbDataReader dr)
	{
		try
		{
			ChequeSHSummaryViewModel chequeSHSummaryViewModel = new ChequeSHSummaryViewModel();
			if (dr["Status"] != DBNull.Value)
			{
				chequeSHSummaryViewModel.Status = (ENCheckSHStatus)dr["Status"];
			}
			if (dr["ChequeCount"] != DBNull.Value)
			{
				chequeSHSummaryViewModel.Count = (int)dr["ChequeCount"];
			}
			if (dr["ChequeSum"] != DBNull.Value)
			{
				chequeSHSummaryViewModel.Sum = (decimal)dr["ChequeSum"];
			}
			return chequeSHSummaryViewModel;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataSummary", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Check\\ViewCheckShVersion5PersistenceRepository.cs", 507);
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
