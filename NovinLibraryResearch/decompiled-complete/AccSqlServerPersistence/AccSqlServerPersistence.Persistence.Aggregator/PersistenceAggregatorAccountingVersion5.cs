using System;
using System.Collections.Generic;
using System.Linq;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Check;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Order;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Pardakht;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Reception;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Sanad;
using AccSqlServerPersistence.Persistence.Repositories.Version5Repositories.Tolid;
using Core.Repository;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Details;
using EntityCache.Core.Repository.Accounting.Tolid;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Aggregator;

public class PersistenceAggregatorAccountingVersion5 : BaseAggregator
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

	public PersistenceAggregatorAccountingVersion5(string connectionString, WebContextHolder context)
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
			ISanad = new SanadV5PersistenceRepository(contextHolder);
			ISanadDet = new SanadDetailV5PersistenceRepository(contextHolder);
			IOrder = new OrderVersion5PersistenceRepository(contextHolder);
			IOrderDetail = new OrderDetailVersion5PersistenceRepository(contextHolder);
			IABAD = new OrderAbadDetVersion5PersistenceRepository(contextHolder);
			IOrderAbad = new OrderAbadVersion5PersistenceRepository(contextHolder);
			IPardakht = new PardakhtVersion5PersistenceRepository(contextHolder);
			IPardakhtCheckM = new PardakhtCheckMVersion5PersistenceRepository(contextHolder);
			IPardakhtCheckSh = new PardakhtCheckShVersion5PersistenceRepository(contextHolder);
			IReception = new ReceptionVersion5PersistenceRepository(contextHolder);
			IReceptionCheckRepository = new ReceptionCheckVersion5PersistenceRepository(contextHolder);
			IReceptionHavale = new ReceptionHavaleVersion5PersistenceRepository(contextHolder);
			IViewCheckMRepository = new ViewCheckMVersion5PersistenceRepository(contextHolder);
			IViewCheckShRepository = new ViewCheckShVersion5PersistenceRepository(contextHolder);
			Tolid = new TolidVersion5PersistenceRepository(contextHolder);
			TolidFormula = new TolidFormulaVersion5PersistenceRepository(contextHolder);
			base.IPrdAnbarGardani = new PrdAnbarGardaniAccountingPersistenceRepository(contextHolder);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetAggregator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Aggregator\\PersistenceAggregatorAccountingVersion5.cs", 70);
			throw;
		}
	}
}
