using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class CheckedSavePersistenceRepository : CheckedSavePersistenceRepositoryBase
{
	public CheckedSavePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> DeleteAsync(checkedTablReportType reportType, Guid hesabGuid, Guid detailGuid, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepository.cs"))
		{
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext obj = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepository.cs", 28));
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@ReportType", (object)(short)reportType),
					new SqlParameter("@entityGuid", (object)hesabGuid),
					new SqlParameter("@DetailGuid", (object)detailGuid)
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("sp_Acc_CheckedSave_Delete", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepository.cs"), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepository.cs", 35);
				return ret;
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
				WebErrorLog.Instance.StartLog(ex2, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepository.cs", 41);
				throw;
			}
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CheckedSaveBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Global_ModelContext obj = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepository.cs", 51));
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@ReportType", (object)(short)item.ReportType),
				new SqlParameter("@entityGuid", (object)item.EntityGuid),
				new SqlParameter("@DetailGuid", (object)item.DetailGuid),
				new SqlParameter("@ISDebit", (object)item.IsDebit),
				new SqlParameter("@Guid", (object)item.Guid),
				new SqlParameter("@dateM", (object)item.DateM)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("sp_Acc_CheckedSave_Save", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepository.cs"), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepository.cs", 61);
			return res;
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
			WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\CheckedSavePersistenceRepository.cs", 67);
			throw;
		}
	}
}
