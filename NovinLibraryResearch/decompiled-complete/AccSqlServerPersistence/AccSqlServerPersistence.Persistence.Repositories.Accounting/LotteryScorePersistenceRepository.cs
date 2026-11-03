using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class LotteryScorePersistenceRepository : LotteryScorePersistenceRepositoryBase
{
	public LotteryScorePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<List<Guid>> StartLottery(int count, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("StartLottery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "StartLottery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepository.cs", 16));
			List<Guid> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					List<Guid> resultGuids = new List<Guid>();
					List<SqlParameter> parameters = new List<SqlParameter>
					{
						new SqlParameter("Count", (object)count)
					};
					using DbDataReader dr = await c.ExecuteReaderAsync("sp_Acc_Lottery_StartLottery", parameters, tranName, token, CommandType.StoredProcedure, 30, "StartLottery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepository.cs");
					while (await dr.ReadAsync())
					{
						resultGuids.Add((Guid)dr[0]);
					}
					dr.Close();
					foreach (Guid item in resultGuids)
					{
						parameters = new List<SqlParameter>
						{
							new SqlParameter("OrderGuid", (object)item)
						};
						await c.ExecuteNoneQueryAsync("sp_Acc_Lottery_InsertTOLottery", parameters, tranName, token, "", CommandType.StoredProcedure, 30, "StartLottery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepository.cs");
					}
					result = resultGuids;
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
					WebErrorLog.Instance.StartLog(ex2, "", "StartLottery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepository.cs", 38);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("StartLottery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\LotteryScorePersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
