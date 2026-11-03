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

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Details;

internal class ReceptionNaghdPersistenceRepository : PersistenceMasterRepository<ReceptionNaghdBusiness, ReceptionNaghd>, IReceptionNaghdRepository, IPersistenceMasterRepository<ReceptionNaghdBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ReceptionNaghdBusiness, ReceptionNaghd>, IItemAsyncMapper<ReceptionNaghd, ReceptionNaghdBusiness>
	{
		public async Task<ReceptionNaghd> MapAsync(string contextGuid, ReceptionNaghdBusiness source, ReceptionNaghd destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceptionNaghd();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DateM = source.DateM;
				destination.Description = source.Description ?? "";
				destination.Price = source.Price;
				destination.TafsilGuid = source.TafsilGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.DecimalGuid = source.DecimalGuid;
				destination.MasterGuid = source.MasterGuid;
				ReceptionNaghd receptionNaghd = destination;
				receptionNaghd.ShortDescription = await source.GetShortDescriptionAsync(EnSystem.Mapper, token, tranName);
				destination.Ordinal = source.Ordinal;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionNaghdPersistenceRepository.cs", 63);
				throw;
			}
			return destination;
		}

		public Task<ReceptionNaghdBusiness> MapAsync(string contextGuid, ReceptionNaghd source, ReceptionNaghdBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceptionNaghdBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DateM = source.DateM;
				destination.Description = source.Description;
				destination.SetPrice(EnSystem.Mapper, source.Price);
				destination.TafsilGuid = source.TafsilGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.DecimalGuid = source.DecimalGuid;
				destination.MasterGuid = source.MasterGuid;
				destination.DateUtc = source.DateUtc;
				destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionNaghdPersistenceRepository.cs", 96);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ReceptionNaghd>> MapListAsync(string contextGuid, IEnumerable<ReceptionNaghdBusiness> source, List<ReceptionNaghd> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReceptionNaghdBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionNaghd> source, List<ReceptionNaghdBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ReceptionNaghd, ReceptionNaghdBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReceptionNaghdBusiness, ReceptionNaghd> MapToDatabase => Mapper;

	public ReceptionNaghdPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionNaghdPersistenceRepository.cs", 28);
		}
	}
}
