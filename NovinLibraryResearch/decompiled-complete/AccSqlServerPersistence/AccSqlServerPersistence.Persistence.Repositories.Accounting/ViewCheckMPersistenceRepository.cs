using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer.Models;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ViewCheckMPersistenceRepository : ViewCheckMBasePersistenceRepository
{
	public ViewCheckMPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override Task<IEnumerable<ChequeMSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
