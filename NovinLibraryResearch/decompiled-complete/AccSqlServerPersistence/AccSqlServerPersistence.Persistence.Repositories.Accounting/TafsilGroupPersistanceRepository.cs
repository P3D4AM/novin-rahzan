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
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.Entities.SerializeData;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class TafsilGroupPersistanceRepository : PersistenceSyncRepository<TafsilGroupBusiness, TafsilGroup>, ITafsilGroupRepository, ISyncRepository<TafsilGroupBusiness>, IEditableDelivery, IRepository<TafsilGroupBusiness>, INoGuidRepository<TafsilGroupBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TafsilGroupBusiness, TafsilGroup>, IItemAsyncMapper<TafsilGroup, TafsilGroupBusiness>, IItemMapper<TafsilGroup, TafsilGroupBusiness>, IItemAsyncMapper<WebTafsilGroup, TafsilGroup>
	{
		public async Task<TafsilGroup> MapAsync(string contextGuid, TafsilGroupBusiness source, TafsilGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TafsilGroup();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Tbl = source.Tbl;
				destination.Description = source.Description;
				destination.CodeMin = source.CodeMin;
				destination.CodeMax = source.CodeMax;
				destination.Status = source.Status;
				TafsilGroup tafsilGroup = destination;
				tafsilGroup.MoeinGuid = await source.GetMoeinGuidAsync(token, tranName);
				destination.MoeinSellGuid = source.MoeinSellGuid;
				destination.MoeinBuyGuid = source.MoeinBuyGuid;
				destination.MoeinBackSellGuid = source.MoeinBackSellGuid;
				destination.MoeinBackBuyGuid = source.MoeinBackBuyGuid;
				destination.MoeinReceptionGuid = source.MoeinReceptionGuid;
				destination.MoeinPardakhtGuid = source.MoeinPardakhtGuid;
				destination.MoeinAmaniMaNazdDigaranGuid = source.MoeinAmaniMaNazdDigaranGuid;
				destination.MoeinAmaniDigaranNazdMaGuid = source.MoeinAmaniDigaranNazdMaGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.BazaryabDefaultGuidData = Json.ToStringJson(source.BazaryabDefaultGuid);
				destination.Type = source.Type;
				if (source.MetaData == null)
				{
					source.MetaData = new TafsilGroupMetaData
					{
						Level = (EnTafsilLevel)15
					};
				}
				destination.MetaData = Json.ToStringJson(source.MetaData);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 242);
				throw;
			}
		}

		public Task<TafsilGroupBusiness> MapAsync(string contextGuid, TafsilGroup source, TafsilGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public TafsilGroupBusiness Map(string contextGuid, TafsilGroup source, TafsilGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TafsilGroupBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Tbl = source.Tbl;
				destination.Description = source.Description;
				destination.CodeMin = source.CodeMin;
				destination.CodeMax = source.CodeMax;
				destination.Status = source.Status;
				destination.SetMoeinGuid(source.MoeinGuid);
				destination.MoeinSellGuid = source.MoeinSellGuid;
				destination.MoeinBuyGuid = source.MoeinBuyGuid;
				destination.MoeinBackSellGuid = source.MoeinBackSellGuid;
				destination.MoeinBackBuyGuid = source.MoeinBackBuyGuid;
				destination.MoeinReceptionGuid = source.MoeinReceptionGuid;
				destination.MoeinPardakhtGuid = source.MoeinPardakhtGuid;
				destination.MoeinAmaniMaNazdDigaranGuid = source.MoeinAmaniMaNazdDigaranGuid;
				destination.MoeinAmaniDigaranNazdMaGuid = source.MoeinAmaniDigaranNazdMaGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.ChangeStatus = EntityChangeSt.Edited;
				destination.BazaryabDefaultGuid = source.BazaryabDefaultGuidData.FromJson<DefualtBazaryabGuidInTafsilGroup>("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 283);
				destination.Type = source.Type;
				if (!string.IsNullOrEmpty(source.MetaData))
				{
					destination.MetaData = source.MetaData.FromJson<TafsilGroupMetaData>("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 286);
				}
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
				WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 293);
				throw;
			}
		}

		public Task<List<TafsilGroup>> MapListAsync(string contextGuid, IEnumerable<TafsilGroupBusiness> source, List<TafsilGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TafsilGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<TafsilGroup> source, List<TafsilGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<TafsilGroup> MapAsync(string contextGuid, WebTafsilGroup source, TafsilGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TafsilGroup();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Tbl = source.Tbl;
				destination.Description = source.Description;
				destination.CodeMin = source.CodeMin;
				destination.CodeMax = source.CodeMax;
				destination.Status = source.Status;
				destination.MoeinGuid = source.MoeinGuid;
				destination.MoeinSellGuid = source.MoeinSellGuid;
				destination.MoeinBuyGuid = source.MoeinBuyGuid;
				destination.MoeinBackSellGuid = source.MoeinBackSellGuid;
				destination.MoeinBackBuyGuid = source.MoeinBackBuyGuid;
				destination.MoeinReceptionGuid = source.MoeinReceptionGuid;
				destination.MoeinPardakhtGuid = source.MoeinPardakhtGuid;
				destination.MoeinAmaniMaNazdDigaranGuid = source.MoeinAmaniMaNazdDigaranGuid;
				destination.MoeinAmaniDigaranNazdMaGuid = source.MoeinAmaniDigaranNazdMaGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.BazaryabDefaultGuidData = Json.ToStringJson(source.BazaryabDefaultGuid);
				destination.Type = source.Type;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 342);
				throw;
			}
		}

		public Task<List<TafsilGroup>> MapListAsync(string contextGuid, IEnumerable<WebTafsilGroup> source, List<TafsilGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TafsilGroup, TafsilGroupBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TafsilGroupBusiness, TafsilGroup> MapToDatabase => Mapper;

	public override IItemMapper<TafsilGroup, TafsilGroupBusiness> MapFromDatabaseNoneAsync => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public TafsilGroupPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<TafsilGroupBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 37));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs"))
		{
			List<TafsilGroupBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<string> list = search?.FixString()?.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<TafsilGroup> source = (IQueryable<TafsilGroup>)c.TafsilLevel;
						foreach (string word in list)
						{
							if (!string.IsNullOrEmpty(word))
							{
								source = source.Where((TafsilGroup p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.Name, $"%{word}%"));
							}
						}
						if (status.HasValue)
						{
							source = source.Where((TafsilGroup p) => p.Status == ((bool?)status).Value);
						}
						result = ((!token.IsCancellationRequested) ? (await MapFromDatabase.MapListAsync(ContextGuid, source.ToList(), null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 58);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<TafsilGroupBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 69));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs"))
		{
			TafsilGroupBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					DbSet<TafsilGroup> obj = ((DbContext)c).Set<TafsilGroup>();
					TafsilGroup tafsilGroup = await ((obj != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TafsilGroup>((IQueryable<TafsilGroup>)obj, (Expression<Func<TafsilGroup, bool>>)((TafsilGroup x) => x.Name == name), token) : null);
					result = ((tafsilGroup != null) ? (await MapFromDatabase.MapAsync(ContextGuid, tafsilGroup, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 82);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<TafsilGroupBusiness>>> IsDuplicateCodeAsync(long codeMin, long codeMax, Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 93));
		using (new JaegerMonitor("IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfoWithValue<List<TafsilGroupBusiness>> ret = new ReturnedSaveFuncInfoWithValue<List<TafsilGroupBusiness>>();
			ReturnedSaveFuncInfoWithValue<List<TafsilGroupBusiness>> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					if (codeMin == 0L && codeMax == 0L)
					{
						result = ret;
						goto IL_0439;
					}
					List<TafsilGroup> list = await EntityFrameworkQueryableExtensions.ToListAsync<TafsilGroup>((IQueryable<TafsilGroup>)((DbContext)c).Set<TafsilGroup>(), token);
					if (list == null || !list.Any())
					{
						result = ret;
						goto IL_0439;
					}
					foreach (TafsilGroup item in list)
					{
						if (((codeMin >= item.CodeMin && codeMin <= item.CodeMax) || (codeMax >= item.CodeMin && codeMax <= item.CodeMax) || (codeMin <= item.CodeMin && codeMax >= item.CodeMax)) && item.Guid != guid)
						{
							ReturnedSaveFuncInfoWithValue<List<TafsilGroupBusiness>> returnedSaveFuncInfoWithValue = ret;
							if (returnedSaveFuncInfoWithValue.value == null)
							{
								returnedSaveFuncInfoWithValue.value = new List<TafsilGroupBusiness>();
							}
							List<TafsilGroupBusiness> value = ret.value;
							value.Add(await MapFromDatabase.MapAsync(ContextGuid, item, null, tranName, token));
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
					WebErrorLog.Instance.StartLog(ex2, "", "IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 114);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("IsDuplicateCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs");
				}
			}
			return ret;
			IL_0439:
			return result;
		}
	}

	public async Task<long> NextMinCodeAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextMinCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 126));
		using (new JaegerMonitor("NextMinCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs"))
		{
			long result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<TafsilGroup> queryable = ((IQueryable<TafsilGroup>)c.TafsilLevel).OrderByDescending((TafsilGroup x) => x.CodeMax).Take(1);
					TafsilGroup tafsilGroup = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TafsilGroup>(queryable, token) : null);
					result = ((tafsilGroup != null) ? (tafsilGroup.CodeMax + 1) : 1);
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
					WebErrorLog.Instance.StartLog(ex2, "", "NextMinCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 138);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextMinCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TafsilGroupBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 154));
				TafsilGroup item2 = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item2, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 156);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 160);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 161);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 174);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebTafsilGroup> items, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 183));
				List<TafsilGroup> list = await Mapper.MapListAsync(ContextGuid, items, null, tranName, token);
				if (list == null || !list.Any())
				{
					return ret;
				}
				foreach (TafsilGroup item in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, token, item.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 187);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 191);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Coding\\TafsilGroupPersistanceRepository.cs", 192);
			}
			return ret;
		}
	}
}
