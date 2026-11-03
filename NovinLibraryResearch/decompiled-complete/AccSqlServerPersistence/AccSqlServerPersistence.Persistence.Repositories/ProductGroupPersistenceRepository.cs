using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class ProductGroupPersistenceRepository : PersistenceSyncRepository<PrdGroupBusiness, ProductGroup>, IProductGroupRepository, ISyncRepository<PrdGroupBusiness>, IEditableDelivery, IRepository<PrdGroupBusiness>, INoGuidRepository<PrdGroupBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdGroupBusiness, ProductGroup>, IItemAsyncMapper<ProductGroup, PrdGroupBusiness>, IItemMapper<ProductGroup, PrdGroupBusiness>
	{
		public Task<ProductGroup> MapAsync(string contextGuid, PrdGroupBusiness source, ProductGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductGroup();
				}
				destination.Level = source.Level;
				destination.Code = source.Code;
				destination.Description = source.Description;
				destination.FullCode = source.FullCode;
				destination.HtmlFull = source.HtmlFull;
				destination.HtmlQuick = source.HtmlQuick;
				destination.Name = source.Name;
				destination.Pic = source.Pic;
				destination.Special = source.Special;
				destination.Status = source.Status;
				destination.TechInfoData = source.TechInfoData;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ParentGuid = source.ParentGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.Slug = source.Slug;
				destination.Type = source.Type;
				destination.ShortName = source.ShortName ?? "";
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 346);
				throw;
			}
		}

		public Task<PrdGroupBusiness> MapAsync(string contextGuid, ProductGroup source, PrdGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public PrdGroupBusiness Map(string contextGuid, ProductGroup source, PrdGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs"))
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
						destination = new PrdGroupBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Level = source.Level;
					destination.Code = source.Code;
					destination.Description = source.Description;
					destination.FullCode = source.FullCode;
					destination.HtmlFull = source.HtmlFull;
					destination.HtmlQuick = source.HtmlQuick;
					destination.Name = source.Name;
					destination.Pic = source.Pic;
					destination.Special = source.Special;
					destination.Status = source.Status;
					destination.TechInfoData = source.TechInfoData;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ParentGuid = source.ParentGuid;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.Slug = source.Slug;
					destination.ChangeStatus = EntityChangeSt.Edited;
					destination.Type = source.Type;
					destination.ShortName = source.ShortName;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 394);
					throw;
				}
			}
		}

		public Task<List<ProductGroup>> MapListAsync(string contextGuid, IEnumerable<PrdGroupBusiness> source, List<ProductGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductGroup> source, List<PrdGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<ProductGroup, PrdGroupBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdGroupBusiness, ProductGroup> MapToDatabase => Mapper;

	public override IItemMapper<ProductGroup, PrdGroupBusiness> MapFromDatabaseNoneAsync => Mapper;

	public new bool IsDisposed { get; private set; }

	public ProductGroupPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<PrdGroupBusiness>> GetListByGrpNameAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetListByGrpNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 33));
		using (new JaegerMonitor("GetListByGrpNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs"))
		{
			IEnumerable<PrdGroupBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					DbSet<ProductGroup> obj = ((DbContext)c).Set<ProductGroup>();
					List<ProductGroup> source = await ((obj != null) ? EntityFrameworkQueryableExtensions.ToListAsync<ProductGroup>(((IQueryable<ProductGroup>)obj).Where((ProductGroup x) => x.Name == name), token) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetListByGrpNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 44);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetListByGrpNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<PrdGroupBusiness> GetAsync(long fullCode, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 55));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs"))
		{
			PrdGroupBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					ProductGroup source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ProductGroup>((IQueryable<ProductGroup>)((DbContext)c).Set<ProductGroup>(), (Expression<Func<ProductGroup, bool>>)((ProductGroup x) => x.FullCode == fullCode), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 66);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<PrdGroupBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 77));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs"))
		{
			PrdGroupBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					ProductGroup source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ProductGroup>((IQueryable<ProductGroup>)((DbContext)c).Set<ProductGroup>(), (Expression<Func<ProductGroup, bool>>)((ProductGroup x) => x.Name == name), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 88);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<long> GetNextCodeAsync(Guid parent, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 99));
		using (new JaegerMonitor("GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs"))
		{
			long result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					List<ProductGroup> list;
					if (parent == Guid.Empty)
					{
						DbSet<ProductGroup> obj = ((DbContext)c).Set<ProductGroup>();
						list = await ((obj != null) ? EntityFrameworkQueryableExtensions.ToListAsync<ProductGroup>((IQueryable<ProductGroup>)obj, token) : null);
					}
					else
					{
						DbSet<ProductGroup> obj2 = ((DbContext)c).Set<ProductGroup>();
						object obj3;
						if (obj2 == null)
						{
							obj3 = null;
						}
						else
						{
							IQueryable<ProductGroup> queryable = ((IQueryable<ProductGroup>)obj2).Where((ProductGroup x) => x.Guid == parent);
							obj3 = ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<ProductGroup>(queryable, token) : null);
						}
						list = await (Task<List<ProductGroup>>)obj3;
					}
					List<ProductGroup> temp = list;
					DbSet<ProductGroup> obj4 = ((DbContext)c).Set<ProductGroup>();
					object obj5;
					if (obj4 == null)
					{
						obj5 = null;
					}
					else
					{
						IQueryable<long> queryable2 = ((IQueryable<ProductGroup>)obj4).Select((ProductGroup q) => q.Code);
						obj5 = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<long>(queryable2, token) : null);
					}
					List<long> obj6 = await (Task<List<long>>)obj5;
					long num2 = 99L;
					if (temp != null && temp.Count > 0)
					{
						num2 = temp.Max((ProductGroup m) => m.Code);
					}
					if (obj6.Contains(num2))
					{
						num2 = (await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ProductGroup>(((IQueryable<ProductGroup>)c.Products_group).OrderByDescending((ProductGroup x) => x.Code).Take(1), token)).Code;
					}
					result = num2 + 1;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 120);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<PrdGroupBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 131));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs"))
		{
			IEnumerable<PrdGroupBusiness> result;
			try
			{
				int num;
				_ = num - 1;
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
						IQueryable<ProductGroup> queryable = (IQueryable<ProductGroup>)c.Products_group;
						foreach (string word in list)
						{
							if (!string.IsNullOrEmpty(word))
							{
								queryable = queryable.Where((ProductGroup p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.Name, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.FullCode.ToString(), $"%{word}%"));
							}
						}
						if (status.HasValue)
						{
							queryable = queryable.Where((ProductGroup p) => p.Status == ((bool?)status).Value);
						}
						if (token.IsCancellationRequested)
						{
							result = null;
						}
						else if (queryable == null || !queryable.Any())
						{
							result = null;
						}
						else
						{
							IItemAsyncMapper<ProductGroup, PrdGroupBusiness> mapFromDatabase = MapFromDatabase;
							string contextGuid = ContextGuid;
							result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<ProductGroup>(queryable, token), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 154);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(PrdGroupBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				line = 1;
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 173));
				line = 2;
				PrdGroupBusiness old = await GetAsync(item.Name, token, tranName);
				line = 3;
				if (old != null && !old.Special && item.Special)
				{
					line = 4;
					await SetSubGroupSpecialAsync(item.Guid, dbcontext, token);
				}
				if (old != null && old.Guid != item.Guid && old.Status != item.Status)
				{
					line = 5;
					old.Status = item.Status;
					line = 6;
					DbSet<ProductGroup> products_group = dbcontext.Products_group;
					products_group.Update(await MapToDatabase.MapAsync(ContextGuid, old, null, tranName, token));
					line = 7;
					return ret;
				}
				line = 8;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				Acc_ModelContext acc_ModelContext = dbcontext;
				returnedSaveFuncInfo.AddReturnedValue(await acc_ModelContext.AddOrUpdateAsync(await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token), item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 193);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 197);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 198);
			}
			return ret;
		}
	}

	private async Task SetSubGroupSpecialAsync(Guid guid, Acc_ModelContext context, CancellationToken token)
	{
		try
		{
			List<ProductGroup> list = await EntityFrameworkQueryableExtensions.ToListAsync<ProductGroup>(((IQueryable<ProductGroup>)context.Products_group).Where((ProductGroup p) => p.ParentGuid == guid), token);
			if (list == null || !list.Any())
			{
				return;
			}
			foreach (ProductGroup item in list)
			{
				if (!item.Special)
				{
					item.Special = true;
					item.ServerStatus = ServerStatusForClient.None;
					item.Modified = DateTime.Now;
					((DbContext)context).Update<ProductGroup>(item);
					await SetSubGroupSpecialAsync(item.Guid, context, token);
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
			WebErrorLog.Instance.StartLog(ex2, "", "SetSubGroupSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 222);
			throw;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PrdGroupBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs"))
		{
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (PrdGroupBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 234);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 238);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 239);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> HasImageAsync(string fileName, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 246));
		try
		{
			using (new JaegerMonitor("HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs"))
			{
				ret.value = ((IQueryable<ProductGroup>)acc_ModelContext.Products_group).Any((ProductGroup w) => w.Pic == fileName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "filename:" + fileName, "HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 254);
			ret.AddReturnedValue(ex, "", "HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 255);
			ret.value = false;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("HasImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs");
			}
		}
		return ret;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 274);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<PrdGroupBusiness>> GetChildrenAsync(Guid parentGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("داداش از کش استفاده کن".GetTranslate());
	}

	public async Task<ReturnedSaveFuncInfo> UpdateSubGroupSpecialAsync(Guid masterGuid, bool special, string technicalInfo, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateSubGroupSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 283));
			IQueryable<ProductGroup> queryable = ((IQueryable<ProductGroup>)c.Products_group).Where((ProductGroup p) => p.ParentGuid == masterGuid);
			if (queryable != null && queryable.Any())
			{
				foreach (ProductGroup group in queryable)
				{
					group.Special = special;
					group.ServerStatus = ServerStatusForClient.None;
					group.Modified = DateTime.Now;
					group.TechInfoData = technicalInfo;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(group, group.Guid), ReturnedState.None, "UpdateSubGroupSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 292);
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await UpdateSubGroupSpecialAsync(group.Guid, special, technicalInfo, token, tranName), ReturnedState.None, "UpdateSubGroupSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 293);
				}
			}
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "UpdateSubGroupSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 296);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "UpdateSubGroupSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 299);
			res.AddReturnedValue(ex2, "", "UpdateSubGroupSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductGroupPersistenceRepository.cs", 300);
		}
		return res;
	}
}
