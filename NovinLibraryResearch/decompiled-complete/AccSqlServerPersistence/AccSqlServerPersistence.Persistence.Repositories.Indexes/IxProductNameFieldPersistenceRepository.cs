using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Indexes;
using EntityCache.Business;
using EntityCache.Core.Repository.Indexes;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Indexes;

public class IxProductNameFieldPersistenceRepository : IxPersistenceRepository<IxProductNameField>, IIxProductNameFieldRepository, IIxRepository, IDisposable
{
	private class mapper : IItemAsyncMapper<IndexEntityBusiness, IxProductNameField>
	{
		public Task<IxProductNameField> MapAsync(string contextGuid, IndexEntityBusiness source, IxProductNameField destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxProductNameFieldPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return Task.FromResult(destination);
					}
					if (destination == null)
					{
						destination = new IxProductNameField();
					}
					destination.EntityGuid = source.EntityGuid;
					destination.WordId = source.WordId;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxProductNameFieldPersistenceRepository.cs", 125);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<IxProductNameField>> MapListAsync(string contextGuid, IEnumerable<IndexEntityBusiness> source, List<IxProductNameField> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	protected override string TableName => "Acc_IxProductNameField";

	protected override IItemAsyncMapper<IndexEntityBusiness, IxProductNameField> MapperToDatabase => Mapper;

	public IxProductNameFieldPersistenceRepository(ContextHolder dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxProductNameFieldPersistenceRepository.cs", 102);
		}
	}
}
