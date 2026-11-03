using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class LotteryPersistenceRepository : LotteryPersistenceRepositoryBase
{
	public LotteryPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<LotteryBusiness>> GetAllAsync(LotteryStatus status, DateTime date1, DateTime date2, Guid cusGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryPersistenceRepository.cs", 19));
			IEnumerable<LotteryBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<SqlParameter> list = new List<SqlParameter>
					{
						new SqlParameter("@Status", (object)(short)status),
						new SqlParameter("@Date1", (object)date1),
						new SqlParameter("@Date2", (object)date2),
						new SqlParameter("@CusGuid", (object)cusGuid)
					};
					List<Lottery> source = new List<Lottery>(RelationalQueryableExtensions.FromSqlRaw<Lottery>(c.Lottery, "sp_Acc_Lottery_SelectAll @Status, @Date1, @Date2, @CusGuid", new object[1] { list }));
					result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, "", token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryPersistenceRepository.cs", 37);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
