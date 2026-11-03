using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repository.Accounting.Aghsat;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Installments;

internal class AghsatShPersistanceRepository : AghsatShPersistanceRepositoryBase
{
	public AghsatShPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> UpdatePardakhtInfoAsync(Guid? pardakhtGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatShPersistanceRepository.cs", 22);
			List<SqlParameter> parameters = null;
			if ((pardakhtGuid ?? Guid.Empty) != Guid.Empty)
			{
				parameters = new List<SqlParameter>
				{
					new SqlParameter("@PardakhtGuid", (object)pardakhtGuid)
				};
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("sp_Acc_AghsatShakhsi_UpdateReceptionInfo", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatShPersistanceRepository.cs"), ReturnedState.None, "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatShPersistanceRepository.cs", 25);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatShPersistanceRepository.cs", 29);
			res.AddReturnedValue(ex, "", "UpdatePardakhtInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Installments\\AghsatShPersistanceRepository.cs", 30);
		}
		return res;
	}
}
