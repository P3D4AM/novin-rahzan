using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Business.Accounting.Bill.Reports;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Bill;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer.Models.Hotel;
using PacketParser.BusinessLayer.Models.Placement;
using PacketParser.Entities;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal abstract class PlacementPersistenceRepositoryBase : PersistenceRepository<PlacementBusiness, Placement>, IPlacementRepository, IRepository<PlacementBusiness>, INoGuidRepository<PlacementBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PlacementBusiness, Placement>, IItemAsyncMapper<Placement, PlacementBusiness>
	{
		public async Task<Placement> MapAsync(string contextGuid, PlacementBusiness source, Placement destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Placement();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.GroupGuid = source.GroupGuid;
				destination.ItemCount = source.ItemCount;
				destination.MasterGuid = source.MasterGuid;
				destination.Price = source.Price;
				destination.ExtraPrice = source.ExtraPrice;
				Placement placement = destination;
				placement.Number = await source.GetNumberAsync(EnSystem.Mapper, token, tranName);
				destination.Title = source.Title;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 252);
				throw;
			}
		}

		public Task<PlacementBusiness> MapAsync(string contextGuid, Placement source, PlacementBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PlacementBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.SetNumber(source.Number);
				destination.GroupGuid = source.GroupGuid;
				destination.ItemCount = source.ItemCount;
				destination.MasterGuid = source.MasterGuid;
				destination.Price = source.Price;
				destination.ExtraPrice = source.ExtraPrice;
				destination.Title = source.Title;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 284);
				throw;
			}
		}

		public Task<List<Placement>> MapListAsync(string contextGuid, IEnumerable<PlacementBusiness> source, List<Placement> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PlacementBusiness>> MapListAsync(string contextGuid, IEnumerable<Placement> source, List<PlacementBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	internal class PlacementWhitEmptyConstructor : PlacementBusiness
	{
		public PlacementWhitEmptyConstructor()
			: base(Guid.Empty.ToString())
		{
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Placement, PlacementBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Placement, PlacementBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PlacementBusiness, Placement> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public PlacementPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 45);
		}
	}

	public async Task<PlacementBusiness> GetAsync(int number, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 49));
		PlacementBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Placement placement = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Placement>((IQueryable<Placement>)c.Placement, (Expression<Func<Placement, bool>>)((Placement q) => q.Number == number), token);
				result = ((placement != null) ? (await MapFromDatabase.MapAsync(ContextGuid, placement, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 61);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<int> GetNextNumber(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 72));
		using (new JaegerMonitor("GetNextNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Placement>(((IQueryable<Placement>)c.Placement).OrderByDescending((Placement x) => x.Number).Take(1), token))?.Number ?? 0) + 1;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetNextNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 83);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<Guid>> GetTypePlacementListAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetTypePlacementListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 94));
		using (new JaegerMonitor("GetTypePlacementListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs"))
		{
			List<Guid> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Guid> queryable = from q in (IQueryable<Placement>)c.Placement
						where q.Status
						select q.GroupGuid ?? Guid.Empty;
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<Guid> queryable2 = queryable.Distinct();
						obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<Guid>(queryable2, token) : null);
					}
					result = await (Task<List<Guid>>)obj;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetTypePlacementListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 104);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetTypePlacementListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<RoomTypeModel>> GetTypeRoomCount(CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetTypeRoomCount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 115));
		using (new JaegerMonitor("GetTypeRoomCount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs"))
		{
			List<RoomTypeModel> result;
			try
			{
				result = (from q in (IQueryable<Placement>)acc_ModelContext.Placement
					group q by q.GroupGuid into q
					select new RoomTypeModel
					{
						Count = q.Count(),
						GroupGuid = q.Key.Value
					})?.ToList();
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetTypeRoomCount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 129);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("GetTypeRoomCount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<PlacementReportsBusiness>> GetAllAsync(RoomFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 140));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs"))
		{
			IEnumerable<PlacementReportsBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Placement> queryable = (IQueryable<Placement>)c.Placement;
					if (filter != null)
					{
						if (filter.Status.HasValue)
						{
							queryable = queryable?.Where((Placement q) => q.Status == filter.Status.Value);
						}
						if ((filter?.GroupGuid ?? Guid.Empty) != Guid.Empty)
						{
							queryable = queryable?.Where((Placement q) => q.GroupGuid == filter.GroupGuid.Value);
						}
						if ((filter?.MasterGuid ?? Guid.Empty) != Guid.Empty)
						{
							queryable = queryable?.Where((Placement q) => q.MasterGuid == filter.MasterGuid.Value);
						}
					}
					IQueryable<PlacementReportsBusiness> queryable2 = queryable.Select((Placement q) => new PlacementReportsBusiness
					{
						Number = q.Number,
						Guid = q.Guid,
						PlacementGroupName = q.ProductGroup.Name,
						ItemCount = q.ItemCount,
						ExtraPrice = q.ExtraPrice,
						MasterGuid = q.MasterGuid,
						SumPrice = q.Price + q.PlacementFacilities.Sum((PlacementFacilitiesRelation q) => q.Facility.Price),
						Description = q.Description,
						Status = q.Status,
						Title = q.Title,
						State = (q.PlacementOrderRelation.Any() ? EnPlacementStatus.Full : (q.ReservationDetail.Where((ReservationDetail q) => q.StartDate <= DateTime.Now && q.EndDate >= DateTime.Now && q.MasterItem.Status).Any() ? EnPlacementStatus.Reserve : EnPlacementStatus.Free)),
						HallName = ((q.Master == null) ? "" : q.Master.Title),
						LastReserveStartDate = (from q in q.ReservationDetail
							where q.StartDate <= DateTime.Now && q.EndDate >= DateTime.Now && q.MasterItem.Status
							orderby q.StartDate
							select q).FirstOrDefault().StartDate,
						LastReserveEndDate = (from q in q.ReservationDetail
							where q.StartDate <= DateTime.Now && q.EndDate >= DateTime.Now && q.MasterItem.Status
							orderby q.StartDate
							select q).FirstOrDefault().EndDate,
						LastReserveDescription = (from q in q.ReservationDetail
							where q.StartDate <= DateTime.Now && q.EndDate >= DateTime.Now && q.MasterItem.Status
							orderby q.StartDate
							select q).FirstOrDefault().Description,
						LastReserveTafsilName = (from q in q.ReservationDetail
							where q.StartDate <= DateTime.Now && q.EndDate >= DateTime.Now && q.MasterItem.Status
							orderby q.StartDate
							select q).FirstOrDefault().MasterItem.TafsilItem.Name,
						LastReservePeopleCount = (from q in q.ReservationDetail
							where q.StartDate <= DateTime.Now && q.EndDate >= DateTime.Now && q.MasterItem.Status
							orderby q.StartDate
							select q).FirstOrDefault().PeopleCount,
						LastReserveNumber = (from q in q.ReservationDetail
							where q.StartDate <= DateTime.Now && q.EndDate >= DateTime.Now && q.MasterItem.Status
							orderby q.StartDate
							select q).FirstOrDefault().MasterItem.Number
					});
					result = await ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<PlacementReportsBusiness>(queryable2, default(CancellationToken)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 177);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public override async Task<PlacementBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 188));
		PlacementBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 3;
			try
			{
				Placement source = await c.Placement.FindAsync(new object[1] { guid });
				PlacementBusiness mapdata = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
				IQueryable<PlacementFacilitiesRelation> queryable = ((IQueryable<PlacementFacilitiesRelation>)c.PlacementFacilitieRelation).Where((PlacementFacilitiesRelation q) => q.PlacementGuid == guid);
				if (queryable != null && queryable.Any())
				{
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<FacilitiesBusinessWithEmptyConstructor> queryable2 = queryable.Select((PlacementFacilitiesRelation q) => new FacilitiesBusinessWithEmptyConstructor
						{
							ContextGuid = ContextGuid,
							Guid = q.FacilitiesGuid,
							Price = q.Facility.Price,
							Modified = q.Facility.Modified,
							Name = q.Facility.Name
						});
						obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<FacilitiesBusinessWithEmptyConstructor>(queryable2, default(CancellationToken)) : null);
					}
					await mapdata.AddRengeFacilities(await (Task<List<FacilitiesBusinessWithEmptyConstructor>>)obj);
				}
				result = mapdata;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs", 212);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Bill\\PlacementPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public virtual Task<IEnumerable<PlacementKardexViewModel>> GetAllKardexAsync(PlacementKardexFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
