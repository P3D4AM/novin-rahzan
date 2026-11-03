using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repository;

internal class RasGiriPersistanceRepository : PersistenceRepository<RasGiriBusiness, RasGiri>, IRasGiriRepository, IRepository<RasGiriBusiness>, INoGuidRepository<RasGiriBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<RasGiriBusiness, RasGiri>, IItemAsyncMapper<RasGiri, RasGiriBusiness>
	{
		public Task<RasGiri> MapAsync(string contextGuid, RasGiriBusiness source, RasGiri destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs"))
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
						destination = new RasGiri();
					}
					destination.Guid = source.Guid;
					destination.Title = source.Title;
					destination.Description = source.Description;
					destination.DateM = source.DateM;
					destination.BaseDateM = source.BaseDateM;
					destination.DayCount = source.DayCount;
					destination.RecordCount = source.RecordCount;
					destination.Sood_Mounthly = source.Sood_Mounthly;
					destination.Sood_Total = source.Sood_Total;
					destination.Sood_Percent = source.Sood_Percent;
					destination.Total_Sum = source.Total_Sum;
					destination.TafsilGuid = source.TafsilGuid;
					destination.SanadGuid = source.SanadGuid;
					destination.SanadType = source.SanadType;
					destination.Modified = source.Modified;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs", 106);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<RasGiriBusiness> MapAsync(string contextGuid, RasGiri source, RasGiriBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs"))
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
						destination = new RasGiriBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.BaseDateM = source.BaseDateM;
					destination.Guid = source.Guid;
					destination.Sood_Percent = source.Sood_Percent;
					destination.TafsilGuid = source.TafsilGuid;
					destination.SanadGuid = source.SanadGuid;
					destination.SanadType = source.SanadType;
					destination.Modified = source.Modified;
					destination.ChangeStatus = EntityChangeSt.Edited;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs", 136);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<RasGiri>> MapListAsync(string contextGuid, IEnumerable<RasGiriBusiness> source, List<RasGiri> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<RasGiriBusiness>> MapListAsync(string contextGuid, IEnumerable<RasGiri> source, List<RasGiriBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private RasGiriDetPersistanceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<RasGiri, RasGiriBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<RasGiriBusiness, RasGiri> MapToDatabase => Mapper;

	public override IItemMapper<RasGiri, RasGiriBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public RasGiriPersistanceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new RasGiriDetPersistanceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_detRepo?.Dispose();
				_detRepo = null;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs", 42);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(RasGiriBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs", 51));
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				RasGiri rasGiri = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(rasGiri, rasGiri.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs", 54);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAllByMasterGuidAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs", 56);
				if (ret.HasError)
				{
					return ret;
				}
				if (item.Items?.Any() ?? false)
				{
					foreach (RasGiriDetBusiness item2 in item.Items)
					{
						item2.MasterGuid = item.Guid;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveRangeAsync(item.Items.ToList(), tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs", 61);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs", 66);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriPersistanceRepository.cs", 67);
			}
			return ret;
		}
	}
}
