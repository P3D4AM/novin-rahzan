using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Sanad;

public class SanadDetailV5PersistenceRepository : ISanad_DetRepository, IRepository<Sanad_DetBusiness>, INoGuidRepository<Sanad_DetBusiness>, IDisposable
{
	private bool _isDisposed;

	private static ConcurrentDictionary<string, SanadDetailV5PersistenceRepository> _instance = new ConcurrentDictionary<string, SanadDetailV5PersistenceRepository>();

	public WebContextHolder Context => DatabaseContext.Context;

	public ContextHolder DatabaseContext { get; private set; }

	public IItemAsyncMapper<Sanad_Det, Sanad_DetBusiness> MapFromDatabase { get; }

	public IItemAsyncMapper<Sanad_DetBusiness, Sanad_Det> MapToDatabase { get; }

	public bool IsDisposed => _isDisposed;

	public SanadDetailV5PersistenceRepository(ContextHolder dbContext)
	{
		_instance.TryAdd(dbContext.Context.Guid, this);
		DatabaseContext = dbContext;
	}

	public static SanadDetailV5PersistenceRepository GetInstance(string contextGuid)
	{
		SanadDetailV5PersistenceRepository sanadDetailV5PersistenceRepository = null;
		try
		{
			_instance.TryGetValue(contextGuid, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 40);
			throw;
		}
	}

	public Task<List<Sanad_DetBusiness>> GetAllAsync(Guid SanadGuid, Document SanadType, Guid MoeinGuid, Guid TafsilGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<Sanad_DetBusiness>> GetAllAsync(Guid SanadGuid, Document SanadType, Guid? MoeinGuid, Guid? TafsilGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveListToSanadDetAsync(int SanadGuid, Document SanadType, List<Sanad_DetBusiness> lst, Guid SanadMoeinGuid, Guid SanadTafsilGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteBySanadNoAsync(int SanadNo, Guid SanadGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<Sanad_DetBusiness>> GetAllAsync(int sanadNo, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebSanad_Det item, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		await DatabaseContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 77);
		try
		{
			throw new NotImplementedException("");
		}
		catch (Exception ex)
		{
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 104);
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 105);
			return res;
		}
	}

	private WebSanad_Det LoadData(DbDataReader dr, Guid masterMoeinGuid, Guid masterTafsilGuid)
	{
		WebSanad_Det webSanad_Det = new WebSanad_Det();
		try
		{
			Guid guid = Guid.Empty;
			Guid guid2 = Guid.Empty;
			Guid guid3 = Guid.Empty;
			Guid guid4 = Guid.Empty;
			if (dr["ID"] != DBNull.Value)
			{
				webSanad_Det.ID = dr["ID"].ToString().ParseToInt();
			}
			if (dr["SanadGuid"] != DBNull.Value)
			{
				webSanad_Det.SanadGuid = (Guid)dr["SanadGuid"];
			}
			if (dr["SanadType"] != DBNull.Value)
			{
				webSanad_Det.AsType = (Document)dr["SanadType"];
			}
			if (dr["Debit"] != DBNull.Value)
			{
				webSanad_Det.Debit = (decimal)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				webSanad_Det.Credit = (decimal)dr["Credit"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				webSanad_Det.Description = dr["Description"].ToString();
			}
			if (dr["GUID"] != DBNull.Value)
			{
				webSanad_Det.Guid = (Guid)dr["GUID"];
			}
			if (dr["DebitMoeinGuid"] != DBNull.Value)
			{
				guid4 = (Guid)dr["DebitMoeinGuid"];
			}
			if (dr["DebitTafsilGuid"] != DBNull.Value)
			{
				guid3 = (Guid)dr["DebitTafsilGuid"];
			}
			if (dr["CreditMoeinGuid"] != DBNull.Value)
			{
				guid2 = (Guid)dr["CreditMoeinGuid"];
			}
			if (dr["CreditTafsilGuid"] != DBNull.Value)
			{
				guid = (Guid)dr["CreditTafsilGuid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				webSanad_Det.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (masterMoeinGuid == Guid.Empty)
			{
				if (webSanad_Det.Debit > 0m)
				{
					webSanad_Det.MoeinGuid = guid4;
					webSanad_Det.TafsilGuid = guid3;
				}
				else
				{
					webSanad_Det.MoeinGuid = guid2;
					webSanad_Det.TafsilGuid = guid;
				}
			}
			if (webSanad_Det.AsType == Document.Pardakht)
			{
				if (guid4 == masterMoeinGuid && guid3 == masterTafsilGuid)
				{
					webSanad_Det.MoeinGuid = guid2;
					webSanad_Det.TafsilGuid = guid;
					webSanad_Det.Debit = 0m;
				}
				else if (guid2 == masterMoeinGuid && guid == masterTafsilGuid)
				{
					webSanad_Det.MoeinGuid = guid4;
					webSanad_Det.TafsilGuid = guid3;
					webSanad_Det.Credit = 0m;
				}
				else if (guid3 == masterTafsilGuid)
				{
					webSanad_Det.MoeinGuid = guid2;
					webSanad_Det.TafsilGuid = guid;
					webSanad_Det.Debit = 0m;
				}
				else if (guid == masterTafsilGuid)
				{
					webSanad_Det.MoeinGuid = guid4;
					webSanad_Det.TafsilGuid = guid3;
					webSanad_Det.Credit = 0m;
				}
				else
				{
					webSanad_Det.MoeinGuid = guid2;
					webSanad_Det.TafsilGuid = guid;
					webSanad_Det.Debit = 0m;
					string text = "reception.\r\n" + $"masterMoieinGuid:{masterMoeinGuid}\r\n" + $"MasterTafsilGuid:{masterTafsilGuid}\r\n" + $"debitMoeinGuid:{guid4}\r\n" + $"debitTafsilGuid:{guid3}\r\n" + $"creditMoeinGuid:{guid2}\r\n" + $"creditTafsilGuid:{guid}\r\n" + "SanadDet:" + Json.ToStringJson(webSanad_Det, indented: true);
					webSanad_Det.Description += text;
				}
			}
			if (webSanad_Det.AsType == Document.Reception)
			{
				if (guid2 == masterMoeinGuid && guid == masterTafsilGuid)
				{
					webSanad_Det.MoeinGuid = guid4;
					webSanad_Det.TafsilGuid = guid3;
					webSanad_Det.Credit = 0m;
				}
				else if (guid4 == masterMoeinGuid && guid3 == masterTafsilGuid)
				{
					webSanad_Det.MoeinGuid = guid2;
					webSanad_Det.TafsilGuid = guid;
					webSanad_Det.Debit = 0m;
				}
				else if (guid == masterTafsilGuid)
				{
					webSanad_Det.MoeinGuid = guid4;
					webSanad_Det.TafsilGuid = guid3;
					webSanad_Det.Credit = 0m;
				}
				else if (guid3 == masterTafsilGuid)
				{
					webSanad_Det.MoeinGuid = guid2;
					webSanad_Det.TafsilGuid = guid;
					webSanad_Det.Debit = 0m;
				}
				else
				{
					webSanad_Det.MoeinGuid = guid4;
					webSanad_Det.TafsilGuid = guid3;
					webSanad_Det.Credit = 0m;
					string text2 = "reception.\r\n" + $"masterMoieinGuid:{masterMoeinGuid}\r\n" + $"MasterTafsilGuid:{masterTafsilGuid}\r\n" + $"debitMoeinGuid:{guid4}\r\n" + $"debitTafsilGuid:{guid3}\r\n" + $"creditMoeinGuid:{guid2}\r\n" + $"creditTafsilGuid:{guid}\r\n" + "SanadDet:" + Json.ToStringJson(webSanad_Det, indented: true);
					webSanad_Det.Description += text2;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 264);
			throw;
		}
		return webSanad_Det;
	}

	public async Task<List<WebSanad_Det>> GetListAsync(Guid sanadGuid, Document sanadType, Guid masterMoeinGuid, Guid masterTafsilGuid, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		List<WebSanad_Det> res = new List<WebSanad_Det>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 274);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadGuid", (object)sanadGuid),
					new SqlParameter("@SanadType", (object)(short)sanadType)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_SanadDet_SelectRowSByOrder", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs");
				while (await dr.ReadAsync(token))
				{
					res.Add(LoadData(dr, masterMoeinGuid, masterTafsilGuid));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 289);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs");
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveSanadDetsAsync(string tranName, Guid sanadGuid, Document sanadType)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "RemoveSanadDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 303);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@SanadGuid", (object)sanadGuid),
				new SqlParameter("@SanadType", (object)(short)sanadType)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_SanadDet_DeleteRows", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "RemoveSanadDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs"), ReturnedState.None, "RemoveSanadDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 311);
		}
		catch (Exception ex)
		{
			res.AddReturnedValue(ex, "", "RemoveSanadDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 315);
			WebErrorLog.Instance.StartLog(ex, "", "RemoveSanadDetsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 316);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> Save_ListAsync(List<WebSanad_Det> list, string tranName, Guid sanadGuid, Document sanadType, DateTime sanadDate, int sanadId2)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await RemoveSanadDetsAsync(tranName, sanadGuid, sanadType), ReturnedState.None, "Save_ListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 325);
			if (res.HasError)
			{
				return res;
			}
			foreach (WebSanad_Det item in list)
			{
				item.SanadGuid = sanadGuid;
				item.AsType = sanadType;
				item.DateM = sanadDate;
				item.Serial = sanadId2.ToString();
				if (!(item.Credit <= 0m) || !(item.Debit <= 0m))
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tranName), ReturnedState.None, "Save_ListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 335);
					if (res.HasError)
					{
						return res;
					}
				}
			}
		}
		catch (Exception ex)
		{
			res.AddReturnedValue(ex, "", "Save_ListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 341);
			WebErrorLog.Instance.StartLog(ex, "", "Save_ListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 342);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		BaseModelContext baseModelContext = await DatabaseContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 350);
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@AsGuid", (object)asGuid)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await baseModelContext.ExecuteNoneQueryAsync("sp_SanadDetail_DeleteRowsByAsGuid", parameters, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs"), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 354);
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 358);
			WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 359);
		}
		return ret;
	}

	private WebSanad_Det LoadDetails(DbDataReader dr)
	{
		WebSanad_Det webSanad_Det = new WebSanad_Det();
		try
		{
			if (dr["Date"] != DBNull.Value)
			{
				webSanad_Det.DateM = (DateTime)dr["Date"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				webSanad_Det.Description = dr["Description"].ToString();
			}
			if (dr["Debit"] != DBNull.Value)
			{
				webSanad_Det.Debit = (decimal)dr["Debit"];
			}
			if (dr["Credit"] != DBNull.Value)
			{
				webSanad_Det.Credit = (decimal)dr["Credit"];
			}
			if (dr["SanadNO"] != DBNull.Value)
			{
				webSanad_Det.SanadNo = (int)dr["SanadNO"];
			}
			if (dr["SanadST"] != DBNull.Value)
			{
				webSanad_Det.SanadST = (SanadSt)(int)dr["SanadST"];
			}
			if (dr["GUID"] != DBNull.Value)
			{
				webSanad_Det.Guid = (Guid)dr["GUID"];
			}
			if (dr["QT"] != DBNull.Value)
			{
				webSanad_Det.Qt = (decimal)(double)dr["QT"];
			}
			if (dr["RowDesc"] != DBNull.Value)
			{
				webSanad_Det.RowDesc = dr["RowDesc"].ToString();
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				webSanad_Det.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				webSanad_Det.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				webSanad_Det.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["AsGuid"] != DBNull.Value)
			{
				webSanad_Det.AsGuid = (Guid)dr["AsGuid"];
			}
			webSanad_Det.SanadGuid = webSanad_Det.Guid;
			if (dr["ASType"] != DBNull.Value)
			{
				webSanad_Det.AsType = (Document)(int)dr["ASType"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDetails", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 387);
			throw;
		}
		return webSanad_Det;
	}

	public async Task<List<WebSanad_Det>> GetAllSanadDetailsAsync(int sanadNo, string tranName = "")
	{
		List<WebSanad_Det> list = new List<WebSanad_Det>();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 396);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@SanadNo", (object)sanadNo)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_VSanadDetail_SelectRow_SanadNO", parameters, tranName, default(CancellationToken), CommandType.StoredProcedure, 30, "GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDetails(dr));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 407);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 428);
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(List<Sanad_DetBusiness> items, Guid MasterGuid, short tryCount, string tranName)
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

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid MasterGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<Sanad_DetBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<Sanad_DetBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<Sanad_DetBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(Sanad_DetBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<Sanad_DetBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<Sanad_DetBusiness> items, SanadBusiness master, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 484);
		try
		{
			if (master == null)
			{
				return res;
			}
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			foreach (Sanad_DetBusiness item in items)
			{
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@SanadGuid", (object)master.Guid),
					new SqlParameter("@Description", (object)(item.RowDesc ?? "")),
					new SqlParameter("@Debit", (object)item.Debit),
					new SqlParameter("@Credit", (object)item.Credit),
					new SqlParameter("@MoeinGuid", (object)item.MoeinGuid),
					new SqlParameter("@GUID", (object)item.Guid),
					new SqlParameter("@QT", (object)item.Qt),
					new SqlParameter("@RowDesc", (object)(item.RowDesc ?? "")),
					new SqlParameter("@SGuid", (object)item.AsGuid),
					new SqlParameter("@Stype", (object)item.AsType),
					new SqlParameter("@Serial", (object)item.Serial)
				};
				if (item.TafsilGuid.HasValue)
				{
					list.Add(new SqlParameter("@TafsilGuid", (object)item.TafsilGuid));
				}
				if (item.DecimalGuid.HasValue)
				{
					list.Add(new SqlParameter("@DecimalGuid", (object)item.DecimalGuid));
				}
				if (item.DateSarresid.HasValue)
				{
					list.Add(new SqlParameter("@DateSarresid", (object)item.DateSarresid));
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_SanadDetail_Insert", list, tranName, default(CancellationToken), "", CommandType.StoredProcedure, 30, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs"), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 509);
			}
		}
		catch (Exception ex)
		{
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 514);
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccV5SqlServerPersistence\\Version5Repositories\\Sanad\\SanadDetailV5PersistenceRepository.cs", 515);
		}
		return res;
	}

	public Task<IEnumerable<Sanad_DetBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Sanad_DetBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}
}
