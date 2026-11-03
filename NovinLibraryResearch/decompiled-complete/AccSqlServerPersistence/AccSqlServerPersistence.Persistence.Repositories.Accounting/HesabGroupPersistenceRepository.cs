using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Coding.ReportsT;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class HesabGroupPersistenceRepository : PersistenceRepository<HesabGroupBusiness, HesabGroup>, IHesabGroupRepository, IRepository<HesabGroupBusiness>, INoGuidRepository<HesabGroupBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<HesabGroupBusiness, HesabGroup>, IItemAsyncMapper<HesabGroup, HesabGroupBusiness>, IItemMapper<HesabGroup, HesabGroupBusiness>
	{
		public Task<HesabGroup> MapAsync(string contextGuid, HesabGroupBusiness source, HesabGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs"))
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
						destination = new HesabGroup();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.Code = source.Code;
					destination.TypeGuid = source.TypeGuid;
					destination.Status = source.Status;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs", 158);
					throw;
				}
			}
		}

		public Task<HesabGroupBusiness> MapAsync(string contextGuid, HesabGroup source, HesabGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public HesabGroupBusiness Map(string contextGuid, HesabGroup source, HesabGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs"))
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
						destination = new HesabGroupBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Name = source.Name;
					destination.Code = source.Code;
					destination.TypeGuid = source.TypeGuid;
					destination.Status = source.Status;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs", 188);
					throw;
				}
			}
		}

		public Task<List<HesabGroup>> MapListAsync(string contextGuid, IEnumerable<HesabGroupBusiness> source, List<HesabGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<HesabGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<HesabGroup> source, List<HesabGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<HesabGroup, HesabGroupBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<HesabGroupBusiness, HesabGroup> MapToDatabase => Mapper;

	public override IItemMapper<HesabGroup, HesabGroupBusiness> MapFromDatabaseNoneAsync => Mapper;

	public HesabGroupPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs", 42);
		}
	}

	public async Task<IEnumerable<HesabGroupBusiness>> GetAllAsync(Guid? hesabTypeGuid, bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs", 46));
		IEnumerable<HesabGroupBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<HesabGroup> queryable = (IQueryable<HesabGroup>)c.HesabGroup;
				if ((hesabTypeGuid ?? Guid.Empty) != Guid.Empty)
				{
					queryable = queryable?.Where((HesabGroup q) => q.TypeGuid == ((Guid?)hesabTypeGuid).Value);
				}
				if (status.HasValue)
				{
					queryable = queryable?.Where((HesabGroup q) => q.Status == ((bool?)status).Value);
				}
				result = ((queryable != null && queryable.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs", 61);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<int> GetNextCodeHesabGroupAsync(Guid typeGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextCodeHesabGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs", 72));
		using (new JaegerMonitor("GetNextCodeHesabGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs"))
		{
			int result;
			try
			{
				int? num = ((IQueryable<HesabGroup>)acc_ModelContext.HesabGroup).Where((HesabGroup p) => p.TypeGuid == typeGuid)?.Select((HesabGroup q) => q.Code)?.MinOrDefault(100);
				HashSet<int> hashSet = ((IQueryable<HesabGroup>)acc_ModelContext.HesabGroup).Select((HesabGroup p) => p.Code).ToHashSet();
				int i;
				for (i = num.Value; hashSet.Contains(i); i++)
				{
				}
				result = i;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetNextCodeHesabGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs", 87);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GetNextCodeHesabGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<HesabGroupReportBusiness>> GetAllAsync(HesabGroupFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs", 98));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs"))
		{
			IEnumerable<HesabGroupReportBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<HesabGroup> queryable = (IQueryable<HesabGroup>)c.HesabGroup;
					if (filter != null)
					{
						if (filter.TypeGuid.HasValue)
						{
							queryable = queryable?.Where((HesabGroup q) => q.TypeGuid == filter.TypeGuid.Value);
						}
						if (filter.Status.HasValue)
						{
							queryable = queryable?.Where((HesabGroup q) => q.Status == filter.Status.Value);
						}
					}
					result = ((queryable != null && queryable.Any()) ? (await EntityFrameworkQueryableExtensions.ToListAsync<HesabGroupReportBusiness>(queryable.Select((HesabGroup q) => new HesabGroupReportBusiness
					{
						Guid = q.Guid,
						Name = q.Name,
						Code = q.Code,
						TypeName = q.HesabTypes.Name
					}), token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs", 122);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\HesabGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
