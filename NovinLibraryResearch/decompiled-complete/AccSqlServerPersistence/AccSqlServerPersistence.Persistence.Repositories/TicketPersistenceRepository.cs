using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Tickets;
using EntityCache.Business.Global;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Report;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class TicketPersistenceRepository : TicketPersistenceRepositoryBase
{
	public TicketPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketFilter filter, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketPersistenceRepository.cs", 22);
		IEnumerable<TicketReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 4;
			try
			{
				List<TicketReportBusiness> list = new List<TicketReportBusiness>();
				List<SqlParameter> p = new List<SqlParameter>();
				if (filter != null)
				{
					if ((filter?.TargetGuid ?? Guid.Empty) != Guid.Empty)
					{
						p.Add(new SqlParameter("@TargetGuid", (object)filter.TargetGuid.Value));
					}
					if (filter.TargetType != 0)
					{
						p.Add(new SqlParameter("@TargetType", (object)filter.TargetType));
					}
					if (filter?.DateTimeFrom.HasValue ?? false)
					{
						p.Add(new SqlParameter("@DateTimeFrom", (object)filter.DateTimeFrom.Value));
					}
					if (filter?.DateTimeTo.HasValue ?? false)
					{
						p.Add(new SqlParameter("@DateTimeTo", (object)filter.DateTimeTo.Value));
					}
					if (filter.StatusId != null && !filter.StatusId.Contains(0) && (filter.StatusId?.Any() ?? false))
					{
						List<int> lst = ((IEnumerable<short>)filter.StatusId).Select((Func<short, int>)((short q) => q)).ToList();
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(ContextGuid, lst);
						if (!returnedSaveFuncInfoWithValue.HasError && returnedSaveFuncInfoWithValue.value > 0)
						{
							p.Add(new SqlParameter("@RefIdStatus", (object)returnedSaveFuncInfoWithValue.value));
						}
					}
					if (filter.Priorities != null && (filter?.Priorities.Any() ?? false))
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue2 = await StimulQueryBusiness.SaveAsync(ContextGuid, filter.Priorities.Select((Priority q) => (int)q).ToList().Distinct());
						if (!returnedSaveFuncInfoWithValue2.HasError && returnedSaveFuncInfoWithValue2.value > 0)
						{
							p.Add(new SqlParameter("@RefIdPriority", (object)returnedSaveFuncInfoWithValue2.value));
						}
					}
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Global_Ticket_SelectAll", p, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketPersistenceRepository.cs");
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
						if (list?.Any() ?? false)
						{
							List<IGrouping<short, TicketReportBusiness>> list2 = list?.GroupBy((TicketReportBusiness q) => q.StatusId)?.ToList();
							if (list2 != null && list2.Any())
							{
								foreach (IGrouping<short, TicketReportBusiness> statusId in list2)
								{
									if (statusId.Key <= 0)
									{
										continue;
									}
									SanadStatusReportBusiness sanadStatusReportBusiness = await SanadStatusReportBusiness.GetAsync(ContextGuid, statusId.Key, token, tranName);
									if (string.IsNullOrEmpty(sanadStatusReportBusiness?.MetaData?.ForeColor) || !(statusId?.Any() ?? false))
									{
										continue;
									}
									foreach (TicketReportBusiness item in statusId)
									{
										if (item != null)
										{
											item.StatusColor = sanadStatusReportBusiness.MetaData.ForeColor;
										}
									}
								}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketPersistenceRepository.cs", 77);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketPersistenceRepository.cs");
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
			if (dr["SourceType"] != DBNull.Value)
			{
				ticketReportBusiness.SourceType = (EnTargetType)dr["SourceType"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				ticketReportBusiness.Status = (bool)dr["Status"];
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\TicketPersistenceRepository.cs", 115);
			throw;
		}
	}
}
