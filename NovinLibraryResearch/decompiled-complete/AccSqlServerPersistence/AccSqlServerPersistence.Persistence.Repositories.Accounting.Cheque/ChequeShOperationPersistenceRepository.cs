using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Cheque;

internal class ChequeShOperationPersistenceRepository : ChequeShOperationPersistenceRepositoryBase
{
	public ChequeShOperationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<List<ChequeShInstantModel>> GetAllChequeShInstantAsync(DateTime dateM, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllChequeShInstantAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepository.cs", 16);
		List<ChequeShInstantModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ChequeShInstantModel> list = new List<ChequeShInstantModel>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@DateM", (object)dateM)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ChequeSh_Instant", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllChequeShInstantAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadDataInstant(dr));
				}
				result = list;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllChequeShInstantAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepository.cs", 30);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllChequeShInstantAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepository.cs");
			}
		}
		return result;
	}

	private ChequeShInstantModel LoadDataInstant(DbDataReader dr)
	{
		try
		{
			ChequeShInstantModel chequeShInstantModel = new ChequeShInstantModel();
			if (dr["NewState"] != DBNull.Value)
			{
				chequeShInstantModel.Status = (ENCheckSHStatus)dr["NewState"];
			}
			if (dr["OpGuid"] != DBNull.Value)
			{
				chequeShInstantModel.OperationGuid = (Guid)dr["OpGuid"];
			}
			if (dr["Guid"] != DBNull.Value)
			{
				chequeShInstantModel.ChequeGuid = (Guid)dr["Guid"];
			}
			return chequeShInstantModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataInstant", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeShOperationPersistenceRepository.cs", 53);
			throw;
		}
	}
}
