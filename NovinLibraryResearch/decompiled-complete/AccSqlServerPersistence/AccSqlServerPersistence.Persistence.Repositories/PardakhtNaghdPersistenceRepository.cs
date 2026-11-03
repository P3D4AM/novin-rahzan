using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Details;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class PardakhtNaghdPersistenceRepository : PersistenceMasterRepository<PardakhtNaghdBusiness, PardakhtNaghd>, IPardakhtNaghdRepository, IPersistenceMasterRepository<PardakhtNaghdBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PardakhtNaghdBusiness, PardakhtNaghd>, IItemAsyncMapper<PardakhtNaghd, PardakhtNaghdBusiness>
	{
		public async Task<PardakhtNaghd> MapAsync(string contextGuid, PardakhtNaghdBusiness source, PardakhtNaghd destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdPersistenceRepository.cs"))
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
						destination = new PardakhtNaghd();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Description = source.RowDesc ?? "";
					destination.Price = source.Price;
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.MasterGuid = source.MasterGuid;
					PardakhtNaghd pardakhtNaghd = destination;
					pardakhtNaghd.ShortDescription = await source.GetShortDescriptionAsync(EnSystem.Mapper, token, tranName);
					destination.Ordinal = source.Ordinal;
					destination.DateM = source.DateM;
					destination.DateUtc = destination.DateM.ToUniversalTime();
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdPersistenceRepository.cs", 63);
					throw;
				}
				return destination;
			}
		}

		public Task<PardakhtNaghdBusiness> MapAsync(string contextGuid, PardakhtNaghd source, PardakhtNaghdBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdPersistenceRepository.cs"))
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
						destination = new PardakhtNaghdBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.RowDesc = source.Description;
					destination.SetPrice(EnSystem.Mapper, source.Price);
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.MasterGuid = source.MasterGuid;
					destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
					destination.DateM = source.DateM;
					destination.DateUtc = source.DateUtc;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdPersistenceRepository.cs", 96);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<PardakhtNaghd>> MapListAsync(string contextGuid, IEnumerable<PardakhtNaghdBusiness> source, List<PardakhtNaghd> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PardakhtNaghdBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtNaghd> source, List<PardakhtNaghdBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<PardakhtNaghd, PardakhtNaghdBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PardakhtNaghdBusiness, PardakhtNaghd> MapToDatabase => Mapper;

	public PardakhtNaghdPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdPersistenceRepository.cs", 32);
		}
	}
}
