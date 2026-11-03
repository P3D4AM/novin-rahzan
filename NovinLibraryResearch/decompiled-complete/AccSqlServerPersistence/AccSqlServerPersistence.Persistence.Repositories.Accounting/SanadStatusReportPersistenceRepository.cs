using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Sanad;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class SanadStatusReportPersistenceRepository : SanadStatusReportPersistenceRepositoryBase
{
	public SanadStatusReportPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<SanadStatusReportErrorTimeModel>> GetAllErrorTimeAsync(CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllErrorTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs", 22);
		IEnumerable<SanadStatusReportErrorTimeModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<SanadStatusReportErrorTimeModel> list = new List<SanadStatusReportErrorTimeModel>();
				using DbDataReader dr = await c.ExecuteReaderAsync("select * from View_SanadStatusReport_ErrorTime", null, tranName, token, CommandType.Text, 30, "GetAllErrorTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllErrorTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs", 38);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllErrorTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<SanadStatusReportForceUpdateModel>> GetAllForceUpdateAsync(DateTime dateMoasser, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllForceUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllForceUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs", 50));
			IEnumerable<SanadStatusReportForceUpdateModel> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					List<SanadStatusReportForceUpdateModel> list = new List<SanadStatusReportForceUpdateModel>();
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@DateM", (object)dateMoasser)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_SanadStatusReport_GetAllForForceUpdate", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllForceUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs");
					while (true)
					{
						if (await dr.ReadAsync(token))
						{
							if (base.IsDisposed || token.IsCancellationRequested)
							{
								result = null;
								break;
							}
							list.Add(LoadForceUpdateData(dr));
							continue;
						}
						dr.Close();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllForceUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs", 69);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllForceUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	private SanadStatusReportErrorTimeModel LoadData(DbDataReader dr)
	{
		try
		{
			SanadStatusReportErrorTimeModel sanadStatusReportErrorTimeModel = new SanadStatusReportErrorTimeModel();
			if (dr["AsGuid"] != DBNull.Value)
			{
				sanadStatusReportErrorTimeModel.AsGuid = (Guid)dr["AsGuid"];
			}
			if (dr["AsType"] != DBNull.Value)
			{
				sanadStatusReportErrorTimeModel.AsType = (Document)dr["AsType"].ToString().ParseToShort(0);
			}
			if (dr["Id2"] != DBNull.Value)
			{
				sanadStatusReportErrorTimeModel.Id2 = (long)dr["Id2"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				sanadStatusReportErrorTimeModel.SanadDateM = (DateTime)dr["DateM"];
			}
			if (dr["ErrorDate"] != DBNull.Value)
			{
				sanadStatusReportErrorTimeModel.ErrorDateM = (DateTime)dr["ErrorDate"];
			}
			if (dr["StatusId"] != DBNull.Value)
			{
				sanadStatusReportErrorTimeModel.StatusId = (int)dr["StatusId"];
			}
			return sanadStatusReportErrorTimeModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs", 95);
			throw;
		}
	}

	private SanadStatusReportForceUpdateModel LoadForceUpdateData(DbDataReader dr)
	{
		try
		{
			SanadStatusReportForceUpdateModel sanadStatusReportForceUpdateModel = new SanadStatusReportForceUpdateModel();
			if (dr["SanadGuid"] != DBNull.Value)
			{
				sanadStatusReportForceUpdateModel.SanadGuid = (Guid)dr["SanadGuid"];
			}
			if (dr["SanadType"] != DBNull.Value)
			{
				sanadStatusReportForceUpdateModel.SanadType = (Document)dr["SanadType"].ToString().ParseToShort(0);
			}
			if (dr["StatusId"] != DBNull.Value)
			{
				sanadStatusReportForceUpdateModel.CurrentStatusId = (int)dr["StatusId"];
			}
			if (dr["SanadDate"] != DBNull.Value)
			{
				sanadStatusReportForceUpdateModel.SanadDate = (DateTime)dr["SanadDate"];
			}
			if (dr["InactivityDays"] != DBNull.Value)
			{
				sanadStatusReportForceUpdateModel.InactivityDays = (int)dr["InactivityDays"];
			}
			if (dr["ActivityDate"] != DBNull.Value)
			{
				sanadStatusReportForceUpdateModel.ActivityDate = (DateTime)dr["ActivityDate"];
			}
			if (dr["TargetStatusId"] != DBNull.Value)
			{
				sanadStatusReportForceUpdateModel.TargetStatusId = (int)dr["TargetStatusId"];
			}
			return sanadStatusReportForceUpdateModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadForceUpdateData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\SanadStatusReportPersistenceRepository.cs", 117);
			throw;
		}
	}
}
