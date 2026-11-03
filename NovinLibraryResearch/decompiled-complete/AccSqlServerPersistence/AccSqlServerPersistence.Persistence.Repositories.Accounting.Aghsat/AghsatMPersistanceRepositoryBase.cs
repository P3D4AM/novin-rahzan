using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting.Aghsat;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Aghsat;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Aghsat;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.Filters;
using PacketParser.Filters.Configs;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Aghsat;

public abstract class AghsatMPersistanceRepositoryBase : PersistenceSyncRepository<AghsatMBusiness, AghsatM>, IAghsatMRepository, ISyncRepository<AghsatMBusiness>, IEditableDelivery, IRepository<AghsatMBusiness>, INoGuidRepository<AghsatMBusiness>, IDisposable
{
	public class MapperClass : IItemAsyncMapper<AghsatMBusiness, AghsatM>, IItemAsyncMapper<AghsatM, AghsatMBusiness>
	{
		public async Task<AghsatM> MapAsync(string contextGuid, AghsatMBusiness source, AghsatM destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			_ = 1;
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				if (destination == null)
				{
					destination = new AghsatM();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Id2 = source.Id2;
				destination.Description = source.Description;
				destination.DateSarResid = source.DateSarResid;
				destination.Date = source.Date;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.Price = source.GetPrice();
				destination.TypePardakht = source.TypePardakht;
				destination.Mounth = source.Mounth;
				destination.GuidGroup = source.GuidGroup;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.MetaData = source.MetaData?.ToString() ?? "";
				destination.RasGiriType = source.RasGiriType;
				destination.VajhEltezam = source.VajhEltezam;
				destination.SanadNo = source.SanadNo;
				destination.VajhEltezamModified = source.VajhEltezamModified;
				destination.DateUtc = destination.Date.ToUniversalTime();
				AghsatM aghsatM = destination;
				aghsatM.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync(token);
				aghsatM = destination;
				aghsatM.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync(token);
				destination.ReceptionSum = (((source?.ReceptionGuid ?? Guid.Empty) != Guid.Empty) ? source.ReceptionSum : 0m);
				destination.ReceptionDateM = (((source?.ReceptionGuid ?? Guid.Empty) != Guid.Empty) ? source.ReceptionDateM : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 300);
				throw;
			}
		}

		public Task<AghsatMBusiness> MapAsync(string contextGuid, AghsatM source, AghsatMBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AghsatMBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Id2 = source.Id2;
				destination.Description = source.Description;
				destination.DateSarResid = source.DateSarResid;
				destination.Date = source.Date;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.Price = source.Price;
				destination.TypePardakht = source.TypePardakht;
				destination.Mounth = source.Mounth;
				destination.GuidGroup = source.GuidGroup;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.ChangeStatus = EntityChangeSt.Edited;
				destination.MetaData = source.MetaData.FromJson<AghsatMMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 333);
				destination.RasGiriType = source.RasGiriType;
				destination.VajhEltezam = source.VajhEltezam;
				destination.DateUtc = source.DateUtc;
				destination.SanadNo = source.SanadNo;
				destination.VajhEltezamModified = source.VajhEltezamModified;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
				destination.ReceptionSum = source.ReceptionSum;
				destination.ReceptionDateM = source.ReceptionDateM;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 349);
				throw;
			}
		}

		public Task<List<AghsatM>> MapListAsync(string contextGuid, IEnumerable<AghsatMBusiness> source, List<AghsatM> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AghsatMBusiness>> MapListAsync(string contextGuid, IEnumerable<AghsatM> source, List<AghsatMBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private MapperClass Mapper { get; set; } = new MapperClass();


	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AghsatM, AghsatMBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AghsatMBusiness, AghsatM> MapToDatabase => Mapper;

	public override IItemMapper<AghsatM, AghsatMBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public AghsatMPersistanceRepositoryBase(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 45);
		}
	}

	public async Task<int> NextId2Async(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 49));
		using (new JaegerMonitor("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs"))
		{
			int result;
			try
			{
				_ = 1;
				try
				{
					result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<AghsatM>(((IQueryable<AghsatM>)c.AghsatM).OrderByDescending((AghsatM x) => x.Id2).Take(1), token))?.Id2 ?? 0) + 1;
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
					WebErrorLog.Instance.StartLog(ex3, "", "NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 61);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<AghsatMBusiness>> GetAllByGroupGuidAsync(Guid groupGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllByGroupGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 72));
		IEnumerable<AghsatMBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<AghsatM> source = ((IQueryable<AghsatM>)c.AghsatM).Where((AghsatM p) => p.GuidGroup == groupGuid);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllByGroupGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 82);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllByGroupGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs");
			}
		}
		return result;
	}

	public async Task<List<AghsatMBusiness>> GetAllNotPayedAsync(Guid? hGuid, HesabType? hesabType, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotPayedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 93));
		using (new JaegerMonitor("GetAllNotPayedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs"))
		{
			List<AghsatMBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<AghsatM> queryable = ((IQueryable<AghsatM>)EntityFrameworkQueryableExtensions.Include<AghsatM, AghsatM_Group>((IQueryable<AghsatM>)c.AghsatM, (Expression<Func<AghsatM, AghsatM_Group>>)((AghsatM q) => q.Groups))).Where((AghsatM q) => q.ReceptionGuid == null || q.ReceptionGuid == Guid.Empty);
					if (hGuid.HasValue)
					{
						queryable = queryable.Where((AghsatM q) => q.Groups.TafsilGuid == ((Guid?)hGuid).Value);
					}
					IItemAsyncMapper<AghsatM, AghsatMBusiness> mapFromDatabase = MapFromDatabase;
					string contextGuid = ContextGuid;
					result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<AghsatM>(queryable, token), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotPayedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 105);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotPayedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual async Task<IEnumerable<AghsatMBusiness>> GetAllAsync(CancellationToken token, AghsatMFilter filter, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 116));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs"))
		{
			IEnumerable<AghsatMBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<AghsatM> aghsatM = (IQueryable<AghsatM>)c.AghsatM;
					aghsatM = (IQueryable<AghsatM>)EntityFrameworkQueryableExtensions.Include<AghsatM, AghsatM_Group>((IQueryable<AghsatM>)EntityFrameworkQueryableExtensions.Include<AghsatM, Reception>(aghsatM, (Expression<Func<AghsatM, Reception>>)((AghsatM q) => q.Receptions)), (Expression<Func<AghsatM, AghsatM_Group>>)((AghsatM q) => q.Groups));
					if (filter == null)
					{
						goto IL_0929;
					}
					if (!token.IsCancellationRequested)
					{
						if (filter.PardakhtType != 0)
						{
							aghsatM = aghsatM?.Where((AghsatM q) => q.TypePardakht == filter.PardakhtType);
						}
						if (filter.CusGroupGuid.HasValue && filter.CusGroupGuid != Guid.Empty)
						{
							aghsatM = aghsatM?.Where((AghsatM q) => q.Groups.Tafsils.Customer.GroupGuid == filter.CusGroupGuid);
						}
						if (filter.ReceptionGuid.HasValue && filter.ReceptionGuid != Guid.Empty)
						{
							aghsatM = aghsatM?.Where((AghsatM q) => q.ReceptionGuid == filter.ReceptionGuid);
						}
						if (filter.GroupGuid.HasValue && filter.GroupGuid != Guid.Empty)
						{
							aghsatM = aghsatM?.Where((AghsatM q) => q.GuidGroup == filter.GroupGuid);
						}
						if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
						{
							aghsatM = aghsatM.Where((AghsatM q) => q.Groups.TafsilGuid == filter.TafsilGuid);
						}
						if (filter.TafsilGuidList?.Any() ?? false)
						{
							aghsatM = aghsatM?.Where((AghsatM q) => filter.TafsilGuidList.Contains(q.Groups.TafsilGuid));
						}
						if (filter.DateFrom.HasValue)
						{
							aghsatM = aghsatM?.Where((AghsatM q) => q.DateSarResid >= filter.DateFrom);
						}
						if (filter.DateTo.HasValue)
						{
							aghsatM = aghsatM?.Where((AghsatM q) => q.DateSarResid <= filter.DateTo);
						}
						goto IL_0929;
					}
					result = null;
					goto end_IL_0112;
					IL_0929:
					if (token.IsCancellationRequested || aghsatM == null || !aghsatM.Any())
					{
						result = null;
					}
					else
					{
						object obj;
						if (aghsatM == null)
						{
							obj = null;
						}
						else
						{
							IQueryable<AghsatMBusinessWithEmptyConstructor> queryable = aghsatM.Select((AghsatM p) => new AghsatMBusinessWithEmptyConstructor
							{
								Status = p.Status,
								ContextGuid = ContextGuid,
								DateSarResid = p.DateSarResid,
								DeliveryDate = p.DeliveryDate,
								DeliveryId = p.DeliveryId,
								DeliveryMessage = p.DeliveryMessage,
								DeliveryShortMessage = p.DeliveryShortMessage,
								Description = p.Description,
								Guid = p.Guid,
								HostRefrenced = p.HostRefrenced,
								Modified = p.Modified,
								Price = p.Price,
								ServerStatus = p.ServerStatus,
								TafsilCode = p.Groups.Tafsils.Code.ToString(),
								FullName = p.Groups.Tafsils.Name,
								ReceptionGuid = p.ReceptionGuid,
								ReceptionDateM = p.ReceptionDateM,
								ReceptionSum = p.ReceptionSum,
								AghsatStatus = ((!p.Status) ? EnAghsatMStatus.Removed : (((p.ReceptionGuid ?? Guid.Empty) == Guid.Empty || p.ReceptionSum <= 0m) ? EnAghsatMStatus.NotRecieved : EnAghsatMStatus.Recieved)),
								Date = p.Date,
								GuidGroup = p.GuidGroup,
								Id2 = p.Id2,
								Mounth = p.Mounth,
								TypePardakht = p.TypePardakht,
								CustomerFirstNumber = p.Groups.Tafsils.TafsilNumber.FirstOrDefault().CalcedNumber.ToString(),
								UserName = p.Groups.CashierItem.Name,
								ChangeStatus = EntityChangeSt.Edited,
								VajhEltezam = p.VajhEltezam,
								SanadNo = p.SanadNo,
								CreditMoeinGuid = p.CreditMoeinGuid,
								CreditTafsilGuid = p.CreditTafsilGuid,
								RasGiriType = p.RasGiriType,
								VajhEltezamModified = p.VajhEltezamModified
							});
							obj = ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<AghsatMBusinessWithEmptyConstructor>(queryable, token) : null);
						}
						List<AghsatMBusinessWithEmptyConstructor> list = await (Task<List<AghsatMBusinessWithEmptyConstructor>>)obj;
						if (token.IsCancellationRequested)
						{
							result = null;
						}
						else
						{
							if (filter.Status.HasValue)
							{
								list = list?.Where((AghsatMBusinessWithEmptyConstructor p) => p.AghsatStatus == filter.Status)?.ToList();
							}
							result = list?.OrderBy((AghsatMBusinessWithEmptyConstructor o) => o.DateSarResidSh);
						}
					}
					end_IL_0112:;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 181);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<AghsatMBusiness> GetAsync(int id2, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 192));
		AghsatMBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				AghsatM aghsatM = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<AghsatM>((IQueryable<AghsatM>)c.AghsatM, (Expression<Func<AghsatM, bool>>)((AghsatM p) => p.Id2 == id2), token);
				result = ((aghsatM != null) ? (await MapFromDatabase.MapAsync(ContextGuid, aghsatM, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 203);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(AghsatMBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 219));
				AghsatM aghsatM = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(aghsatM, aghsatM.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 221);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 225);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 226);
			}
			return ret;
		}
	}

	public async Task<int> GetCountByHGuidAsync(Guid tafsilGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetCountByHGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 232));
		int result;
		try
		{
			result = ((IQueryable<AghsatM>)EntityFrameworkQueryableExtensions.Include<AghsatM, AghsatM_Group>((IQueryable<AghsatM>)acc_ModelContext.AghsatM, (Expression<Func<AghsatM, AghsatM_Group>>)((AghsatM q) => q.Groups))).Count((AghsatM q) => q.Groups.TafsilGuid == tafsilGuid && (q.ReceptionGuid == null || q.ReceptionGuid == Guid.Empty));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetCountByHGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs", 241);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetCountByHGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatMPersistanceRepository.cs");
			}
		}
		return result;
	}

	public Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<IEnumerable<AghsatMSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<IEnumerable<AghsatMDelayModel>> GetAllDelayedInstallmentsAsync(AghsatMDelayedConfig config, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<ReturnedSaveFuncInfo> UpdateReceptionInfoAsync(Guid? receptionGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
