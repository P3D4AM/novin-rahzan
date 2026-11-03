using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Reservation;
using PacketParser.Entities;
using PacketParser.Filters.Hotel;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.ReservationClass;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ReservationPersistanceRepository : ReservationBasePersistanceRepository
{
	public ReservationPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<ReservationReportBusiness>> GetAllModelAsync(ReservationFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ReservationPersistanceRepository.cs", 21));
		IEnumerable<ReservationReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ReservationReportBusiness> list = new List<ReservationReportBusiness>();
				List<SqlParameter> list2 = null;
				if (filter != null)
				{
					list2 = new List<SqlParameter>();
					if (filter.Status.HasValue)
					{
						list2.Add(new SqlParameter("@Status", (object)filter.Status.Value));
					}
					if (filter.DateFrom.HasValue)
					{
						list2.Add(new SqlParameter("@DateFrom", (object)filter.DateFrom.Value));
					}
					if (filter.DateTo.HasValue)
					{
						list2.Add(new SqlParameter("@DateTo", (object)filter.DateTo.Value));
					}
					if (filter.TafsilGuid.HasValue)
					{
						list2.Add(new SqlParameter("@TafsilGuid", (object)filter.TafsilGuid.Value));
					}
					if (filter.CashierGuid.HasValue)
					{
						list2.Add(new SqlParameter("@CashierGuid", (object)filter.CashierGuid.Value));
					}
					if (filter.NumberFrom != 0)
					{
						list2.Add(new SqlParameter("@NumberFrom", (object)filter.NumberFrom));
					}
					if (filter.NumberTo != 0)
					{
						list2.Add(new SqlParameter("@NumberTo", (object)filter.NumberTo));
					}
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Reservation_GetAll", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ReservationPersistanceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ReservationPersistanceRepository.cs", 49);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ReservationPersistanceRepository.cs");
			}
		}
		return result;
	}

	private ReservationReportBusiness LoadData(DbDataReader dr)
	{
		try
		{
			ReservationReportBusiness reservationReportBusiness = new ReservationReportBusiness();
			if (dr["Guid"] != DBNull.Value)
			{
				reservationReportBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				reservationReportBusiness.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["CashierName"] != DBNull.Value)
			{
				reservationReportBusiness.CashierName = dr["CashierName"].ToString();
			}
			if (dr["DateM"] != DBNull.Value)
			{
				reservationReportBusiness.DateM = (DateTime)dr["DateM"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				reservationReportBusiness.Description = dr["Description"].ToString();
			}
			if (dr["OrderGuid"] != DBNull.Value)
			{
				reservationReportBusiness.OrderGuid = (Guid?)dr["OrderGuid"];
			}
			if (dr["ReceptionGuid"] != DBNull.Value)
			{
				reservationReportBusiness.ReceptionGuid = (Guid?)dr["ReceptionGuid"];
			}
			if (dr["OrderSum"] != DBNull.Value)
			{
				reservationReportBusiness.OrderSum = (decimal)dr["OrderSum"];
			}
			if (dr["ReceptionSum"] != DBNull.Value)
			{
				reservationReportBusiness.ReceptionSum = (decimal)dr["ReceptionSum"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				reservationReportBusiness.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				reservationReportBusiness.TafsilName = dr["TafsilName"].ToString();
			}
			if (dr["Number"] != DBNull.Value)
			{
				reservationReportBusiness.Number = (int)dr["Number"];
			}
			return reservationReportBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ReservationPersistanceRepository.cs", 81);
			throw;
		}
	}
}
