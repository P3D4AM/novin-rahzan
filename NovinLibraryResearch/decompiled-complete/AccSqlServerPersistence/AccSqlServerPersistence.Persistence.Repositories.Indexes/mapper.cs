using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Indexes;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Indexes;

internal class mapper : IItemAsyncMapper<IndexEntityBusiness, IxProductDescField>
{
	public Task<IxProductDescField> MapAsync(string contextGuid, IndexEntityBusiness source, IxProductDescField destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxProductDescFieldPersistenceRepository.cs"))
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
					destination = new IxProductDescField();
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Indexes\\IxProductDescFieldPersistenceRepository.cs", 55);
				throw;
			}
			return Task.FromResult(destination);
		}
	}

	public Task<List<IxProductDescField>> MapListAsync(string contextGuid, IEnumerable<IndexEntityBusiness> source, List<IxProductDescField> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
