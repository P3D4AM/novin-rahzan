using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Selection;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class PrdAnbarGardaniDetBajePersistenceRepository : PrdAnbarGardaniDetBasePersistenceRepository
{
	public PrdAnbarGardaniDetBajePersistenceRepository(ContextHolder db, PrdSerialPersistanceRepositoryBase serialRepo)
		: base(db, serialRepo)
	{
	}

	public override async Task GetAllAsync(PrdAnbarGardaniBusiness master, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBajePersistenceRepository.cs", 19));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBajePersistenceRepository.cs"))
		{
			try
			{
				_ = 1;
				try
				{
					IIncludableQueryable<PrdAnbarGardaniDet, ProductVahed> val = EntityFrameworkQueryableExtensions.Include<PrdAnbarGardaniDet, ProductVahed>((IQueryable<PrdAnbarGardaniDet>)EntityFrameworkQueryableExtensions.Include<PrdAnbarGardaniDet, Depot>((IQueryable<PrdAnbarGardaniDet>)EntityFrameworkQueryableExtensions.ThenInclude<PrdAnbarGardaniDet, ProductGroupRelation, ProductGroup>((IIncludableQueryable<PrdAnbarGardaniDet, IEnumerable<ProductGroupRelation>>)(object)EntityFrameworkQueryableExtensions.ThenInclude<PrdAnbarGardaniDet, Product, ICollection<ProductGroupRelation>>(EntityFrameworkQueryableExtensions.Include<PrdAnbarGardaniDet, Product>(((IQueryable<PrdAnbarGardaniDet>)c.PrdAnbarGardaniDet).Where((PrdAnbarGardaniDet x) => x.MasterGuid == master.Guid), (Expression<Func<PrdAnbarGardaniDet, Product>>)((PrdAnbarGardaniDet q) => q.Product)), (Expression<Func<Product, ICollection<ProductGroupRelation>>>)((Product q) => q.ProductGroupRelations)), (Expression<Func<ProductGroupRelation, ProductGroup>>)((ProductGroupRelation q) => q.Group)), (Expression<Func<PrdAnbarGardaniDet, Depot>>)((PrdAnbarGardaniDet q) => q.Depot)), (Expression<Func<PrdAnbarGardaniDet, ProductVahed>>)((PrdAnbarGardaniDet q) => q.Product.VahedItem));
					if (val == null || !((IQueryable<PrdAnbarGardaniDet>)val).Any())
					{
						goto end_IL_00fb;
					}
					foreach (PrdAnbarGardaniDet item2 in (IEnumerable<PrdAnbarGardaniDet>)val)
					{
						if (item2 != null)
						{
							PrdAnbarGardaniDetBusiness item = await MapFromDatabase.MapAsync(base.ContextGuid, item2, PrdAnbarGardaniDetBusiness.GetFactory(base.ContextGuid, master), tranName, token);
							master.AddToDetList(item);
						}
					}
					goto end_IL_00ef;
					end_IL_00fb:;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBajePersistenceRepository.cs", 42);
					throw;
				}
				return;
				end_IL_00ef:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBajePersistenceRepository.cs");
				}
			}
		}
	}
}
