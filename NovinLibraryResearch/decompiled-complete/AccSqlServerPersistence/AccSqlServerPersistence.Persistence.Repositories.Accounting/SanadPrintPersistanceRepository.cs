using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class SanadPrintPersistanceRepository : PersistenceSyncRepository<SanadPrintBusiness, SanadPrint>, ISanadPrint, ISyncRepository<SanadPrintBusiness>, IEditableDelivery, IRepository<SanadPrintBusiness>, INoGuidRepository<SanadPrintBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SanadPrintBusiness, SanadPrint>, IItemAsyncMapper<SanadPrint, SanadPrintBusiness>
	{
		public Task<SanadPrint> MapAsync(string contextGuid, SanadPrintBusiness source, SanadPrint destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SanadPrint();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Tel = source.Tel;
				destination.Address = source.Address;
				destination.Mob = source.Mob;
				destination.Status = source.Status;
				destination.DateM = source.DateM;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.MobileLong = source.MobileLong;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs", 126);
				throw;
			}
		}

		public Task<SanadPrintBusiness> MapAsync(string contextGuid, SanadPrint source, SanadPrintBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new SanadPrintBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Tel = source.Tel;
				destination.Address = source.Address;
				destination.Mob = source.Mob;
				destination.DateM = source.DateM;
				destination.AsGuid = source.AsGuid;
				destination.AsType = source.AsType;
				destination.MobileLong = source.MobileLong;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs", 164);
				throw;
			}
		}

		public Task<List<SanadPrint>> MapListAsync(string contextGuid, IEnumerable<SanadPrintBusiness> source, List<SanadPrint> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SanadPrintBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadPrint> source, List<SanadPrintBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<SanadPrint, SanadPrintBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SanadPrintBusiness, SanadPrint> MapToDatabase => Mapper;

	public override IItemMapper<SanadPrint, SanadPrintBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public SanadPrintPersistanceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs", 41);
		}
	}

	public async Task<SanadPrintBusiness> GetAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs"))
		{
			SanadPrintBusiness result;
			try
			{
				_ = 2;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs", 49));
					DbSet<SanadPrint> sanadPrint = c.SanadPrint;
					SanadPrint source = await ((sanadPrint != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<SanadPrint>((IQueryable<SanadPrint>)sanadPrint, (Expression<Func<SanadPrint, bool>>)((SanadPrint x) => x.AsGuid == asGuid && x.AsType == asType), token) : null);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs", 57);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<SanadPrintBusiness> GetAsync(long number, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs"))
		{
			SanadPrintBusiness result;
			try
			{
				_ = 2;
				try
				{
					if (number < 9999999)
					{
						result = null;
					}
					else
					{
						c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs", 73));
						DbSet<SanadPrint> sanadPrint = c.SanadPrint;
						object obj;
						if (sanadPrint == null)
						{
							obj = null;
						}
						else
						{
							IQueryable<SanadPrint> queryable = ((IQueryable<SanadPrint>)sanadPrint).Where((SanadPrint x) => x.MobileLong == number);
							if (queryable == null)
							{
								obj = null;
							}
							else
							{
								IOrderedQueryable<SanadPrint> orderedQueryable = queryable.OrderByDescending((SanadPrint q) => q.DateM);
								obj = ((orderedQueryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<SanadPrint>((IQueryable<SanadPrint>)orderedQueryable, token) : null);
							}
						}
						SanadPrint source = await (Task<SanadPrint>)obj;
						result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs", 81);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadPrintPersistanceRepository.cs");
				}
			}
			return result;
		}
	}
}
