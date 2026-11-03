using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Aghsat;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Aghsat;
using PacketParser.Entities;
using PacketParser.Filters.Configs;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Installments;

internal class AghsatMPersistanceRepository : AghsatMPersistanceRepositoryBase
{
	public AghsatMPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<AghsatMDelayModel>> GetAllDelayedInstallmentsAsync(AghsatMDelayedConfig config, CancellationToken token, string tranName)
	{
		BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "GetAllDelayedInstallmentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs", 20);
		IEnumerable<AghsatMDelayModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<AghsatMDelayModel> list = new List<AghsatMDelayModel>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@MaxDelayDayCount", (object)config.MaxDelayDayCount),
					new SqlParameter("@dateM", (object)config.DateM),
					new SqlParameter("@PenaltyPercent", (object)config.PenaltyPercent)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_AghsatM_DelayedInstallments", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllDelayedInstallmentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllDelayedInstallmentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs", 42);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllDelayedInstallmentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateReceptionInfoAsync(Guid? receptionGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs", 57);
			List<SqlParameter> parameters = null;
			if ((receptionGuid ?? Guid.Empty) != Guid.Empty)
			{
				parameters = new List<SqlParameter>
				{
					new SqlParameter("@ReceptionGuid", (object)receptionGuid)
				};
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("sp_Acc_AghsatAshkhas_UpdateReceptionInfo", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs"), ReturnedState.None, "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs", 60);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs", 64);
			res.AddReturnedValue(ex, "", "UpdateReceptionInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs", 65);
		}
		return res;
	}

	private AghsatMDelayModel LoadData(DbDataReader dr)
	{
		try
		{
			AghsatMDelayModel aghsatMDelayModel = new AghsatMDelayModel();
			if (dr["Guid"] != DBNull.Value)
			{
				aghsatMDelayModel.Guid = (Guid)dr["Guid"];
			}
			if (dr["VajhEltezam"] != DBNull.Value)
			{
				aghsatMDelayModel.VajhEltezam = (decimal)dr["VajhEltezam"];
			}
			if (dr["DateSarResid"] != DBNull.Value)
			{
				aghsatMDelayModel.DateSarresidM = (DateTime)dr["DateSarResid"];
			}
			if (dr["EltezamStartDate"] != DBNull.Value)
			{
				aghsatMDelayModel.EltezamStartDateM = (DateTime)dr["EltezamStartDate"];
			}
			if (dr["DelayDayCount"] != DBNull.Value)
			{
				aghsatMDelayModel.DelayDayCount = (int)dr["DelayDayCount"];
			}
			if (dr["TafsilGuid"] != DBNull.Value)
			{
				aghsatMDelayModel.TafsilGuid = (Guid)dr["TafsilGuid"];
			}
			if (dr["Code"] != DBNull.Value)
			{
				aghsatMDelayModel.TafsilCode = (long)dr["Code"];
			}
			if (dr["Name"] != DBNull.Value)
			{
				aghsatMDelayModel.TafsilName = dr["Name"].ToString();
			}
			if (dr["MoeinGuid"] != DBNull.Value)
			{
				aghsatMDelayModel.MoeinGuid = (Guid)dr["MoeinGuid"];
			}
			if (dr["Id2"] != DBNull.Value)
			{
				aghsatMDelayModel.Id2 = (int)dr["Id2"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				aghsatMDelayModel.Description = dr["Description"].ToString();
			}
			if (dr["Price"] != DBNull.Value)
			{
				aghsatMDelayModel.Price = (decimal)dr["Price"];
			}
			if (dr["SanadNo"] != DBNull.Value)
			{
				aghsatMDelayModel.SanadNo = (int)dr["SanadNo"];
			}
			if (dr["PenaltyPrice"] != DBNull.Value)
			{
				aghsatMDelayModel.PenaltyPrice = (decimal)dr["PenaltyPrice"];
			}
			if (dr["VajhEltezamModified"] != DBNull.Value)
			{
				aghsatMDelayModel.VajhEltezamModified = (DateTime)dr["VajhEltezamModified"];
			}
			return aghsatMDelayModel;
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
			WebErrorLog.Instance.StartLog(ex2, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatMPersistanceRepository.cs", 95);
			throw;
		}
	}
}
