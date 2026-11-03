using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class TafsilSelectedDetListPersistenceRepository : PersistenceMasterRepository<TafsilSelectedDetListBusiness, TafsilSelectedDetails>, ITafsilSelectedDetListRepository, IPersistenceMasterRepository<TafsilSelectedDetListBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TafsilSelectedDetListBusiness, TafsilSelectedDetails>, IItemAsyncMapper<TafsilSelectedDetails, TafsilSelectedDetListBusiness>
	{
		public Task<TafsilSelectedDetails> MapAsync(string contextGuid, TafsilSelectedDetListBusiness source, TafsilSelectedDetails destination, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TafsilSelectedDetails();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.HGuid = source.HGuid;
				destination.HType = source.HType;
				destination.MasterGuid = source.MasterGuid;
				destination.Description = source.Description;
				return Task.FromResult(destination);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedDetListPersistenceRepository.cs", 59);
				throw;
			}
		}

		public async Task<TafsilSelectedDetListBusiness> MapAsync(string contextGuid, TafsilSelectedDetails source, TafsilSelectedDetListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				if (destination == null)
				{
					destination = new TafsilSelectedDetListBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				await destination.SetHGuidAsync(source.HGuid, EnSystem.Mapper, token, tranName);
				destination.HType = source.HType;
				destination.MasterGuid = source.MasterGuid;
				destination.Description = source.Description;
				return destination;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedDetListPersistenceRepository.cs", 86);
				throw;
			}
		}

		public Task<List<TafsilSelectedDetails>> MapListAsync(string contextGuid, IEnumerable<TafsilSelectedDetListBusiness> source, List<TafsilSelectedDetails> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TafsilSelectedDetListBusiness>> MapListAsync(string contextGuid, IEnumerable<TafsilSelectedDetails> source, List<TafsilSelectedDetListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TafsilSelectedDetails, TafsilSelectedDetListBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TafsilSelectedDetListBusiness, TafsilSelectedDetails> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public TafsilSelectedDetListPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedDetListPersistenceRepository.cs", 32);
		}
	}
}
