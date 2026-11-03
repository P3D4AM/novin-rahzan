using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Indexes;
using PacketParser;
using PacketParser.BusinessLayer;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class FullTextWordPersistenceRepository : FullTextWordPersistenceRepositoryBase
{
	public FullTextWordPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<FullTextWordBusiness> lst, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("توقع نداریم که فراخوانی بشه!".GetTranslate());
	}
}
