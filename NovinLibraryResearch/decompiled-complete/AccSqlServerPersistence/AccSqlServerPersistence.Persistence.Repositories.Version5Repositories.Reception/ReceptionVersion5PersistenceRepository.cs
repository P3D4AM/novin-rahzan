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
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
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

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Reception;

public class ReceptionVersion5PersistenceRepository : IReceptionRepository, IEntityRepository<ReceptionBusiness, ReceptionReportBusiness>, IEditableDelivery, IDisposable
{
	private bool _isDisposed;

	private ReceptionCheckVersion5PersistenceRepository _receptionV5CheckRepo;

	private SanadDetailV5PersistenceRepository _receptionEnteghalV5Repo;

	private ReceptionHavaleVersion5PersistenceRepository _receptionHavaleV5Repo;

	private ReceptionNaghdVersion5PersistenceRepository _receptionNaghV5Repo;

	private TableRelatioShipPersistenceRepository _relationRepo;

	private static ConcurrentDictionary<string, ReceptionVersion5PersistenceRepository> _instance = new ConcurrentDictionary<string, ReceptionVersion5PersistenceRepository>();

	public WebContextHolder Context => DbContext.Context;

	public string ContextGuid => Context.Guid;

	protected ContextHolder DbContext { get; private set; }

	public bool IsDisposed => _isDisposed;

	public Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public ReceptionVersion5PersistenceRepository(ContextHolder dbContext)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs"))
		{
			try
			{
				_instance.TryAdd(dbContext.Context.Guid, this);
				DbContext = dbContext;
				_receptionV5CheckRepo = new ReceptionCheckVersion5PersistenceRepository(dbContext);
				_receptionEnteghalV5Repo = new SanadDetailV5PersistenceRepository(dbContext);
				_receptionHavaleV5Repo = new ReceptionHavaleVersion5PersistenceRepository(dbContext);
				_receptionNaghV5Repo = new ReceptionNaghdVersion5PersistenceRepository(dbContext);
				_relationRepo = new TableRelatioShipPersistenceRepository(dbContext);
				DbContext = dbContext;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 60);
				throw;
			}
		}
	}

	public static ReceptionVersion5PersistenceRepository GetInstance(string contextGuid)
	{
		ReceptionVersion5PersistenceRepository receptionVersion5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 75);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 90);
		}
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 114);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 115);
			return returnedSaveFuncInfo;
		}
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(int id, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 123);
		try
		{
			List<SqlParameter> list = new List<SqlParameter>
			{
				new SqlParameter("@ID", (object)id)
			};
			List<SqlParameter> list2 = list;
			list2.Add(new SqlParameter("@CashierGuid", (object)(await CashierBusiness.GetCurrentCashierAsync(ContextGuid)).Guid));
			List<SqlParameter> parameters = list;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Reception_DeleteRowBySanadID", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs"), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 131);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 135);
			res.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 136);
		}
		return res;
	}

	public async Task<ReceptionBusiness> GetAsync(Guid guid, OrderBusiness order, EnSystem system, CancellationToken token, string tranName)
	{
		ReceptionBusiness res = null;
		WebReception web = null;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 145);
		try
		{
			int num;
			_ = num - 1;
			_ = 3;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Reception_SelectRowByGuid", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
				if (await dr.ReadAsync())
				{
					web = await LoadReceptionAsync(dr, isLoadForReport: false, token, tranName);
				}
				dr.Close();
				res = await ReceptionMapper.Instance.MapAsync(Context.Guid, web, new ReceptionBusiness(ContextGuid, order), tranName, token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 157);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public Task<ReceptionReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(ReceptionBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 204);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 205);
			return returnedSaveFuncInfo;
		}
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<ReceptionBusiness>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		return Task.FromResult<List<ReceptionBusiness>>(null);
	}

	public Task Add2CacheAsync(ReceptionBusiness item, CancellationToken token, string tranName)
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

	public async Task<int> NextId2Async(int? statusId, CancellationToken token, string tranName)
	{
		int res = 1;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 243);
		try
		{
			_ = 1;
			try
			{
				res = await c.ExecuteScalarAsync<int>("sp_Reception_NextID2", null, tranName, default(CancellationToken), 30, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 250);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName)
	{
		Guid guid = Guid.Empty;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 264);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID2", (object)id2)
				};
				object obj = await c.ExecuteScalarAsync<object>("sp_Reception_ID2_TO_Guid", parameters, tranName, default(CancellationToken), 30, "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
				if (obj != null)
				{
					guid = (Guid)obj;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 273);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
		}
		return guid;
	}

	public async Task<ReceptionBusiness> GetFromDbAsync(Guid guid, OrderBusiness order, EnSystem system, CancellationToken token, string tranName)
	{
		return await GetAsync(guid, order, system, token, tranName);
	}

	public async Task<List<ReceptionReportBusiness>> GetAllAsync(KhazaneFilter filters, CancellationToken token, string tranName)
	{
		List<ReceptionReportBusiness> list = new List<ReceptionReportBusiness>();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 289);
		try
		{
			string SourceStr = "";
			foreach (ENSource source in filters.Sources)
			{
				string text = SourceStr;
				short num = (short)source;
				SourceStr = text + num + ",";
			}
			int num2 = (await StimulQueryBusiness.SaveAsync(ContextGuid, filters.Id2, token, tranName))?.value ?? 0;
			List<SqlParameter> list2 = new List<SqlParameter>
			{
				new SqlParameter("@Sum1", (object)filters.SumFrom),
				new SqlParameter("@Sum2", (object)filters.SumTo),
				new SqlParameter("@Num1", (object)filters.Id2From),
				new SqlParameter("@Num2", (object)filters.Id2To),
				new SqlParameter("@ReferencedId", (object)num2),
				new SqlParameter("@SourceSTR", (object)SourceStr),
				new SqlParameter("@ErjaSTR", (object)filters.HostRefrences)
			};
			if (filters.DateFrom.HasValue)
			{
				list2.Add(new SqlParameter("@Date1", (object)filters.DateFrom));
			}
			if (filters.DateTo.HasValue)
			{
				list2.Add(new SqlParameter("@Date2", (object)filters.DateTo));
			}
			if (filters.Description != "%%")
			{
				list2.Add(new SqlParameter("@Desc", (object)filters.Description));
			}
			if (filters.CashierGuid != Guid.Empty)
			{
				list2.Add(new SqlParameter("@CashierGuid", (object)filters.CashierGuid));
			}
			if (filters.TafsilGuid != Guid.Empty)
			{
				list2.Add(new SqlParameter("@TafsilGuid", (object)filters.TafsilGuid));
			}
			if (filters.CustomerGroupGuid != Guid.Empty)
			{
				list2.Add(new SqlParameter("@CustomerGroupGuid", (object)filters.CustomerGroupGuid));
			}
			if (filters.Type.HasValue)
			{
				list2.Add(new SqlParameter("@TypeBySell", (object)(int)filters.Type.Value));
			}
			if (filters.Id2 != null && filters.Id2.Any())
			{
				list2.Add(new SqlParameter("@Numbers", (object)string.Join(",", filters.Id2)));
			}
			using DbDataReader dr = await c.ExecuteReaderAsync("sp_Reception_GetAllForReport", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			while (await dr.ReadAsync())
			{
				list.Add(LoadReceptionReport(dr, token));
			}
			dr.Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 322);
			throw;
		}
		return list?.OrderBy((ReceptionReportBusiness q) => q.DateM)?.ToList();
	}

	public async Task<ReturnedSaveFuncInfo> SetReceptionTypeBySellAsync(List<Guid> lst1, List<Guid> lst2, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 331);
		try
		{
			foreach (Guid item in lst1)
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)item)
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Reception_SetTypeBySell", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs"), ReturnedState.None, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 338);
			}
			foreach (Guid item2 in lst2)
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)item2)
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Sell_SetTasviyeDay", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs"), ReturnedState.None, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 344);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 349);
			res.AddReturnedValue(ex, "", "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 350);
		}
		return res;
	}

	public async Task<int> Guid_To_IdAsync(Guid guid, CancellationToken token, string tranName = "")
	{
		int res = 0;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 359);
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
				object obj = await c.ExecuteScalarAsync<object>("sp_Reception_Guid_To_ID", parameters, tranName, token, 30, "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
				if (obj != null)
				{
					res = (int)obj;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 369);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Guid_To_IdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<WebReception> GetAsync(Guid guid, bool isLoadForReport, CancellationToken token, string tranName = "")
	{
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 382);
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 394);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
		}
	}

	private async Task<WebReception> LoadReceptionAsync(DbDataReader dr, bool isLoadForReport, CancellationToken token, string tranName = "")
	{
		WebReception item = new WebReception();
		try
		{
			item.Guid = (Guid)dr["Guid"];
			item.Id = (int)dr["ID"];
			if (dr["ID2"] != DBNull.Value)
			{
				item.Id2 = (int)dr["ID2"];
			}
			if (dr["SanadSt"] != DBNull.Value)
			{
				item.SanadSt = (short)dr["SanadSt"];
			}
			if (dr["SanadNo"] != DBNull.Value)
			{
				item.SanadNumber = (int)dr["SanadNo"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				item.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				item.UserGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				item.Description = dr["Description"].ToString();
			}
			if (dr["Date"] != DBNull.Value)
			{
				item.Date = (DateTime)dr["Date"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				item.Status = (bool)dr["Status"];
			}
			item.DateSabt = item.Date;
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				item.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["TypeBySell"] != DBNull.Value)
			{
				item.DependencyType = (DependencyType)dr["TypeBySell"];
			}
			if (isLoadForReport)
			{
				if (dr["ServerID"] != DBNull.Value)
				{
					item.ReferencedId = (int)dr["ServerID"];
				}
				if (dr["IP"] != DBNull.Value)
				{
					item.Ip = dr["IP"].ToString();
				}
				if (dr["CustomerFullName"] != DBNull.Value)
				{
					item.FullName = dr["CustomerFullName"].ToString();
				}
			}
			if (string.IsNullOrEmpty(item.Description))
			{
				item.Description = "** فاقد توضیحات **";
			}
			WebReception webReception = item;
			if (webReception.ListCheck == null)
			{
				webReception.ListCheck = new List<WebReceptionCheck>();
			}
			webReception = item;
			if (webReception.ListBank == null)
			{
				webReception.ListBank = new List<WebReceptionHavale>();
			}
			webReception = item;
			if (webReception.ListNaghd == null)
			{
				webReception.ListNaghd = new List<WebReceptionNaghd>();
			}
			webReception = item;
			if (webReception.ListEnteghal == null)
			{
				webReception.ListEnteghal = new List<WebSanad_Det>();
			}
			List<WebReceptionCheck> listCheck = item.ListCheck;
			listCheck.AddRange(await _receptionV5CheckRepo.GetAllChecksAsync(item.Id, token, tranName));
			List<WebReceptionHavale> listBank = item.ListBank;
			listBank.AddRange(await _receptionHavaleV5Repo.GetAllHavaleAsync(item.Id, tranName));
			List<WebReceptionNaghd> listNaghd = item.ListNaghd;
			listNaghd.AddRange(await _receptionNaghV5Repo.GetAllNaghdAsync(item.Id, tranName));
			List<WebSanad_Det> listEnteghal = item.ListEnteghal;
			listEnteghal.AddRange(await _receptionEnteghalV5Repo.GetListAsync(item.Guid, Document.Reception, item.MoeinGuid, item.TafsilGuid, tranName, token));
			if (!item.ListCheck.Any() && !item.ListBank.Any() && !item.ListNaghd.Any() && !item.ListEnteghal.Any())
			{
				return null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 445);
			throw;
		}
		return item;
	}

	private ReceptionReportBusiness LoadReceptionReport(DbDataReader dr, CancellationToken token)
	{
		ReceptionReportBusiness receptionReportBusiness = new ReceptionReportBusiness(ContextGuid);
		try
		{
			receptionReportBusiness.Guid = (Guid)dr["Guid"];
			if (dr["ID2"] != DBNull.Value)
			{
				receptionReportBusiness.Id2 = (int)dr["ID2"];
			}
			if (dr["SanadStatus"] != DBNull.Value)
			{
				receptionReportBusiness.SanadSt = (short)(int)dr["SanadStatus"];
			}
			if (dr["SanadNo"] != DBNull.Value)
			{
				receptionReportBusiness.SanadNo = (int)dr["SanadNo"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				receptionReportBusiness.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				receptionReportBusiness.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				receptionReportBusiness.Description = dr["Description"].ToString();
			}
			if (dr["Date"] != DBNull.Value)
			{
				receptionReportBusiness.DateM = (DateTime)dr["Date"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				receptionReportBusiness.Status = (bool)dr["Status"];
			}
			if (dr["TypeBySell"] != DBNull.Value)
			{
				receptionReportBusiness.DependencyType = (DependencyType)dr["TypeBySell"];
			}
			if (dr["ServerID"] != DBNull.Value)
			{
				receptionReportBusiness.DeliveryId = dr["ServerID"].ToString().ParseToInt();
			}
			if (dr["IP"] != DBNull.Value)
			{
				receptionReportBusiness.Ip = dr["IP"].ToString();
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				receptionReportBusiness.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["Sum_Naghd"] != DBNull.Value)
			{
				receptionReportBusiness.SumNaghd = (decimal)dr["Sum_Naghd"];
			}
			if (dr["Sum_Havale"] != DBNull.Value)
			{
				receptionReportBusiness.SumHavale = (decimal)dr["Sum_Havale"];
			}
			if (dr["Sum_Check"] != DBNull.Value)
			{
				receptionReportBusiness.SumChequeSh = (decimal)dr["Sum_Check"];
			}
			if (dr["Sum_Enteghal"] != DBNull.Value)
			{
				receptionReportBusiness.SumEnteghal = (decimal)dr["Sum_Enteghal"];
			}
			if (string.IsNullOrEmpty(receptionReportBusiness.Description))
			{
				receptionReportBusiness.Description = "** فاقد توضیحات **";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadReceptionReport", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 495);
			throw;
		}
		return receptionReportBusiness;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> AddAsync(ReceptionBusiness item, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
		await DbContext.GetContextAsync(tranName, 60000, "AddAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 503);
		res.value = 0;
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 559);
			res.AddReturnedValue(ex, "", "AddAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 560);
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> EditAsync(ReceptionBusiness item, int receptionId, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		await DbContext.GetContextAsync(tranName, 60000, "EditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 569);
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 617);
			res.AddReturnedValue(ex, "", "EditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 618);
			return res;
		}
	}

	private async Task<ReturnedSaveFuncInfo> AddSanadDetAsync(ReceptionBusiness item, Guid receptionGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 651);
			res.AddReturnedValue(ex, "", "AddSanadDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 652);
			return res;
		}
	}

	public async Task<int> ID2_TO_IDAsync(int id2, CancellationToken token, string tranName = "")
	{
		int res = 0;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "ID2_TO_IDAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 659);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ID2", (object)id2)
				};
				res = await c.ExecuteScalarAsync<int>("sp_Reception_ID2_TO_ID", parameters, tranName, default(CancellationToken), 30, "ID2_TO_IDAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ID2_TO_IDAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 667);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("ID2_TO_IDAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<Guid> Id_To_GuidAsync(int id, CancellationToken token, string tranName = "")
	{
		Guid guid = Guid.Empty;
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 680);
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@id", (object)id)
				};
				guid = await c.ExecuteScalarAsync<Guid>("sp_Reception_ID_To_Guid", parameters, tranName, default(CancellationToken), 30, "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 688);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("Id_To_GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
		}
		return guid;
	}

	public Task<ReceptionBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<Guid>> GetNotTransferedAsync(CancellationToken token, string tranName)
	{
		List<Guid> res = new List<Guid>();
		BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 705);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Reception_SelectNotTransfered", null, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					res.Add((Guid)dr["Guid"]);
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 715);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetNotTransferedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			BaseModelContext c = await DbContext.GetContextAsync(tranName, 60000, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 725);
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
					returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("SP_Reception_UpdateTransferDate", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs"), ReturnedState.None, "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 733);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 737);
					res.AddReturnedValue(ex, "", "UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs", 738);
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("UpdateTransferDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Reception\\ReceptionVersion5PersistenceRepository.cs");
				}
			}
			return res;
		}
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
