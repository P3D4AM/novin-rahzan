using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Tolid;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Tolid;

internal class TolidFormulaMavadPersistenceRepository : PersistenceMasterRepository<TolidFormulaMavadBusiness, TolidFormulaMavad>, ITolidFormulaMavadRepository, IPersistenceMasterRepository<TolidFormulaMavadBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TolidFormulaMavad, TolidFormulaMavadBusiness>, IItemAsyncMapper<TolidFormulaMavadBusiness, TolidFormulaMavad>
	{
		public async Task<TolidFormulaMavadBusiness> MapAsync(string contextGuid, TolidFormulaMavad source, TolidFormulaMavadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaMavadPersistenceRepository.cs"))
			{
				_ = 1;
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new TolidFormulaMavadBusiness(contextGuid);
					}
					destination.Description = source.Description;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
					await destination.SetProductGuid(source.ProductGuid, EnSystem.Mapper, tranName);
					await destination.SetDepotGuid(source.DepotGuid, EnSystem.Mapper, tranName);
					destination.Quantity = source.Quantity;
					destination.Serial = source.Serial;
					destination.V2Guid = source.V2Guid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaMavadPersistenceRepository.cs", 62);
					throw;
				}
				return destination;
			}
		}

		public Task<TolidFormulaMavad> MapAsync(string contextGuid, TolidFormulaMavadBusiness source, TolidFormulaMavad destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaMavadPersistenceRepository.cs"))
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
						destination = new TolidFormulaMavad();
					}
					destination.Description = source.Description ?? "";
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
					destination.ProductGuid = source.ProductGuid;
					destination.DepotGuid = source.DepotGuid;
					destination.Quantity = source.Quantity;
					destination.Serial = source.Serial;
					destination.V2Guid = source.V2Guid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaMavadPersistenceRepository.cs", 92);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<TolidFormulaMavadBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaMavad> source, List<TolidFormulaMavadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TolidFormulaMavad>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaMavadBusiness> source, List<TolidFormulaMavad> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TolidFormulaMavad, TolidFormulaMavadBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TolidFormulaMavadBusiness, TolidFormulaMavad> MapToDatabase => Mapper;

	public TolidFormulaMavadPersistenceRepository(ContextHolder dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tolid\\TolidFormulaMavadPersistenceRepository.cs", 33);
		}
	}
}
