using AccSqlServerPersistence.Persistence.Repositories.Accounting.TafsilPersistanceRepo;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class TafsilPersistenceRepository : TafsilPersistenceRepositoryBase
{
	public TafsilPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}
}
