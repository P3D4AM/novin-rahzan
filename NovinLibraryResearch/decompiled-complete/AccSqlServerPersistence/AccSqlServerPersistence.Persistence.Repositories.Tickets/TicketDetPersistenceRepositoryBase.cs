using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Tickets;
using EntityCache.Business.Global.Tickets;
using EntityCache.Business.Tickets;
using EntityCache.Core;
using EntityCache.Core.Repository.Tickets;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Tickets;

public abstract class TicketDetPersistenceRepositoryBase : PersistenceRepository<TicketDetBusiness, TicketDet>, ITicketDetRepository, IRepository<TicketDetBusiness>, INoGuidRepository<TicketDetBusiness>, IDisposable
{
	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => false;

	public override bool HasModified => false;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<TicketDet, TicketDetBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TicketDetBusiness, TicketDet> MapToDatabase => Mapper;

	public override IItemMapper<TicketDet, TicketDetBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public TicketDetPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tickets\\TicketDetPersistenceRepositoryBase .cs", 39);
		}
	}

	public virtual Task<IEnumerable<TicketReportBusiness>> GetAllAsync(TicketDetFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
