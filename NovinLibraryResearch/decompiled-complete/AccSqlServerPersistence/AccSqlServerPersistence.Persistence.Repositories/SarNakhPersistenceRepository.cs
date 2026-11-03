using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Global.Tasks.SarNakh;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Filters.Global;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Global.Tasks;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class SarNakhPersistenceRepository : SarNakhPersistenceRepositoryBase
{
	public SarNakhPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<SarNakhReportBusiness>> GetAllAsync(SarNakhFilter filter, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\SarNakhPersistenceRepository.cs", 19);
		IEnumerable<SarNakhReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SarNakhReportBusiness> list = new List<SarNakhReportBusiness>();
				List<SqlParameter> list2 = new List<SqlParameter>();
				if (filter?.Status.HasValue ?? false)
				{
					list2.Add(new SqlParameter("@Status", (object)filter.Status.Value));
				}
				if (filter?.StatusId.HasValue ?? false)
				{
					list2.Add(new SqlParameter("@StatusId", (object)filter.StatusId.Value));
				}
				if (filter?.DateFrom.HasValue ?? false)
				{
					list2.Add(new SqlParameter("@DateFrom", (object)filter.DateFrom.Value));
				}
				if (filter?.DateTo.HasValue ?? false)
				{
					list2.Add(new SqlParameter("@DateTo", (object)filter.DateTo.Value));
				}
				if ((filter?.CashierGuid ?? Guid.Empty) != Guid.Empty)
				{
					list2.Add(new SqlParameter("@CashierGuid", (object)filter.CashierGuid.Value));
				}
				if ((filter?.IntroductionGuid ?? Guid.Empty) != Guid.Empty)
				{
					list2.Add(new SqlParameter("@IntroductionGuid", (object)filter.IntroductionGuid.Value));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Global_Sarnakh_GetAll", list2, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\SarNakhPersistenceRepository.cs");
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					result = null;
				}
				else
				{
					while (true)
					{
						if (await dr.ReadAsync())
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\SarNakhPersistenceRepository.cs", 44);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\SarNakhPersistenceRepository.cs");
			}
		}
		return result;
	}

	private SarNakhReportBusiness LoadData(DbDataReader dr)
	{
		try
		{
			SarNakhReportBusiness sarNakhReportBusiness = new SarNakhReportBusiness();
			if (dr["Guid"] != DBNull.Value)
			{
				sarNakhReportBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["Company"] != DBNull.Value)
			{
				sarNakhReportBusiness.Company = dr["Company"].ToString();
			}
			if (dr["FullName"] != DBNull.Value)
			{
				sarNakhReportBusiness.FullName = dr["FullName"].ToString();
			}
			if (dr["CashierGuid"] != DBNull.Value)
			{
				sarNakhReportBusiness.CashierGuid = (Guid)dr["CashierGuid"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				sarNakhReportBusiness.Description = dr["Description"].ToString();
			}
			if (dr["Modified"] != DBNull.Value)
			{
				sarNakhReportBusiness.Modified = (DateTime)dr["Modified"];
			}
			if (dr["Status"] != DBNull.Value)
			{
				sarNakhReportBusiness.Status = (bool)dr["Status"];
			}
			if (dr["StatusId"] != DBNull.Value)
			{
				sarNakhReportBusiness.StatusId = (short)dr["StatusId"];
			}
			if (dr["IntroductionGuid"] != DBNull.Value)
			{
				sarNakhReportBusiness.IntroductionGuid = (Guid)dr["IntroductionGuid"];
			}
			if (dr["CashierName"] != DBNull.Value)
			{
				sarNakhReportBusiness.CashierName = dr["CashierName"].ToString();
			}
			if (dr["StatusName"] != DBNull.Value)
			{
				sarNakhReportBusiness.StatusName = dr["StatusName"].ToString();
			}
			if (dr["IntroName"] != DBNull.Value)
			{
				sarNakhReportBusiness.IntroName = dr["IntroName"].ToString();
			}
			if (dr["CreationDate"] != DBNull.Value)
			{
				sarNakhReportBusiness.CreationDate = (DateTime)dr["CreationDate"];
			}
			if (dr["IsLinked"] != DBNull.Value)
			{
				sarNakhReportBusiness.IsChangedToCustomer = dr["IsLinked"].ToString().ParseToBoolean();
			}
			return sarNakhReportBusiness;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\SarNakhPersistenceRepository.cs", 78);
			throw;
		}
	}
}
