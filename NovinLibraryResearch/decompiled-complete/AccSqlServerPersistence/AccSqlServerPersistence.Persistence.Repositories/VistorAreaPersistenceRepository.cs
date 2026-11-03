using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class VistorAreaPersistenceRepository : PersistenceSyncRepository<VisitorAreaBusiness, VisitorArea>, IVistorAreaRepository, ISyncRepository<VisitorAreaBusiness>, IEditableDelivery, IRepository<VisitorAreaBusiness>, INoGuidRepository<VisitorAreaBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<VisitorAreaBusiness, VisitorArea>, IItemAsyncMapper<VisitorArea, VisitorAreaBusiness>, IItemMapper<VisitorArea, VisitorAreaBusiness>
	{
		public Task<VisitorArea> MapAsync(string contextGuid, VisitorAreaBusiness source, VisitorArea destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new VisitorArea();
				}
				destination.Level = source.Level;
				destination.Description = source.Description ?? "";
				destination.Name = source.Name;
				destination.Status = source.Status;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ParentGuid = source.ParentGuid;
				destination.FullCode = source.FullCode;
				destination.Code = source.Code;
				destination.ServerStatus = source.ServerStatus;
				destination.HostRefrenced = source.HostRefrenced ?? "";
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage ?? "";
				destination.DeliveryShortMessage = source.DeliveryShortMessage ?? "";
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 152);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<VisitorAreaBusiness> MapAsync(string contextGuid, VisitorArea source, VisitorAreaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public VisitorAreaBusiness Map(string contextGuid, VisitorAreaBusiness source, VisitorAreaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs"))
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
						destination = new VisitorAreaBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Level = source.Level;
					destination.Description = source.Description;
					destination.Name = source.Name;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ParentGuid = source.ParentGuid;
					destination.ChangeStatus = EntityChangeSt.Edited;
					destination.FullCode = source.FullCode;
					destination.Code = source.Code;
					destination.ServerStatus = source.ServerStatus;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 191);
					throw;
				}
				return destination;
			}
		}

		public Task<List<VisitorArea>> MapListAsync(string contextGuid, IEnumerable<VisitorAreaBusiness> source, List<VisitorArea> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<VisitorAreaBusiness>> MapListAsync(string contextGuid, IEnumerable<VisitorArea> source, List<VisitorAreaBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public VisitorAreaBusiness Map(string contextGuid, VisitorArea source, VisitorAreaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs"))
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
						destination = new VisitorAreaBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Description = source.Description;
					destination.Name = source.Name;
					destination.Status = source.Status;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Code = source.Code;
					destination.FullCode = source.FullCode;
					destination.ParentGuid = source.ParentGuid;
					destination.Level = source.Level;
					destination.ServerStatus = source.ServerStatus;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 232);
					throw;
				}
				return destination;
			}
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => false;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<VisitorArea, VisitorAreaBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<VisitorAreaBusiness, VisitorArea> MapToDatabase => Mapper;

	public override IItemMapper<VisitorArea, VisitorAreaBusiness> MapFromDatabaseNoneAsync => Mapper;

	public VistorAreaPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 39);
		}
	}

	public async Task<VisitorAreaBusiness> GetAsync(long code, Guid? parentGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 43));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs"))
		{
			VisitorAreaBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (parentGuid.HasValue && parentGuid == Guid.Empty)
					{
						parentGuid = null;
					}
					VisitorArea visitorArea = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<VisitorArea>((IQueryable<VisitorArea>)c.VisitorAreas, (Expression<Func<VisitorArea, bool>>)((VisitorArea x) => x.Code == code && x.ParentGuid == parentGuid), token);
					result = ((visitorArea != null) ? (await MapFromDatabase.MapAsync(ContextGuid, visitorArea, new VisitorAreaBusiness(ContextGuid), tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 58);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<VisitorAreaBusiness>> GetAllAsync(Guid? parentGuid, bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 69));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs"))
		{
			IEnumerable<VisitorAreaBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<VisitorArea> queryable = (IQueryable<VisitorArea>)c.VisitorAreas;
					if (status.HasValue)
					{
						queryable = queryable.Where((VisitorArea q) => q.Status == ((bool?)status).Value);
					}
					if ((parentGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((VisitorArea q) => q.ParentGuid == parentGuid);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 83);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<long> GetNextCodeAsync(short? level, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 94));
		using (new JaegerMonitor("GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs"))
		{
			long result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<VisitorArea> queryable = (from q in (IQueryable<VisitorArea>)c.VisitorAreas
						where ((level ?? 0) == 0 || q.Level == ((short?)level).Value) && q.Status
						select q into x
						orderby x.Code descending
						select x).Take(1);
					VisitorArea visitorArea = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<VisitorArea>(queryable, token) : null);
					result = ((visitorArea != null) ? (visitorArea.Code + 1) : 100);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs", 106);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\VistorAreaPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<IEnumerable<VisitorAreaBusiness>> GetChildrenAsync(Guid parentGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("Please Use Cache");
	}
}
