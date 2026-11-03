using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Khazane;

internal class PardakhtPersistenceRepository : PardakhtPersistenceRepositoryBase
{
	public PardakhtPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs", 24);
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@TafsilGuid", (object)tafsilGuid),
				new SqlParameter("@Name", (object)name)
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("sp_Acc_Pardakht_UpdateCustomerFullName", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs"), ReturnedState.None, "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs", 30);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs", 34);
			res.AddReturnedValue(ex, "", "UpdateCustomerFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs", 35);
		}
		return res;
	}

	public override async Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs", 45);
			List<SqlParameter> parameters = null;
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				parameters = new List<SqlParameter>(1)
				{
					new SqlParameter("@Guid", (object)guid)
				};
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("Acc_Pardakht_UpdateDateMoaser", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs"), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs", 48);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs", 52);
			res.AddReturnedValue(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\PardakhtPersistenceRepository.cs", 53);
		}
		return res;
	}
}
