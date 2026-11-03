using System;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Bill;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal class FacilitiesPersistenceRepository : PersistenceRepository<FacilitiesBusiness, Facilities>, IFacilitiesRepository, IRepository<FacilitiesBusiness>, INoGuidRepository<FacilitiesBusiness>, IDisposable
{
	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Facilities, FacilitiesBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Facilities, FacilitiesBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<FacilitiesBusiness, Facilities> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public FacilitiesPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\FacilitiesPersistenceRepository.cs", 39);
		}
	}
}
