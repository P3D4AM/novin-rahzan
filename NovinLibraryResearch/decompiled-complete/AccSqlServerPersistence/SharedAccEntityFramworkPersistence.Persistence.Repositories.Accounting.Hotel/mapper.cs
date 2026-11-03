using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal class mapper : IItemAsyncMapper<FacilitiesBusiness, Facilities>, IItemAsyncMapper<Facilities, FacilitiesBusiness>
{
	public Task<Facilities> MapAsync(string contextGuid, FacilitiesBusiness source, Facilities destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new Facilities();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.Price = source.Price;
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\FacilitiesPersistenceRepository.cs", 64);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<FacilitiesBusiness> MapAsync(string contextGuid, Facilities source, FacilitiesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new FacilitiesBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.Price = source.Price;
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\FacilitiesPersistenceRepository.cs", 90);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<Facilities>> MapListAsync(string contextGuid, IEnumerable<FacilitiesBusiness> source, List<Facilities> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<FacilitiesBusiness>> MapListAsync(string contextGuid, IEnumerable<Facilities> source, List<FacilitiesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
