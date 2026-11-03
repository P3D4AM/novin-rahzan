using System;
using System.Collections.Generic;
using System.Linq;
using AccSqlServerPersistence.Persistence.Repositories;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Details;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Khazane;
using AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;
using AccSqlServerPersistence.Persistence.Repository;
using Core.Repository;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Details;
using EntityCache.Core.Repository.Accounting.Tolid;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Aggregator;

public class PersistenceAggregatorBajeVersion6 : BaseAggregator
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

	public PersistenceAggregatorBajeVersion6(string connectionString, WebContextHolder context)
		: base(connectionString, context)
	{
	}

	public override void SetAggregator(List<IDatabaseManager> DbContexts)
	{
		try
		{
			ContextHolder contextHolder = (ContextHolder)DbContexts.SingleOrDefault((IDatabaseManager p) => p.Config == EnCacheConfig.Accounting);
			base.SetAggregator(DbContexts);
			IGardeshTafil = new GardeshTafsil_TablePersistanceRepository(contextHolder);
			ISanad = new SanadPersistanceRepository(contextHolder);
			ISanadDet = new SanadDetPersistenceRepository(contextHolder);
			IOrder = new OrderPersistenceRepository(contextHolder, base.TableRelationShip);
			IOrderDetail = new OrderDetPersistenceRepository(contextHolder);
			IABAD = new AbadDetPersistanceRepository(contextHolder);
			IOrderAbad = new OrderAbadPersistanceRepository(contextHolder);
			IPardakht = new PardakhtPersistenceRepository(contextHolder);
			IPardakhtCheckM = new PardakhtCheckMPersistanceRepository(contextHolder);
			IPardakhtCheckSh = new PardakhtCheckShBajePersistanceRepository(contextHolder);
			IReceptionCheckRepository = new ReceptionCheckBajePersistenceRepository(contextHolder);
			IReception = new ReceptionPersistenceRepository(contextHolder, IReceptionCheckRepository, base.TableRelationShip);
			IReceptionHavale = new ReceptionHavalePersistenceRepository(contextHolder);
			IViewCheckMRepository = new ViewCheckMPersistenceRepository(contextHolder);
			IViewCheckShRepository = new ViewCheckShPersistenceRepository(contextHolder);
			Tolid = new TolidPersistenceRepository(contextHolder);
			TolidFormula = new TolidFormulaPersistenceRepository(contextHolder);
			base.IPrdAnbarGardani = new PrdAnbarGardaniBajePersistenceRepository(contextHolder);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetAggregator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Aggregator\\PersistenceAggregatorBajeVersion6.cs", 67);
			throw;
		}
	}
}
