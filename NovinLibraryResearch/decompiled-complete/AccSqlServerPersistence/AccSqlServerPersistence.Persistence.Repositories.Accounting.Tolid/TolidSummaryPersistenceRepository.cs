using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Tolid.Summary;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;

internal class TolidSummaryPersistenceRepository : TolidSummaryPersistenceRepositoryBase
{
	public TolidSummaryPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<List<TolidSummaryDetailBusiness>> GetAllAsync(TolidSummaryBusiness master, DateTime dateM, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepository.cs"))
		{
			List<TolidSummaryDetailBusiness> list = new List<TolidSummaryDetailBusiness>();
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepository.cs", 18));
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@DateM", (object)dateM),
						new SqlParameter("@MasterGuid", (object)master.Guid)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_TolidSummary_Details", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepository.cs");
					while (await dr.ReadAsync())
					{
						list.Add(LoadData(master, dr));
					}
					dr.Close();
				}
				catch (SqlException val)
				{
					SqlException val2 = val;
					if (!((Exception)(object)val2).Message.Contains("By User"))
					{
						WebErrorLog.Instance.StartLog((Exception)(object)val2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepository.cs", 33);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepository.cs", 39);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	private TolidSummaryDetailBusiness LoadData(TolidSummaryBusiness master, DbDataReader dr)
	{
		TolidSummaryDetailBusiness tolidSummaryDetailBusiness = new TolidSummaryDetailBusiness(ContextGuid);
		try
		{
			tolidSummaryDetailBusiness.MasterGuid = master?.Guid ?? Guid.Empty;
			tolidSummaryDetailBusiness.Modified = DateTime.Now;
			if (dr["PRDGuid"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.ProductGuid = (Guid)dr["PRDGuid"];
			}
			if (dr["PRDCode"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.PrdCode = dr["PRDCode"].ToString();
			}
			if (dr["PRDName"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.PrdName = dr["PRDName"].ToString();
			}
			if (dr["VahedName"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.VahedName = dr["VahedName"].ToString();
			}
			if (dr["GuidGroup"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.GroupGuid = (Guid)dr["GuidGroup"];
			}
			if (dr["GroupName"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.GroupName = dr["GroupName"].ToString();
			}
			if (dr["DPTGuid"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.DepotGuid = (Guid)dr["DPTGuid"];
			}
			if (dr["DPTName"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.DepotName = dr["DPTName"].ToString();
			}
			if (dr["RealAmount"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.RealAmount = (decimal)(double)dr["RealAmount"];
			}
			if (dr["AvailableAmount"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.AvailableAmount = (decimal)(double)dr["AvailableAmount"];
			}
			if (dr["ProducedAmount"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.ProducedAmount = (decimal)(double)dr["ProducedAmount"];
			}
			if (dr["Guid"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.Guid = (Guid)dr["Guid"];
			}
			else
			{
				tolidSummaryDetailBusiness.Guid = Guid.NewGuid();
			}
			if (dr["TolidGuid"] != DBNull.Value)
			{
				tolidSummaryDetailBusiness.TolidGuid = (Guid)dr["TolidGuid"];
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidSummaryPersistenceRepository.cs", 75);
			throw;
		}
		return tolidSummaryDetailBusiness;
	}
}
