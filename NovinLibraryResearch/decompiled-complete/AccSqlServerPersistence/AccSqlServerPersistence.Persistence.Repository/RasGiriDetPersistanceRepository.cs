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
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repository;

internal class RasGiriDetPersistanceRepository : PersistenceRepository<RasGiriDetBusiness, RasGiriDet>, IRasGiriDetRepository, IRepository<RasGiriDetBusiness>, INoGuidRepository<RasGiriDetBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<RasGiriDetBusiness, RasGiriDet>, IItemAsyncMapper<RasGiriDet, RasGiriDetBusiness>
	{
		public Task<RasGiriDet> MapAsync(string contextGuid, RasGiriDetBusiness source, RasGiriDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs"))
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
						destination = new RasGiriDet();
					}
					destination.Guid = source.Guid;
					destination.DateM = source.DateM;
					destination.Price = source.Price;
					destination.Enheraf = source.Enheraf;
					destination.Description = source.Description;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs", 110);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<RasGiriDetBusiness> MapAsync(string contextGuid, RasGiriDet source, RasGiriDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs"))
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
						destination = new RasGiriDetBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.DateM = source.DateM;
					destination.Price = source.Price;
					destination.Description = source.Description;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs", 138);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<RasGiriDet>> MapListAsync(string contextGuid, IEnumerable<RasGiriDetBusiness> source, List<RasGiriDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<RasGiriDetBusiness>> MapListAsync(string contextGuid, IEnumerable<RasGiriDet> source, List<RasGiriDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<RasGiriDet, RasGiriDetBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<RasGiriDetBusiness, RasGiriDet> MapToDatabase => Mapper;

	public override IItemMapper<RasGiriDet, RasGiriDetBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public RasGiriDetPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<RasGiriDetBusiness>> GetAllByMasterGuidAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs", 33));
		using (new JaegerMonitor("GetAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs"))
		{
			IEnumerable<RasGiriDetBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					DbSet<RasGiriDet> rasGiriDet = c.RasGiriDet;
					List<RasGiriDet> source = await ((rasGiriDet != null) ? EntityFrameworkQueryableExtensions.ToListAsync<RasGiriDet>(((IQueryable<RasGiriDet>)rasGiriDet).Where((RasGiriDet x) => x.MasterGuid == masterGuid), token) : null);
					result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs", 44);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAllByMasterGuidAsync(Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs", 61));
				acc_ModelContext.RasGiriDet?.RemoveRange((IEnumerable<RasGiriDet>)((IQueryable<RasGiriDet>)acc_ModelContext.RasGiriDet).Where((RasGiriDet w) => w.MasterGuid == masterGuid).ToList());
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs", 67);
				ret.AddReturnedValue(ex2, "", "RemoveAllByMasterGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs", 68);
			}
			return ret;
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\RasGiriDetPersistanceRepository.cs", 82);
		}
	}
}
