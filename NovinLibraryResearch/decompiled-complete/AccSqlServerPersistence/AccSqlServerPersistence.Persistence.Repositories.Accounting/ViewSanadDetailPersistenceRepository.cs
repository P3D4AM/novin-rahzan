using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Records.Sanad;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ViewSanadDetailPersistenceRepository : ViewSanadDetailPersistenceRepositoryBase
{
	public ViewSanadDetailPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<List<WebSanad_Det>> GetAllSanadDetailsAsync(int sanadNo, CancellationToken token, string tranName = "")
	{
		List<WebSanad_Det> list = new List<WebSanad_Det>();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 23);
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
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_ViewSanadDetail_SelectRow_SanadNO", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDetails(dr));
				}
				dr.Close();
				list = list.OrderBy((WebSanad_Det p) => p.Ordinal).ToList();
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 37);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllSanadDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs");
			}
		}
		return list;
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAutoArticlesAsync(Document asType, Guid asGuid, IEnumerable<Sanad_DetBusiness> containsItems, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAutoArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 52));
			List<ViewSanadDetail> list = await EntityFrameworkQueryableExtensions.ToListAsync<ViewSanadDetail>(((IQueryable<ViewSanadDetail>)c.ViewSanadDetail).Where((ViewSanadDetail q) => q.AsGuid == asGuid && q.AsType == asType), token);
			if (list == null || !list.Any())
			{
				return res;
			}
			if (containsItems?.Any() ?? false)
			{
				list = list.Where((ViewSanadDetail q) => !containsItems.Select((Sanad_DetBusiness q) => q.Guid).Contains(q.Guid))?.ToList();
			}
			if (list == null || !list.Any())
			{
				return res;
			}
			c.ViewSanadDetail.RemoveRange((IEnumerable<ViewSanadDetail>)list);
		}
		catch (Exception ex)
		{
			res.AddReturnedValue(ex, "", "RemoveAutoArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 62);
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAutoArticlesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 63);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string caller, SanadBusiness sanadMaster, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 70);
		try
		{
			if (sanadMaster == null)
			{
				return res;
			}
			if (!(sanadMaster.ListSanad_Det?.Any() ?? false))
			{
				return res;
			}
			foreach (Sanad_DetBusiness item in sanadMaster.ListSanad_Det)
			{
				if (item.AsType == Document.CheckSanad && sanadMaster.SanadNo <= 0)
				{
					sanadMaster.SanadNo = await sanadMaster.GetSanadNoAsync(token, tranName);
				}
				List<SqlParameter> list = new List<SqlParameter>
				{
					new SqlParameter("@date", (object)sanadMaster.DateM),
					new SqlParameter("@desc", (object)item.RowDesc),
					new SqlParameter("@asType", (object)(short)item.AsType),
					new SqlParameter("@debit", (object)item.Debit),
					new SqlParameter("@credit", (object)item.Credit),
					new SqlParameter("@sanadNo", (object)sanadMaster.SanadNo),
					new SqlParameter("@sanadSt", (object)(short)sanadMaster.SanadSt),
					new SqlParameter("@guid", (object)item.Guid),
					new SqlParameter("@qt", (object)item.Qt),
					new SqlParameter("@rowDesc", (object)item.RowDesc),
					new SqlParameter("@moeinGuid", (object)item.MoeinGuid),
					new SqlParameter("@cashierGuid", (object)sanadMaster.UserGuid),
					new SqlParameter("@serial", (object)item.Serial)
				};
				if (item.DecimalGuid.HasValue && item.DecimalGuid != Guid.Empty)
				{
					list.Add(new SqlParameter("@decimalGuid", (object)item.DecimalGuid));
				}
				if ((item?.TafsilGuid ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("@tafsilGuid", (object)item.TafsilGuid.Value));
				}
				if ((item?.TafsilGuid2 ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("@tafsilGuid2", (object)item.TafsilGuid2.Value));
				}
				if ((item?.TafsilGuid3 ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("@tafsilGuid3", (object)item.TafsilGuid3.Value));
				}
				_ = item.AsGuid;
				if (item.AsGuid == Guid.Empty)
				{
					throw new ArgumentException("asGuid is not valid!");
				}
				list.Add(new SqlParameter("@asGuid", (object)item.AsGuid));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync("sp_Acc_ViewSanadDetail_Insert", list, tranName, token, "", CommandType.StoredProcedure, 1800, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs"), ReturnedState.None, "save_rangeAsync_" + caller, "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 101);
				token.ThrowIfCancellationRequested();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "caller:" + caller + " --- sp_VSanadDetail_Insert ", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 108);
			res.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 109);
		}
		return res;
	}

	public override async Task<int> GetFutureSanadCountAsync(DateTime lastDate, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetFutureSanadCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 115);
		int result;
		try
		{
			_ = 1;
			try
			{
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Date", (object)lastDate)
				};
				result = await c.ExecuteScalarAsync<int>("SELECT count(*) FROM [dbo].[Acc_ViewSanadDetail] Where datem>@date", parameters, tranName, token, 30, "GetFutureSanadCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", CommandType.Text);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetFutureSanadCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 126);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetFutureSanadCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<List<ViewSanadExistRecord>> GetAllExistRecordAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllExistRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 137);
		List<ViewSanadExistRecord> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ViewSanadExistRecord> list = new List<ViewSanadExistRecord>();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_ViewSanadDetail_GetAllExistRecord", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllExistRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (base.IsDisposed || token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						list.Add(LoadRecord(dr));
						continue;
					}
					result = list;
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllExistRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 153);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllExistRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs");
			}
		}
		return result;
	}

	private WebSanad_Det LoadDetails(DbDataReader dr)
	{
		try
		{
			WebSanad_Det webSanad_Det = new WebSanad_Det();
			if (dr["DateM"] != DBNull.Value)
			{
				webSanad_Det.DateM = (DateTime)dr["DateM"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				webSanad_Det.RowDesc = dr["Description"].ToString();
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
				webSanad_Det.SanadST = (SanadSt)(short)dr["SanadST"];
			}
			if (dr["GUID"] != DBNull.Value)
			{
				webSanad_Det.Guid = (Guid)dr["GUID"];
			}
			if (dr["QT"] != DBNull.Value)
			{
				webSanad_Det.Qt = (decimal)dr["QT"];
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
				webSanad_Det.AsType = (Document)(short)dr["ASType"];
			}
			if (dr["Ordinal"] != DBNull.Value)
			{
				webSanad_Det.Ordinal = (int)dr["Ordinal"];
			}
			if (dr["DecimalGuid"] != DBNull.Value)
			{
				webSanad_Det.DecimalGuid = (Guid)dr["DecimalGuid"];
			}
			if (dr["Serial"] != DBNull.Value)
			{
				webSanad_Det.Serial = dr["Serial"].ToString();
			}
			if (dr["TafsilGuid2"] != DBNull.Value)
			{
				webSanad_Det.TafsilGuid2 = (Guid)dr["TafsilGuid2"];
			}
			if (dr["TafsilGuid3"] != DBNull.Value)
			{
				webSanad_Det.TafsilGuid3 = (Guid)dr["TafsilGuid3"];
			}
			if (dr["TafsilGuid4"] != DBNull.Value)
			{
				webSanad_Det.TafsilGuid4 = (Guid)dr["TafsilGuid4"];
			}
			return webSanad_Det;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDetails", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 193);
			throw;
		}
	}

	private ViewSanadExistRecord LoadRecord(DbDataReader dr)
	{
		try
		{
			ViewSanadExistRecord viewSanadExistRecord = new ViewSanadExistRecord();
			if (dr["AsGuid"] != DBNull.Value)
			{
				viewSanadExistRecord.AsGuid = (Guid)dr["AsGuid"];
			}
			if (dr["AsType"] != DBNull.Value)
			{
				viewSanadExistRecord.AsType = (Document)(int)dr["AsType"];
			}
			return viewSanadExistRecord;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "LoadRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\ViewSanadDetailPersistenceRepository.cs", 210);
			throw;
		}
	}
}
