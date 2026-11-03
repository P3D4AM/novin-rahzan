using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Filters;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class PrdAnbarGardaniBajePersistenceRepository : PrdAnbarGardaniPersistenceRepositoryBase
{
	public PrdAnbarGardaniBajePersistenceRepository(ContextHolder db)
		: base(db, new PrdAnbarGardaniDetBajePersistenceRepository(db, new PrdSerialPersistanceRepository(db)))
	{
	}

	public override async Task<List<PrdAnbarGardaniDetBusiness>> GetAllAsync(PrdAnbarGardaniBusiness master, AnbarGardaniFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs", 26));
			List<PrdAnbarGardaniDetBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 4;
				try
				{
					List<PrdAnbarGardaniDetBusiness> list = new List<PrdAnbarGardaniDetBusiness>();
					ProductCompactFilter filter2 = new ProductCompactFilter
					{
						LoadJustParents = false
					};
					ProductGroupFilter productGroupFilter = new ProductGroupFilter
					{
						System = EnSystem.UserInterface,
						LoadJustParents = false,
						Type = EnProductType.Product
					};
					IEnumerable<ProductCompactBusiness> prds = await ProductCompactBusiness.GetAllAsync(ContextGuid, EnSystem.UserInterface, filter2, token, tranName);
					IEnumerable<DepotBusiness> depots = await DepotBusiness.GetAllAsync(ContextGuid, EnSystem.UserInterface, true, tranName, token);
					await PrdGroupBusiness.GetAllAsync(ContextGuid, productGroupFilter, token, tranName);
					IEnumerable<PrdAnbarRelationBusiness> anbarRelations = await PrdAnbarRelationBusiness.GetAllAsync(ContextGuid, EnSystem.UserInterface, null, null, token, tranName);
					await prdAnbarGardaniDet.GetAllAsync(master, token, tranName);
					IEnumerable<Guid> guids = master.ListAnbarGardaniDet.Select((PrdAnbarGardaniDetBusiness s) => s.PrdGuid);
					if (guids.Any())
					{
						prds = prds.Where((ProductCompactBusiness w) => !guids.Contains(w.Guid));
					}
					if (filter != null && filter.DepotGuid.HasValue)
					{
						DepotBusiness d2 = depots.FirstOrDefault((DepotBusiness w) => w.Guid == filter.DepotGuid.Value);
						list.AddRange(prds.Select((ProductCompactBusiness p) => new PrdAnbarGardaniDetBusiness(ContextGuid)
						{
							Amount = (anbarRelations.FirstOrDefault((PrdAnbarRelationBusiness w) => w.PrdGuid == p.Guid && w.AnbarGuid == d2.Guid)?.Amount ?? 0m),
							MasterGuid = (master?.Guid ?? Guid.Empty),
							PrdCode = p.IdProduct,
							PrdDepotName = d2.Name,
							DptGuid = d2.Guid,
							PrdGuid = p.Guid,
							PrdVahedName = p.VahedName,
							PrdName = p.Name,
							PrdGroupName = p.GroupName,
							Status = false
						}).ToList());
					}
					else
					{
						list.AddRange(prds.SelectMany((ProductCompactBusiness p) => depots.Select((DepotBusiness d) => new PrdAnbarGardaniDetBusiness(ContextGuid)
						{
							Amount = (anbarRelations.FirstOrDefault((PrdAnbarRelationBusiness w) => w.PrdGuid == p.Guid && w.AnbarGuid == d.Guid)?.Amount ?? 0m),
							MasterGuid = (master?.Guid ?? Guid.Empty),
							PrdCode = p.IdProduct,
							PrdDepotName = d.Name,
							DptGuid = d.Guid,
							PrdGuid = p.Guid,
							PrdVahedName = p.VahedName,
							PrdName = p.Name,
							PrdGroupName = p.GroupName,
							Status = false
						})).ToList());
					}
					if (master.ListAnbarGardaniDet.Any())
					{
						list.InsertRange(0, master.ListAnbarGardaniDet);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs", 83);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<List<PrdAnbarGardaniReportBusiness>> GetAllAsync(AnbarGardaniFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs", 94));
		List<PrdAnbarGardaniReportBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<PrdAnbarGardani> queryable = (IQueryable<PrdAnbarGardani>)c.PrdAnbarGardani;
				if (!string.IsNullOrEmpty(filter.Description))
				{
					foreach (string word in filter.Description?.FixString()?.SplitString())
					{
						if (!string.IsNullOrEmpty(word))
						{
							queryable = queryable?.Where((PrdAnbarGardani p) => p.Description.Contains(word) || p.Number == word.ParseToInt());
						}
					}
				}
				if (filter.DateFrom.HasValue)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => q.DateM >= filter.DateFrom);
				}
				if (filter.DateTo.HasValue)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => q.DateM <= filter.DateTo);
				}
				if (filter.Type.HasValue)
				{
					queryable = queryable?.Where((PrdAnbarGardani q) => (int?)q.Type == (int?)filter.Type);
				}
				IQueryable<PrdAnbarGardaniReportBusiness> queryable2 = (queryable?.OrderBy((PrdAnbarGardani x) => x.DateM))?.Select((PrdAnbarGardani q) => new PrdAnbarGardaniReportBusiness
				{
					DateM = q.DateM,
					Description = q.Description,
					EzafeSum = q.EzafeSum,
					Guid = q.Guid,
					KasrSum = q.KasrSum,
					Number = q.Number,
					SanadNo = q.SanadNo,
					ServerStatus = q.ServerStatus,
					UserName = q.CashierItem.Name,
					StatusId = q.StatusId,
					Type = q.Type
				});
				List<PrdAnbarGardaniReportBusiness> list = ((!token.IsCancellationRequested) ? (await ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<PrdAnbarGardaniReportBusiness>(queryable2, token) : null)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs", 131);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<PrdAnbarGardaniReportBusiness> GetReportAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs", 142));
		PrdAnbarGardaniReportBusiness result;
		try
		{
			_ = 1;
			try
			{
				PrdAnbarGardani prdAnbarGardani = await c.PrdAnbarGardani.FindAsync(new object[1] { guid });
				if (prdAnbarGardani == null)
				{
					result = null;
				}
				else
				{
					PrdAnbarGardaniReportBusiness prdAnbarGardaniReportBusiness = new PrdAnbarGardaniReportBusiness
					{
						DateM = prdAnbarGardani.DateM,
						Description = prdAnbarGardani.Description,
						EzafeSum = prdAnbarGardani.EzafeSum,
						Guid = prdAnbarGardani.Guid,
						KasrSum = prdAnbarGardani.KasrSum,
						Number = prdAnbarGardani.Number,
						SanadNo = prdAnbarGardani.SanadNo,
						ServerStatus = prdAnbarGardani.ServerStatus,
						UserName = (prdAnbarGardani.CashierItem?.Name ?? ""),
						StatusId = prdAnbarGardani.StatusId,
						Type = prdAnbarGardani.Type
					};
					result = prdAnbarGardaniReportBusiness;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs", 167);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniBajePersistenceRepository.cs");
			}
		}
		return result;
	}
}
