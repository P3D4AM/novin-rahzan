using System;
using System.Collections.Generic;
using System.Linq;
using AccSqlServerPersistence.Persistence.Repositories;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Details;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Khazane;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Check;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Order;
using AccSqlServerPersistence.Persistence.Repository;
using Core.Repository;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Details;
using EntityCache.Core.Repository.Accounting.Tolid;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Aggregator;

public class PersistenceAggregatorAccountingVersion6 : BaseAggregator
{
	public override IGardeshTafsilRepository IGardeshTafil { get; set; }

	public override ISanadRepository ISanad { get; set; }

	public override ISanad_DetRepository ISanadDet { get; set; }

	public override IOrderAbadRepository IOrderAbad { get; set; }

	public override IOrderRepository IOrder { get; set; }

	public override IOrderDetailRepository IOrderDetail { get; set; }

	public override IAbadDetRepository IABAD { get; set; }

	public override IPardakhtRepository IPardakht { get; set; }

	public override IPardakhtCheckShRepository IPardakhtCheckSh { get; set; }

	public override IPardakhtCheckMRepository IPardakhtCheckM { get; set; }

	public override IReceptionRepository IReception { get; set; }

	public override IReceptionCheckRepository IReceptionCheckRepository { get; set; }

	public override IReceptionHavaleRepository IReceptionHavale { get; set; }

	public override IViewCheckMRepository IViewCheckMRepository { get; set; }

	public override IViewCheckShRepository IViewCheckShRepository { get; set; }

	public override ITolidFormulaRepository TolidFormula { get; set; }

	public override ITolidRepository Tolid { get; set; }

	public PersistenceAggregatorAccountingVersion6(string connectionString, WebContextHolder context)
		: base(connectionString, context)
	{
	}

	public override void SetAggregator(List<IDatabaseManager> DbContexts)
	{
		if (string.IsNullOrEmpty(base.ConnectionString))
		{
			throw new NullReferenceException("SqlConnectionSting is null or empty!!");
		}
		try
		{
			ContextHolder contextHolder = (ContextHolder)DbContexts.SingleOrDefault((IDatabaseManager p) => p.Config == EnCacheConfig.Accounting);
			base.SetAggregator(DbContexts);
			IGardeshTafil = new GardeshTafsil_ViewPersistenceRepository(contextHolder);
			ISanad = new SanadPersistanceRepository(contextHolder);
			ISanadDet = new SanadDetPersistenceRepository(contextHolder);
			IOrder = new OrderPersistenceRepository(contextHolder, base.TableRelationShip);
			IOrderDetail = new OrderDetPersistenceRepository(contextHolder);
			IABAD = new OrderAbadDetVersion5PersistenceRepository(contextHolder);
			IOrderAbad = new OrderAbadVersion5PersistenceRepository(contextHolder);
			IPardakht = new PardakhtPersistenceRepository(contextHolder);
			IPardakhtCheckM = new PardakhtCheckMPersistanceRepository(contextHolder);
			IPardakhtCheckSh = new PardakhtCheckShAccountingPersistanceRepository(contextHolder);
			IReceptionCheckRepository = new ReceptionCheckAccountingPersistenceRepository(contextHolder);
			IReception = new ReceptionPersistenceRepository(contextHolder, IReceptionCheckRepository, base.TableRelationShip);
			IReceptionHavale = new ReceptionHavalePersistenceRepository(contextHolder);
			IViewCheckMRepository = new ViewCheckMVersion5PersistenceRepository(contextHolder);
			IViewCheckShRepository = new ViewCheckShVersion5PersistenceRepository(contextHolder);
			Tolid = new TolidPersistenceRepository(contextHolder);
			TolidFormula = new TolidFormulaPersistenceRepository(contextHolder);
			base.IPrdAnbarGardani = new PrdAnbarGardaniAccountingPersistenceRepository(contextHolder);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetAggregator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Aggregator\\PersistenceAggregatorAccountingVersion6.cs", 72);
			throw;
		}
	}
}
