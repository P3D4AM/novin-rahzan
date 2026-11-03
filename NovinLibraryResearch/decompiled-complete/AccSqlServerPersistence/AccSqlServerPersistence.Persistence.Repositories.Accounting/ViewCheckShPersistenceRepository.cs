using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer.Models;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ViewCheckShPersistenceRepository : ViewCheckShBasePersistenceRepository
{
	public ViewCheckShPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override Task<IEnumerable<ChequeSHSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
