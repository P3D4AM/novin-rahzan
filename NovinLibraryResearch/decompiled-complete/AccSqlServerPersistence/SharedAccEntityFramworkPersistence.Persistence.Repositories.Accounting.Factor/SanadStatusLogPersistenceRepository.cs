using System;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Factor;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Factor;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Factor;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Factor;

public class SanadStatusLogPersistenceRepository : PersistenceRepository<SanadStatusLogBusiness, SanadStatusLog>, ISanadStatusLogRepository, IRepository<SanadStatusLogBusiness>, INoGuidRepository<SanadStatusLogBusiness>, IDisposable
{
	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<SanadStatusLog, SanadStatusLogBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<SanadStatusLog, SanadStatusLogBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SanadStatusLogBusiness, SanadStatusLog> MapToDatabase => Mapper;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public SanadStatusLogPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusLogPersistenceRepository.cs", 41);
		}
	}
}
