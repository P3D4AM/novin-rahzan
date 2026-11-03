using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Selection;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

public abstract class PrdAnbarGardaniDetBasePersistenceRepository : PersistenceMasterRepository<PrdAnbarGardaniDetBusiness, PrdAnbarGardaniDet>, IPrdAnbargardaniDetRepository, IPersistenceMasterRepository<PrdAnbarGardaniDetBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdAnbarGardaniDetBusiness, PrdAnbarGardaniDet>, IItemAsyncMapper<PrdAnbarGardaniDet, PrdAnbarGardaniDetBusiness>
	{
		public Task<PrdAnbarGardaniDet> MapAsync(string contextGuid, PrdAnbarGardaniDetBusiness source, PrdAnbarGardaniDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBasePersistenceRepository.cs"))
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
						destination = new PrdAnbarGardaniDet();
					}
					destination.Guid = source.Guid;
					destination.MasterGuid = source.MasterGuid;
					destination.PrdGuid = source.PrdGuid;
					destination.DptGuid = source.DptGuid;
					destination.QtKasr = source.QtKasr;
					destination.QtEzafe = source.QtEzafe;
					destination.UnitPrice = source.UnitPrice;
					destination.Modified = source.Modified;
					destination.KasrSum = source.KasrSum;
					destination.EzafeSum = source.EzafeSum;
					destination.QtRealAmount = source.QtRealAmount;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBasePersistenceRepository.cs", 95);
					throw;
				}
			}
		}

		public Task<PrdAnbarGardaniDetBusiness> MapAsync(string contextGuid, PrdAnbarGardaniDet source, PrdAnbarGardaniDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBasePersistenceRepository.cs"))
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
						destination = new PrdAnbarGardaniDetBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.MasterGuid = source.MasterGuid;
					destination.PrdGuid = source.PrdGuid;
					destination.DptGuid = source.DptGuid;
					destination.QtRealAmount = source.QtRealAmount;
					destination.QtKasr = source.QtKasr;
					destination.QtEzafe = source.QtEzafe;
					destination.UnitPrice = source.UnitPrice;
					destination.Modified = source.Modified;
					destination.PrdCode = source.Product?.IdProduct ?? "";
					destination.PrdName = source.Product?.Name ?? "";
					destination.PrdVahedName = source.Product?.VahedItem?.Name ?? "";
					destination.PrdGroupName = source.Product?.ProductGroupRelations?.FirstOrDefault((ProductGroupRelation q) => q.Ordinal == 1)?.Group?.Name ?? "";
					destination.PrdDepotName = source.Depot?.Name ?? "";
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBasePersistenceRepository.cs", 133);
					throw;
				}
			}
		}

		public Task<List<PrdAnbarGardaniDet>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarGardaniDetBusiness> source, List<PrdAnbarGardaniDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdAnbarGardaniDetBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarGardaniDet> source, List<PrdAnbarGardaniDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	protected PrdSerialPersistanceRepositoryBase _serialRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<PrdAnbarGardaniDet, PrdAnbarGardaniDetBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdAnbarGardaniDetBusiness, PrdAnbarGardaniDet> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public PrdAnbarGardaniDetBasePersistenceRepository(ContextHolder db, PrdSerialPersistanceRepositoryBase serialRepo)
		: base(db)
	{
		_serialRepo = serialRepo;
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_serialRepo?.Dispose();
				_serialRepo = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBasePersistenceRepository.cs", 39);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PrdAnbarGardaniDetBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBasePersistenceRepository.cs", 47);
			foreach (PrdAnbarGardaniDetBusiness item in items)
			{
				PrdAnbarGardaniDet prdAnbarGardaniDet = await MapToDatabase.MapAsync(base.ContextGuid, item, new PrdAnbarGardaniDet(), tranName, token);
				prdAnbarGardaniDet.MasterGuid = masterGuid;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(prdAnbarGardaniDet, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBasePersistenceRepository.cs", 52);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBasePersistenceRepository.cs", 57);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\PrdAnbarGardaniDetBasePersistenceRepository.cs", 58);
		}
		return res;
	}

	public override Task<List<PrdAnbarGardaniDetBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public abstract Task GetAllAsync(PrdAnbarGardaniBusiness master, CancellationToken token, string tranName);
}
