using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Aghsat;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Aghsat;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.Filters;
using PacketParser.Filters.Khazane;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repository.Accounting.Aghsat;

public abstract class AghsatShPersistanceRepositoryBase : PersistenceSyncRepository<AghsatShBusiness, AghsatSh>, IAghsatShRepository, ISyncRepository<AghsatShBusiness>, IEditableDelivery, IRepository<AghsatShBusiness>, INoGuidRepository<AghsatShBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AghsatShBusiness, AghsatSh>, IItemAsyncMapper<AghsatSh, AghsatShBusiness>
	{
		public Task<AghsatSh> MapAsync(string contextGuid, AghsatShBusiness source, AghsatSh destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AghsatSh();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ID2 = source.ID2;
				destination.TafsilGuid = source.TafsilGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.Description = source.Description;
				destination.DateSarResid = source.DateSarResid;
				destination.DateM = source.DateM;
				destination.UserGuid = source.UserGuid;
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.Price = source.Price;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.Source = source.Source;
				destination.ChangeStatus = source.ChangeStatus;
				destination.MetaData = source.MetaData?.ToString() ?? "";
				destination.DateUtc = destination.DateM.ToUniversalTime();
				destination.PardakhtDateM = (((source?.PardakhtGuid ?? Guid.Empty) != Guid.Empty) ? source.PardakhtDateM : null);
				destination.PardakhtSum = (((source?.PardakhtGuid ?? Guid.Empty) != Guid.Empty) ? source.PardakhtSum : 0m);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 299);
				throw;
			}
		}

		public async Task<AghsatShBusiness> MapAsync(string contextGuid, AghsatSh source, AghsatShBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AghsatShBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ID2 = source.ID2;
				destination.TafsilGuid = source.TafsilGuid;
				AghsatShBusiness aghsatShBusiness = destination;
				aghsatShBusiness.FullName = (await TafsilBusiness.GetAsync(contextGuid, EnSystem.Mapper, source.TafsilGuid, tranName, token, "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs"))?.Name ?? "";
				destination.MoeinGuid = source.MoeinGuid;
				destination.Description = source.Description;
				destination.DateSarResid = source.DateSarResid;
				destination.DateM = source.DateM;
				destination.UserGuid = source.UserGuid;
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.Price = source.Price;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.Source = source.Source;
				destination.ChangeStatus = EntityChangeSt.Edited;
				destination.MetaData = source.MetaData.FromJson<PardakhtChequeShMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 334);
				destination.DateUtc = source.DateUtc;
				destination.ChangeStatus = EntityChangeSt.Edited;
				destination.PardakhtDateM = source.PardakhtDateM;
				destination.PardakhtSum = source.PardakhtSum;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 345);
				throw;
			}
		}

		public Task<List<AghsatSh>> MapListAsync(string contextGuid, IEnumerable<AghsatShBusiness> source, List<AghsatSh> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AghsatShBusiness>> MapListAsync(string contextGuid, IEnumerable<AghsatSh> source, List<AghsatShBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AghsatSh, AghsatShBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AghsatShBusiness, AghsatSh> MapToDatabase => Mapper;

	public override IItemMapper<AghsatSh, AghsatShBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public AghsatShPersistanceRepositoryBase(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 46);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(AghsatShBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				AghsatShBusiness aghsatShBusiness = await GetAsync(item.Guid, token, tranName);
				if (aghsatShBusiness != null && aghsatShBusiness.MetaData != null && item.MetaData == null)
				{
					throw new Exception("metaData for aghsatM is removed!");
				}
				return await base.SaveAsync(item, tryCount, token, tranName);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 61);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 62);
			}
			return ret;
		}
	}

	public Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<int> NextId2Async(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 69));
		using (new JaegerMonitor("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<AghsatSh>(((IQueryable<AghsatSh>)c.AghsatSh).OrderByDescending((AghsatSh x) => x.ID2).Take(1), token))?.ID2 ?? 0) + 1;
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
					WebErrorLog.Instance.StartLog(ex3, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 81);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AghsatShBusiness>> GetAllAsync(AghsatShFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 92));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs"))
		{
			IEnumerable<AghsatShBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<string> list = filter.SearchString?.FixString()?.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<AghsatSh> queryable = (IQueryable<AghsatSh>)EntityFrameworkQueryableExtensions.Include<AghsatSh, Tafsil>((IQueryable<AghsatSh>)c.AghsatSh, (Expression<Func<AghsatSh, Tafsil>>)((AghsatSh x) => x.Tafsils));
						foreach (string word in list)
						{
							if (!string.IsNullOrEmpty(word))
							{
								queryable = queryable.Where((AghsatSh p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.Tafsils.Name, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.ID2.ToString(), $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.Price.ToString(), $"%{word}%"));
							}
						}
						if (filter.DateFrom.HasValue)
						{
							queryable = queryable.Where((AghsatSh p) => p.DateSarResid >= filter.DateFrom.Value);
						}
						if (filter.DateTo.HasValue)
						{
							queryable = queryable.Where((AghsatSh p) => p.DateSarResid <= filter.DateTo.Value);
						}
						if ((filter.PardakhtGuid ?? Guid.Empty) != Guid.Empty)
						{
							queryable = queryable?.Where((AghsatSh q) => q.PardakhtGuid == filter.PardakhtGuid.Value);
						}
						if ((filter.TafsilGuid ?? Guid.Empty) != Guid.Empty)
						{
							queryable = queryable?.Where((AghsatSh q) => q.TafsilGuid == filter.TafsilGuid.Value);
						}
						if (token.IsCancellationRequested)
						{
							throw new OperationCanceledException();
						}
						if (queryable == null || !queryable.Any())
						{
							result = null;
						}
						else
						{
							object obj;
							if (queryable == null)
							{
								obj = null;
							}
							else
							{
								IQueryable<AghsatShBusinessWithEmptyConstructor> queryable2 = queryable.Select((AghsatSh p) => new AghsatShBusinessWithEmptyConstructor
								{
									ChangeStatus = EntityChangeSt.Edited,
									Status = p.Status,
									ContextGuid = ContextGuid,
									DateM = p.DateM,
									DateSarResid = p.DateSarResid,
									DeliveryDate = p.DeliveryDate,
									DeliveryId = p.DeliveryId,
									DeliveryMessage = p.DeliveryMessage,
									DeliveryShortMessage = p.DeliveryShortMessage,
									Description = p.Description,
									Guid = p.Guid,
									HostRefrenced = p.HostRefrenced,
									ID2 = p.ID2,
									Modified = p.Modified,
									MoeinGuid = p.MoeinGuid,
									PardakhtGuid = p.PardakhtGuid,
									Price = p.Price,
									ServerStatus = p.ServerStatus,
									Source = p.Source,
									TafsilGuid = p.TafsilGuid,
									FullName = p.Tafsils.Name,
									UserGuid = p.UserGuid,
									PardakhtSum = p.PardakhtSum,
									PardakhtDateM = p.PardakhtDateM,
									St = ((!p.Status) ? EnAghsatShStatus.Removed : (((p.PardakhtGuid ?? Guid.Empty) == Guid.Empty || p.PardakhtSum <= 0m) ? EnAghsatShStatus.NotPayed : EnAghsatShStatus.Payed)),
									MetaData = p.MetaData.FromJson<PardakhtChequeShMetaData>("AghsatShPersistanceRepository", "GetAllAsync", 1)
								});
								obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<AghsatShBusinessWithEmptyConstructor>(queryable2, token) : null);
							}
							List<AghsatShBusinessWithEmptyConstructor> list2 = await (Task<List<AghsatShBusinessWithEmptyConstructor>>)obj;
							if (filter.Status.HasValue)
							{
								list2 = list2?.Where((AghsatShBusinessWithEmptyConstructor p) => p.St == filter.Status)?.ToList();
							}
							if (token.IsCancellationRequested)
							{
								throw new OperationCanceledException();
							}
							result = list2;
						}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 150);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<AghsatShBusiness> GetAsync(int id2, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 157));
		AghsatShBusiness result;
		try
		{
			_ = 1;
			try
			{
				AghsatSh aghsatSh = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<AghsatSh>((IQueryable<AghsatSh>)EntityFrameworkQueryableExtensions.Include<AghsatSh, Tafsil>((IQueryable<AghsatSh>)c.AghsatSh, (Expression<Func<AghsatSh, Tafsil>>)((AghsatSh x) => x.Tafsils)), (Expression<Func<AghsatSh, bool>>)((AghsatSh q) => q.ID2 == id2), token);
				result = ((aghsatSh != null) ? new AghsatShBusinessWithEmptyConstructor
				{
					ChangeStatus = EntityChangeSt.Edited,
					Status = aghsatSh.Status,
					ContextGuid = ContextGuid,
					DateM = aghsatSh.DateM,
					DateSarResid = aghsatSh.DateSarResid,
					DeliveryDate = aghsatSh.DeliveryDate,
					DeliveryId = aghsatSh.DeliveryId,
					DeliveryMessage = aghsatSh.DeliveryMessage,
					DeliveryShortMessage = aghsatSh.DeliveryShortMessage,
					Description = aghsatSh.Description,
					Guid = aghsatSh.Guid,
					HostRefrenced = aghsatSh.HostRefrenced,
					ID2 = aghsatSh.ID2,
					Modified = aghsatSh.Modified,
					MoeinGuid = aghsatSh.MoeinGuid,
					PardakhtGuid = aghsatSh.PardakhtGuid,
					Price = aghsatSh.Price,
					ServerStatus = aghsatSh.ServerStatus,
					Source = aghsatSh.Source,
					TafsilGuid = aghsatSh.TafsilGuid,
					FullName = aghsatSh.Tafsils.Name,
					UserGuid = aghsatSh.UserGuid,
					PardakhtSum = aghsatSh.PardakhtSum,
					PardakhtDateM = aghsatSh.PardakhtDateM,
					St = ((!aghsatSh.Status) ? EnAghsatShStatus.Removed : ((!((aghsatSh.PardakhtGuid ?? Guid.Empty) == Guid.Empty) && !(aghsatSh.PardakhtSum <= 0m)) ? EnAghsatShStatus.Payed : EnAghsatShStatus.NotPayed)),
					MetaData = aghsatSh.MetaData.FromJson<PardakhtChequeShMetaData>("AghsatShPersistanceRepository", "GetAsync", 1)
				} : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 196);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<AghsatShBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 207));
		AghsatShBusiness result;
		try
		{
			_ = 1;
			try
			{
				AghsatSh aghsatSh = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<AghsatSh>((IQueryable<AghsatSh>)EntityFrameworkQueryableExtensions.Include<AghsatSh, Tafsil>((IQueryable<AghsatSh>)c.AghsatSh, (Expression<Func<AghsatSh, Tafsil>>)((AghsatSh x) => x.Tafsils)), (Expression<Func<AghsatSh, bool>>)((AghsatSh q) => q.Guid == guid), token);
				result = ((aghsatSh != null) ? new AghsatShBusinessWithEmptyConstructor
				{
					ChangeStatus = EntityChangeSt.Edited,
					Status = aghsatSh.Status,
					ContextGuid = ContextGuid,
					DateM = aghsatSh.DateM,
					DateSarResid = aghsatSh.DateSarResid,
					DeliveryDate = aghsatSh.DeliveryDate,
					DeliveryId = aghsatSh.DeliveryId,
					DeliveryMessage = aghsatSh.DeliveryMessage,
					DeliveryShortMessage = aghsatSh.DeliveryShortMessage,
					Description = aghsatSh.Description,
					Guid = aghsatSh.Guid,
					HostRefrenced = aghsatSh.HostRefrenced,
					ID2 = aghsatSh.ID2,
					Modified = aghsatSh.Modified,
					MoeinGuid = aghsatSh.MoeinGuid,
					PardakhtGuid = aghsatSh.PardakhtGuid,
					Price = aghsatSh.Price,
					ServerStatus = aghsatSh.ServerStatus,
					Source = aghsatSh.Source,
					TafsilGuid = aghsatSh.TafsilGuid,
					FullName = aghsatSh.Tafsils.Name,
					UserGuid = aghsatSh.UserGuid,
					PardakhtSum = aghsatSh.PardakhtSum,
					PardakhtDateM = aghsatSh.PardakhtDateM,
					St = ((!aghsatSh.Status) ? EnAghsatShStatus.Removed : ((!((aghsatSh.PardakhtGuid ?? Guid.Empty) == Guid.Empty) && !(aghsatSh.PardakhtSum <= 0m)) ? EnAghsatShStatus.Payed : EnAghsatShStatus.NotPayed)),
					MetaData = aghsatSh.MetaData.FromJson<PardakhtChequeShMetaData>("AghsatShPersistanceRepository", "GetAsync", 1)
				} : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs", 246);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatShPersistanceRepository.cs");
			}
		}
		return result;
	}

	public virtual Task<List<AghsatSHSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdatePardakhtInfoAsync(Guid? pardakhtGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
