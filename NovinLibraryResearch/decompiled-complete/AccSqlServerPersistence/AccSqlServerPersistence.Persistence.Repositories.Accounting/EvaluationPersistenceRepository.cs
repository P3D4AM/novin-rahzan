using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache;
using PacketParser.BusinessLayer.Models.Evaluation;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class EvaluationPersistenceRepository : EvaluationBasePersistenceRepository
{
	public EvaluationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<List<EvaluationDetailModel>> GetAllDetailsAsync(Guid? guid, DateTime dateM, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\EvaluationPersistenceRepository.cs"))
		{
			List<EvaluationDetailModel> list = new List<EvaluationDetailModel>();
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\EvaluationPersistenceRepository.cs", 24));
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				List<EvaluationDetailModel> result;
				try
				{
					short digitForRound = ClsCache.GetCache(ContextGuid).MultiLanguageConfig.DigitForRound;
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("@DateM", (object)dateM),
						new SqlParameter("@MasterGuid", (object)guid)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Evaluation_GetDetails", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\EvaluationPersistenceRepository.cs");
					while (true)
					{
						if (!(await dr.ReadAsync(token)))
						{
							dr.Close();
							break;
						}
						if (!token.IsCancellationRequested)
						{
							list.Add(LoadData(dr, digitForRound));
							continue;
						}
						result = null;
						goto end_IL_0000;
					}
					goto end_IL_00e4;
					end_IL_0000:;
				}
				catch (SqlException val)
				{
					SqlException val2 = val;
					if (!((Exception)(object)val2).Message.Contains("By User"))
					{
						WebErrorLog.Instance.StartLog((Exception)(object)val2, "", "GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\EvaluationPersistenceRepository.cs", 44);
					}
					goto end_IL_00e4;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\EvaluationPersistenceRepository.cs", 50);
					throw;
				}
				return result;
				end_IL_00e4:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\EvaluationPersistenceRepository.cs");
				}
			}
			return list;
		}
	}

	private EvaluationDetailModel LoadData(DbDataReader dr, short digitForRound)
	{
		try
		{
			EvaluationDetailModel evaluationDetailModel = new EvaluationDetailModel(digitForRound);
			if (dr["ProductGuid"] != DBNull.Value)
			{
				evaluationDetailModel.ProductGuid = (Guid)dr["ProductGuid"];
			}
			if (dr["ProductName"] != DBNull.Value)
			{
				evaluationDetailModel.ProductName = dr["ProductName"].ToString();
			}
			if (dr["ProductCode"] != DBNull.Value)
			{
				evaluationDetailModel.IdProduct = dr["ProductCode"].ToString().Trim();
			}
			if (dr["ProductGroupGuid"] != DBNull.Value)
			{
				evaluationDetailModel.ProductGroupGuid = (Guid)dr["ProductGroupGuid"];
			}
			if (dr["ProductGroupName"] != DBNull.Value)
			{
				evaluationDetailModel.ProductGroupName = dr["ProductGroupName"].ToString();
			}
			if (dr["UnitName"] != DBNull.Value)
			{
				evaluationDetailModel.UnitName = dr["UnitName"].ToString();
			}
			if (dr["LastDate"] != DBNull.Value)
			{
				evaluationDetailModel.LastEvaluationDateM = (DateTime)dr["LastDate"];
			}
			if (dr["LastQuantity"] != DBNull.Value)
			{
				evaluationDetailModel.LastQuantity = (decimal)(double)dr["LastQuantity"];
			}
			if (dr["LastUnitPrice"] != DBNull.Value)
			{
				evaluationDetailModel.LastKardexPrice = (decimal)dr["LastUnitPrice"];
			}
			if (dr["NewPrice"] != DBNull.Value)
			{
				evaluationDetailModel.NewPrice = (decimal)dr["NewPrice"];
			}
			return evaluationDetailModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\EvaluationPersistenceRepository.cs", 81);
			throw;
		}
	}
}
