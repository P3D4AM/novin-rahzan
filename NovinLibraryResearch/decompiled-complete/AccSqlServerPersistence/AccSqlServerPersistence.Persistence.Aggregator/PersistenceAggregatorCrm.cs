using System;
using System.Collections.Generic;
using System.Linq;
using AccSqlServerPersistence.Persistence.Repositories.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Tickets;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Aggregator;

public class PersistenceAggregatorCrm : PersistenceAggregatorBajeVersion6
{
	public override ITicketRepository Ticket { get; set; }

	public override ITicketDetRepository TicketDet { get; set; }

	public PersistenceAggregatorCrm(string connectionString, WebContextHolder context)
		: base(connectionString, context)
	{
	}

	public override void SetAggregator(List<IDatabaseManager> DbContexts)
	{
		try
		{
			ContextHolder dbContext = (ContextHolder)DbContexts.SingleOrDefault((IDatabaseManager p) => p.Config == EnCacheConfig.Global);
			base.SetAggregator(DbContexts);
			Ticket = new ExternalTicketPersistenceRepository(dbContext);
			TicketDet = new ExternalTicketDetailPersistenceRepository(dbContext);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetAggregator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Aggregator\\PersistenceAggregatorCrm.cs", 31);
			throw;
		}
	}
}
