using System;
using AccSqlServerPersistence.Entities.Indexes;
using EntityCache.Business;
using EntityCache.Core.Repository.Indexes;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Indexes;

public class IxProductDescFieldPersistenceRepository : IxPersistenceRepository<IxProductDescField>, IIxProductDescFieldRepository, IIxRepository, IDisposable
{
	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	protected override string TableName => "Acc_IxProductDescField";

	protected override IItemAsyncMapper<IndexEntityBusiness, IxProductDescField> MapperToDatabase => Mapper;

	public IxProductDescFieldPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxProductDescFieldPersistenceRepository.cs", 31);
		}
	}
}
