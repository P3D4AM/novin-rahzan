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

internal class ChequeMOperationPersistenceRepository : ChequeMOperationPersistenceRepositoryBase
{
	public ChequeMOperationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<List<MoghayeratChequeAvalDoreModel>> GetAllAvalDoreMoghyerat(Guid chequeMojodGuid, Guid chequeVagozarGuid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllAvalDoreMoghyerat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs", 16);
		List<MoghayeratChequeAvalDoreModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<MoghayeratChequeAvalDoreModel> list = new List<MoghayeratChequeAvalDoreModel>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@MoeinGuid_Vagozar", (object)chequeVagozarGuid),
					new SqlParameter("@MoeinGuid_Mojod", (object)chequeMojodGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Moghayerat_ChequeAdore", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAvalDoreMoghyerat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs");
				while (await dr.ReadAsync())
				{
					list.Add(LoadData(dr));
				}
				dr.Close();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAvalDoreMoghyerat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs", 36);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAvalDoreMoghyerat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<List<ChequeMInstantModel>> GetAllChequeMInstantAsync(DateTime dateM, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllChequeMInstantAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs", 47);
		List<ChequeMInstantModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<ChequeMInstantModel> list = new List<ChequeMInstantModel>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@DateM", (object)dateM)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_ChequeM_Instant", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllChequeMInstantAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllChequeMInstantAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs", 61);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllChequeMInstantAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs");
			}
		}
		return result;
	}

	private MoghayeratChequeAvalDoreModel LoadData(DbDataReader dr)
	{
		try
		{
			MoghayeratChequeAvalDoreModel moghayeratChequeAvalDoreModel = new MoghayeratChequeAvalDoreModel();
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				moghayeratChequeAvalDoreModel.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				moghayeratChequeAvalDoreModel.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["MoeinName"] != DBNull.Value)
			{
				moghayeratChequeAvalDoreModel.MoeinName = dr["MoeinName"].ToString();
			}
			if (dr["TafsilName"] != DBNull.Value)
			{
				moghayeratChequeAvalDoreModel.TafsilName = dr["TafsilName"].ToString();
			}
			if (dr["SumAvalDoreFormCheque"] != DBNull.Value)
			{
				moghayeratChequeAvalDoreModel.SumAvalDoreFromCheque = (decimal)dr["SumAvalDoreFormCheque"];
			}
			if (dr["SumAvalDoreFromSanad"] != DBNull.Value)
			{
				moghayeratChequeAvalDoreModel.SumAvalDoreFromSanad = (decimal)dr["SumAvalDoreFromSanad"];
			}
			return moghayeratChequeAvalDoreModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs", 87);
			throw;
		}
	}

	private ChequeMInstantModel LoadDataInstant(DbDataReader dr)
	{
		try
		{
			ChequeMInstantModel chequeMInstantModel = new ChequeMInstantModel();
			if (dr["NewState"] != DBNull.Value)
			{
				chequeMInstantModel.Status = (ENUMCheck)dr["NewState"];
			}
			if (dr["OpGuid"] != DBNull.Value)
			{
				chequeMInstantModel.OperationGuid = (Guid)dr["OpGuid"];
			}
			if (dr["Guid"] != DBNull.Value)
			{
				chequeMInstantModel.ChequeGuid = (Guid)dr["Guid"];
			}
			return chequeMInstantModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadDataInstant", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeMOperationPersistenceRepository.cs", 105);
			throw;
		}
	}
}
