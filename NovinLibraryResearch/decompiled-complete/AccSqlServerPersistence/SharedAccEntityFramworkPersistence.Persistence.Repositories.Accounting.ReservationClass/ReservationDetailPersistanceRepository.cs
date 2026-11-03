using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Reservation;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Reservation;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.ReservationClass;

internal class ReservationDetailPersistanceRepository : PersistenceMasterRepository<ReservationDetailBusiness, ReservationDetail>, IReservationDetailRepository, IPersistenceMasterRepository<ReservationDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ReservationDetail, ReservationDetailBusiness>, IItemAsyncMapper<ReservationDetailBusiness, ReservationDetail>
	{
		public async Task<ReservationDetailBusiness> MapAsync(string contextGuid, ReservationDetail source, ReservationDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			if (destination == null)
			{
				throw new NotImplementedException("Please Fill Destination !!");
			}
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.MasterGuid = source.MasterGuid;
				destination.Description = source.Description;
				destination.OrderGuid = source.OrderGuid;
				await destination.SetPlacementGuidAsync(source.PlacementGuid, EnSystem.Mapper, token, tranName);
				destination.StartDate = source.StartDate;
				destination.EndDate = source.EndDate;
				destination.PeopleCount = source.PeopleCount;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationDetailPersistanceRepository.cs", 98);
				throw;
			}
		}

		public Task<ReservationDetail> MapAsync(string contextGuid, ReservationDetailBusiness source, ReservationDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReservationDetail();
				}
				destination.Guid = source.Guid;
				destination.MasterGuid = source.MasterGuid;
				destination.Description = source.Description;
				destination.OrderGuid = source.OrderGuid;
				destination.PlacementGuid = source.PlacementGuid;
				destination.StartDate = source.StartDate;
				destination.EndDate = source.EndDate;
				destination.PeopleCount = source.PeopleCount;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationDetailPersistanceRepository.cs", 126);
				throw;
			}
		}

		public Task<List<ReservationDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<ReservationDetail> source, List<ReservationDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReservationDetail>> MapListAsync(string contextGuid, IEnumerable<ReservationDetailBusiness> source, List<ReservationDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<ReservationDetail, ReservationDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReservationDetailBusiness, ReservationDetail> MapToDatabase => Mapper;

	public ReservationDetailPersistanceRepository(ContextHolder dbContext)
		: base(dbContext)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationDetailPersistanceRepository.cs", 39);
		}
	}

	public override Task<List<ReservationDetailBusiness>> GetAllAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task GetAllAsync(ReservationBusiness master, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationDetailPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationDetailPersistanceRepository.cs", 45));
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					List<ReservationDetail> list = await EntityFrameworkQueryableExtensions.ToListAsync<ReservationDetail>(((IQueryable<ReservationDetail>)c.ReservationDetail).Where((ReservationDetail x) => x.MasterGuid == master.Guid), token);
					if (list == null || !list.Any())
					{
						return;
					}
					foreach (ReservationDetail item in list)
					{
						IItemAsyncMapper<ReservationDetail, ReservationDetailBusiness> mapFromDatabase = MapFromDatabase;
						string guid = base.Context.Guid;
						ReservationDetail source = item;
						ReservationDetailBusiness data = await mapFromDatabase.MapAsync(guid, source, await ReservationDetailBusiness.GetFactoryAsync(base.ContextGuid, master), tranName, token);
						await master.AddDetailAsync(data, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationDetailPersistanceRepository.cs", 61);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ReservationClass\\ReservationDetailPersistanceRepository.cs");
				}
			}
		}
	}
}
