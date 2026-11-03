using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class PrdAnbarGardaniDetAccountingPersistenceRepository : PrdAnbarGardaniDetBasePersistenceRepository
{
	public PrdAnbarGardaniDetAccountingPersistenceRepository(ContextHolder db, PrdSerialPersistanceRepositoryBase serialRepo)
		: base(db, serialRepo)
	{
	}

	public override async Task GetAllAsync(PrdAnbarGardaniBusiness master, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetAccountingPersistenceRepository.cs", 18));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetAccountingPersistenceRepository.cs"))
		{
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<PrdAnbarGardaniDet> queryable = ((IQueryable<PrdAnbarGardaniDet>)c.PrdAnbarGardaniDet).Where((PrdAnbarGardaniDet x) => x.MasterGuid == master.Guid);
					if (queryable == null || !queryable.Any())
					{
						goto end_IL_0101;
					}
					foreach (PrdAnbarGardaniDet item in queryable)
					{
						PrdAnbarGardaniDetBusiness anbarGardaniDetMapped = await MapFromDatabase.MapAsync(base.ContextGuid, item, PrdAnbarGardaniDetBusiness.GetFactory(base.ContextGuid, master), tranName, token);
						if (anbarGardaniDetMapped == null)
						{
							continue;
						}
						if (anbarGardaniDetMapped.Amount <= 0m && anbarGardaniDetMapped.UnitPrice <= 0m)
						{
							KardexBusiness kardexBusiness = await KardexBusiness.GetLastRecordsByDateAsync(base.ContextGuid, master.DateM, anbarGardaniDetMapped.PrdGuid, anbarGardaniDetMapped.DptGuid, token, tranName);
							if (kardexBusiness != null)
							{
								anbarGardaniDetMapped.Amount = Math.Round(kardexBusiness.RemQT.GetValueOrDefault(), 3);
								anbarGardaniDetMapped.UnitPrice = kardexBusiness.RemUnitPrice.GetValueOrDefault();
							}
						}
						if (!anbarGardaniDetMapped.QtRealAmount.HasValue)
						{
							if (anbarGardaniDetMapped.QtKasr > 0m)
							{
								anbarGardaniDetMapped.QtRealAmount = Math.Round(anbarGardaniDetMapped.Amount - anbarGardaniDetMapped.QtKasr, 3);
							}
							if (anbarGardaniDetMapped.QtEzafe > 0m)
							{
								anbarGardaniDetMapped.QtRealAmount = Math.Round(anbarGardaniDetMapped.Amount + anbarGardaniDetMapped.QtEzafe, 3);
							}
						}
						master.AddToDetList(anbarGardaniDetMapped);
					}
					goto end_IL_00f1;
					end_IL_0101:;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetAccountingPersistenceRepository.cs", 65);
					throw;
				}
				return;
				end_IL_00f1:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetAccountingPersistenceRepository.cs");
				}
			}
		}
	}
}
