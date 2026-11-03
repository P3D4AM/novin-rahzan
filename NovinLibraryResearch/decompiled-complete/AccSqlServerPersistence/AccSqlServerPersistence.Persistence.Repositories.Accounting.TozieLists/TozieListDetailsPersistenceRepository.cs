using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.TozieLists;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.TozieList;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.TozieList;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.TozieLists;

internal class TozieListDetailsPersistenceRepository : PersistenceMasterRepository<TozieListDetailBusiness, TozieListDetail>, ITozieListDetailRepository, IPersistenceMasterRepository<TozieListDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TozieListDetailBusiness, TozieListDetail>, IItemAsyncMapper<TozieListDetail, TozieListDetailBusiness>
	{
		public Task<TozieListDetail> MapAsync(string contextGuid, TozieListDetailBusiness source, TozieListDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TozieListDetail();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.Status = source.Status;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListDetailsPersistenceRepository.cs", 88);
			}
			return Task.FromResult(destination);
		}

		public Task<TozieListDetailBusiness> MapAsync(string contextGuid, TozieListDetail source, TozieListDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TozieListDetailBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.Status = source.Status;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListDetailsPersistenceRepository.cs", 107);
			}
			return Task.FromResult(destination);
		}

		public Task<List<TozieListDetail>> MapListAsync(string contextGuid, IEnumerable<TozieListDetailBusiness> source, List<TozieListDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TozieListDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TozieListDetail> source, List<TozieListDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _disposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TozieListDetail, TozieListDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TozieListDetailBusiness, TozieListDetail> MapToDatabase => Mapper;

	public new bool IsDisposed
	{
		get
		{
			if (!_disposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public TozieListDetailsPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_disposed)
			{
				_disposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListDetailsPersistenceRepository.cs", 34);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<TozieListDetailBusiness> items, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListDetailsPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (items != null)
				{
					if (items != null && items.Count() > 0)
					{
						BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListDetailsPersistenceRepository.cs", 46);
						foreach (TozieListDetailBusiness item in items)
						{
							TozieListDetail tozieListDetail = await MapToDatabase.MapAsync(base.ContextGuid, item, new TozieListDetail(), tranName, token);
							tozieListDetail.MasterGuid = masterGuid;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(tozieListDetail, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListDetailsPersistenceRepository.cs", 51);
						}
					}
				}
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
				WebErrorLog.Instance.StartLog(ex3, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\TozieLists\\TozieListDetailsPersistenceRepository.cs", 60);
				throw;
			}
			return ret;
		}
	}
}
