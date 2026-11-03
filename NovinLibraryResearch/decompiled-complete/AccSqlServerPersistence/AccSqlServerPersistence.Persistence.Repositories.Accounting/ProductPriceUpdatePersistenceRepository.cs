using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ProductPriceUpdatePersistenceRepository : ProductPriceUpdatePersistenceRepositoryBase
{
	public ProductPriceUpdatePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> InsertBatchAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseModelContext obj = await dbContext.GetContextAsync(tranName, 60000, "InsertBatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepository.cs", 23);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await obj.ExecuteNoneQueryAsync("sp_ProductPriceUpdate_InsertBatch", null, tranName, token, "", CommandType.StoredProcedure, 30, "InsertBatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepository.cs"), ReturnedState.None, "InsertBatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepository.cs", 24);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InsertBatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepository.cs", 28);
			res.AddReturnedValue(ex, "", "InsertBatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepository.cs", 29);
		}
		return res;
	}
}
