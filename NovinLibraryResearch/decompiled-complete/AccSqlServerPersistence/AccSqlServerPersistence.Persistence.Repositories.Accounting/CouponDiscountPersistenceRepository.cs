using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Accounting.DiscountCoupon.Reports;
using PacketParser.Entities;
using PacketParser.Filters.CouponDiscount;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.DiscountCoupon;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class CouponDiscountPersistenceRepository : CouponDiscountPersistenceRepositoryBase
{
	public CouponDiscountPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<Guid>> GetAllOrderRelationAsync(Guid tafsilGuid, DateTime orderDate, decimal orderSum, int refId, Guid? specificCouponGuid, long mobile, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 22);
		IEnumerable<Guid> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<Guid> list = new List<Guid>();
				List<SqlParameter> list2 = new List<SqlParameter>
				{
					new SqlParameter("@TafsilGuid", (object)tafsilGuid),
					new SqlParameter("@OrderDate", (object)orderDate),
					new SqlParameter("@OrderTotalSum", (object)orderSum),
					new SqlParameter("@RefrenceId", (object)refId),
					new SqlParameter("@SanadPrintNumber", (object)mobile)
				};
				if ((specificCouponGuid ?? Guid.Empty) != Guid.Empty)
				{
					list2.Add(new SqlParameter("@SpecificCouponGuid", (object)specificCouponGuid));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("Acc_Coupon_SelectContainsForOrder", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					result = null;
				}
				else
				{
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (base.IsDisposed || token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							if (dr["CouponGuid"] != DBNull.Value)
							{
								list.Add((Guid)dr["CouponGuid"]);
							}
							continue;
						}
						result = list;
						break;
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 50);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllOrderRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<CouponDiscountReportBusiness>> GetAllAsync(CouponFilter filter, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 61);
		IEnumerable<CouponDiscountReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<CouponDiscountReportBusiness> list = new List<CouponDiscountReportBusiness>();
				List<SqlParameter> list2 = null;
				if (filter != null)
				{
					list2 = new List<SqlParameter>();
					if (filter.Type.HasValue)
					{
						list2.Add(new SqlParameter("@Type", (object)(short)filter.Type.Value));
					}
					if (filter.Status.HasValue)
					{
						list2.Add(new SqlParameter("@Status", (object)filter.Status));
					}
					if (filter.DateFrom.HasValue)
					{
						list2.Add(new SqlParameter("@DateFrom", (object)filter.DateFrom));
					}
					if (filter.DateTo.HasValue)
					{
						list2.Add(new SqlParameter("@DateTo", (object)filter.DateTo));
					}
					if (!string.IsNullOrEmpty(filter.CouponCode))
					{
						list2.Add(new SqlParameter("@CouponCode", (object)filter.CouponCode));
					}
					if (filter.ServerStatus != 0)
					{
						list2.Add(new SqlParameter("@ServerStatus", (object)(short)filter.ServerStatus));
					}
					if ((filter.CashierGuid ?? Guid.Empty) != Guid.Empty)
					{
						list2.Add(new SqlParameter("@CashierGuid", (object)filter.CashierGuid.Value));
					}
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Coupon_SelectAll", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					result = null;
				}
				else
				{
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (base.IsDisposed || token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							list.Add(LoadData(dr));
							continue;
						}
						if ((filter?.ModeList?.Any()).GetValueOrDefault())
						{
							list = list?.Where((CouponDiscountReportBusiness q) => filter.ModeList.Contains(q.Mode))?.ToList();
						}
						if ((filter?.UsingStatus ?? EnCouponUsageType.None) != 0)
						{
							if (filter.UsingStatus == EnCouponUsageType.NotUsed)
							{
								list = list?.Where((CouponDiscountReportBusiness q) => q.RemainCount > 0 && q.DateTo.HasValue && q.DateTo.Value > DateTime.Now && q.Mode == EnCouponMode.Coupon)?.ToList();
							}
							else if (filter.UsingStatus == EnCouponUsageType.Used)
							{
								list = list?.Where((CouponDiscountReportBusiness q) => q.RemainCount <= 0 || (q.DateTo.HasValue && q.DateTo.Value <= DateTime.Now) || q.Mode != EnCouponMode.Coupon)?.ToList();
							}
						}
						result = list;
						break;
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 102);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<Guid>> GetAllForScheduleAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllForScheduleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 113);
		IEnumerable<Guid> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<Guid> list = new List<Guid>();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Coupon_GetBySchedule", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllForScheduleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					result = null;
				}
				else
				{
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (base.IsDisposed || token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							if (dr["Guid"] != DBNull.Value)
							{
								list.Add((Guid)dr["Guid"]);
							}
							continue;
						}
						result = list;
						break;
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllForScheduleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 130);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllForScheduleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<int> GetUsedCountAsync(Guid guid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetUsedCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 141);
		int result;
		try
		{
			_ = 1;
			try
			{
				new List<Guid>();
				result = await c.ExecuteScalarAsync<int>("sp_Acc_Coupon_GetUsedCount", new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				}, tranName, token, 30, "GetUsedCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetUsedCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 152);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetUsedCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<CouponDiscountUsagesBusiness>> GetAllUagesAsync(Guid guid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllUagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 163);
		IEnumerable<CouponDiscountUsagesBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<CouponDiscountUsagesBusiness> list = new List<CouponDiscountUsagesBusiness>();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Coupon_GetUsages", new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				}, tranName, token, CommandType.StoredProcedure, 30, "GetAllUagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					result = null;
				}
				else
				{
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (base.IsDisposed || token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							list.Add(LoadDataUsages(dr));
							continue;
						}
						result = list;
						break;
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllUagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 181);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllUagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs");
			}
		}
		return result;
	}

	private CouponDiscountReportBusiness LoadData(DbDataReader dr)
	{
		try
		{
			CouponDiscountReportBusiness couponDiscountReportBusiness = new CouponDiscountReportBusiness();
			if (dr["Guid"] != DBNull.Value)
			{
				couponDiscountReportBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				couponDiscountReportBusiness.Status = (bool)dr["Status"];
			}
			if (dr["Title"] != DBNull.Value)
			{
				couponDiscountReportBusiness.Title = dr["Title"].ToString();
			}
			if (dr["DateFrom"] != DBNull.Value)
			{
				couponDiscountReportBusiness.DateFrom = (DateTime)dr["DateFrom"];
			}
			if (dr["DateTo"] != DBNull.Value)
			{
				couponDiscountReportBusiness.DateTo = (DateTime)dr["DateTo"];
			}
			if (dr["UserName"] != DBNull.Value)
			{
				couponDiscountReportBusiness.UserName = dr["UserName"].ToString();
			}
			if (dr["CountCouponUseForUser"] != DBNull.Value)
			{
				couponDiscountReportBusiness.CountCouponUseForUser = (int)dr["CountCouponUseForUser"];
			}
			if (dr["CountCouponUse"] != DBNull.Value)
			{
				couponDiscountReportBusiness.CountCouponUse = (int)dr["CountCouponUse"];
			}
			if (dr["ServerStatus"] != DBNull.Value)
			{
				couponDiscountReportBusiness.ServerStatus = (ServerStatusForClient)dr["ServerStatus"];
			}
			if (dr["Type"] != DBNull.Value)
			{
				couponDiscountReportBusiness.Type = (EnCouponType)dr["Type"];
			}
			if (dr["Mode"] != DBNull.Value)
			{
				couponDiscountReportBusiness.Mode = (EnCouponMode)dr["Mode"];
			}
			if (dr["UsesCount"] != DBNull.Value)
			{
				couponDiscountReportBusiness.UsedCount = (int)dr["UsesCount"];
			}
			return couponDiscountReportBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 212);
			throw;
		}
	}

	private CouponDiscountUsagesBusiness LoadDataUsages(DbDataReader dr)
	{
		try
		{
			CouponDiscountUsagesBusiness couponDiscountUsagesBusiness = new CouponDiscountUsagesBusiness(ContextGuid);
			if (dr["CouponGuid"] != DBNull.Value)
			{
				couponDiscountUsagesBusiness.CouponGuid = (Guid)dr["CouponGuid"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				couponDiscountUsagesBusiness.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			return couponDiscountUsagesBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataUsages", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CouponDiscountPersistenceRepository.cs", 227);
			throw;
		}
	}
}
