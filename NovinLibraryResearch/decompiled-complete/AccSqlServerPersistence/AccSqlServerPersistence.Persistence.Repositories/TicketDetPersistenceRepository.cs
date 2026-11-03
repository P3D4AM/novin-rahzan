using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Tickets;
using EntityCache.Business.Global.Tickets;
using PacketParser.Entities;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class TicketDetPersistenceRepository : TicketDetPersistenceRepositoryBase
{
	public TicketDetPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketDetFilter filter, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketDetPersistenceRepository.cs", 20);
		IEnumerable<TicketReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<TicketReportBusiness> list = new List<TicketReportBusiness>();
				if (filter == null)
				{
					throw new ArgumentNullException();
				}
				List<SqlParameter> list2 = new List<SqlParameter>();
				if ((filter?.CashierGuid ?? Guid.Empty) != Guid.Empty)
				{
					list2.Add(new SqlParameter("@CashierGuid", (object)filter.CashierGuid.Value));
				}
				if ((filter?.TafsilGuid ?? Guid.Empty) != Guid.Empty)
				{
					list2.Add(new SqlParameter("@TafsilGuid", (object)filter.TafsilGuid.Value));
				}
				if ((filter?.MasterGuid ?? Guid.Empty) != Guid.Empty)
				{
					list2.Add(new SqlParameter("@MasterGuid", (object)filter.MasterGuid.Value));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Global_TicketDet_SelectAll", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketDetPersistenceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketDetPersistenceRepository.cs", 42);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketDetPersistenceRepository.cs");
			}
		}
		return result;
	}

	private TicketReportBusiness LoadData(DbDataReader dr)
	{
		try
		{
			TicketReportBusiness ticketReportBusiness = new TicketReportBusiness();
			if (dr["Guid"] != DBNull.Value)
			{
				ticketReportBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Number"] != DBNull.Value)
			{
				ticketReportBusiness.Number = (int)dr["Number"];
			}
			if (dr["Title"] != DBNull.Value)
			{
				ticketReportBusiness.Title = dr["Title"].ToString();
			}
			if (dr["DateM"] != DBNull.Value)
			{
				ticketReportBusiness.DateM = (DateTime)dr["DateM"];
			}
			if (dr["TargetGuid"] != DBNull.Value)
			{
				ticketReportBusiness.TargetGuid = (Guid)dr["TargetGuid"];
			}
			if (dr["TargetType"] != DBNull.Value)
			{
				ticketReportBusiness.TargetType = (EnTargetType)dr["TargetType"];
			}
			if (dr["Modified"] != DBNull.Value)
			{
				ticketReportBusiness.Modified = (DateTime)dr["Modified"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				ticketReportBusiness.Description = dr["Description"].ToString();
			}
			if (dr["ReferenceGuid"] != DBNull.Value)
			{
				ticketReportBusiness.ReferenceGuid = (Guid)dr["ReferenceGuid"];
			}
			if (dr["SourceGuid"] != DBNull.Value)
			{
				ticketReportBusiness.SourceGuid = (Guid)dr["SourceGuid"];
			}
			if (dr["SourceType"] != DBNull.Value)
			{
				ticketReportBusiness.SourceType = (EnTargetType)dr["SourceType"];
			}
			if (dr["StatusId"] != DBNull.Value)
			{
				ticketReportBusiness.StatusId = (short)dr["StatusId"];
			}
			if (dr["Priority"] != DBNull.Value)
			{
				ticketReportBusiness.Priority = (Priority)(short)dr["Priority"];
			}
			if (dr["SourceName"] != DBNull.Value)
			{
				ticketReportBusiness.SourceName = dr["SourceName"].ToString();
			}
			if (dr["PartName"] != DBNull.Value)
			{
				ticketReportBusiness.PartName = dr["PartName"].ToString();
			}
			if (dr["StatusName"] != DBNull.Value)
			{
				ticketReportBusiness.StatusName = dr["StatusName"].ToString();
			}
			if (dr["MasterGuid"] != DBNull.Value)
			{
				ticketReportBusiness.MasterGuid = (Guid)dr["MasterGuid"];
			}
			return ticketReportBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketDetPersistenceRepository.cs", 79);
			throw;
		}
	}
}
