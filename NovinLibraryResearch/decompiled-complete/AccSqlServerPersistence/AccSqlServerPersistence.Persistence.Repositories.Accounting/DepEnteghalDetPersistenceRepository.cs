using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class DepEnteghalDetPersistenceRepository : PersistenceMasterRepository<DepEnteghalDetBusiness, DepEnteghalDet>, IDepEnteghalDetRepository, IPersistenceMasterRepository<DepEnteghalDetBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DepEnteghalDetBusiness, DepEnteghalDet>, IItemAsyncMapper<DepEnteghalDet, DepEnteghalDetBusiness>
	{
		public Task<DepEnteghalDet> MapAsync(string contextGuid, DepEnteghalDetBusiness source, DepEnteghalDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs"))
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
						destination = new DepEnteghalDet();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
					destination.DptGuidSrc = source.DptGuidSrc;
					destination.DptGuidDst = source.DptGuidDst;
					destination.PrdGuid = source.ProductGuid;
					destination.Quantity = source.Quantity;
					destination.VahedGuid = source.VahedGuid;
					destination.Description = source.Description;
					destination.Serial = source.Serial;
					destination.Price = source.Price;
					destination.V2Qt = source.V2Qt;
					destination.V2Price = source.V2Price;
					destination.Ordinal = source.Ordinal;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs", 129);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public async Task<DepEnteghalDetBusiness> MapAsync(string contextGuid, DepEnteghalDet source, DepEnteghalDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs"))
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
						destination = new DepEnteghalDetBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.MasterGuid = source.MasterGuid;
					destination.DptGuidSrc = source.DptGuidSrc;
					destination.DptGuidDst = source.DptGuidDst;
					await destination.SetPrdGuidAsync(source.PrdGuid, EnSystem.Mapper);
					destination.Quantity = source.Quantity;
					destination.VahedGuid = source.VahedGuid;
					destination.Description = source.Description;
					destination.Serial = source.Serial;
					destination.Price = source.Price;
					destination.V2Qt = source.V2Qt;
					destination.V2Price = source.V2Price;
					destination.Ordinal = source.Ordinal;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs", 167);
					throw;
				}
				return destination;
			}
		}

		public Task<List<DepEnteghalDet>> MapListAsync(string contextGuid, IEnumerable<DepEnteghalDetBusiness> source, List<DepEnteghalDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DepEnteghalDetBusiness>> MapListAsync(string contextGuid, IEnumerable<DepEnteghalDet> source, List<DepEnteghalDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<DepEnteghalDet, DepEnteghalDetBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<DepEnteghalDetBusiness, DepEnteghalDet> MapToDatabase => Mapper;

	public DepEnteghalDetPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs", 34);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<DepEnteghalDetBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (items?.Any((DepEnteghalDetBusiness p) => p.Guid == Guid.Empty) ?? false)
			{
				throw new ArgumentException("one or more items in list contains empty value for field Guid");
			}
			try
			{
				if (items != null)
				{
					if (items != null && items.Count() > 0)
					{
						short ordinal = 1;
						List<DepEnteghalDet> des = await MapToDatabase.MapListAsync(base.ContextGuid, items, null, tranName, token);
						BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs", 50);
						foreach (DepEnteghalDet item in des)
						{
							item.MasterGuid = masterGuid;
							item.Ordinal = ordinal;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs", 55);
							ordinal++;
						}
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs", 63);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs", 64);
			}
			return ret;
		}
	}

	public override async Task<List<DepEnteghalDetBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs", 71));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs"))
		{
			List<DepEnteghalDetBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<DepEnteghalDet> queryable = ((IQueryable<DepEnteghalDet>)c.DepEnteghalDets).Where((DepEnteghalDet x) => x.MasterGuid == masterGuid);
					List<DepEnteghalDet> source = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<DepEnteghalDet>(queryable, token) : null);
					result = (await MapFromDatabase.MapListAsync(base.ContextGuid, source, null, tranName, token))?.OrderBy((DepEnteghalDetBusiness q) => q.Ordinal)?.ToList();
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs", 84);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepEnteghalDetPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
