using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Global.Tasks;
using EntityCache.Business.Global.Tasks.SarNakh;
using EntityCache.Core;
using EntityCache.Core.Repository.Global.Tasks;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Filters.Global;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global.Tasks;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global.Tasks;

internal abstract class SarNakhPersistenceRepositoryBase : PersistenceRepository<SarNakhBusiness, SarNakh>, ISarNakhRepository, IRepository<SarNakhBusiness>, INoGuidRepository<SarNakhBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<SarNakhBusiness, SarNakh>, IItemAsyncMapper<SarNakh, SarNakhBusiness>
	{
		public Task<SarNakh> MapAsync(string contextGuid, SarNakhBusiness source, SarNakh destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs"))
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
						destination = new SarNakh();
					}
					destination.Guid = source.Guid;
					destination.Company = source.Company;
					destination.FullName = source.FullName;
					destination.CashierGuid = source.CashierGuid;
					destination.Modified = source.Modified;
					destination.Status = source.Status;
					destination.StatusId = source.StatusId;
					destination.Description = source.Description;
					destination.CreationDate = source.CreationDate;
					destination.MetaData = Json.ToStringJson(source.MetaData);
					destination.IntroductionGuid = source.IntroductionGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 189);
					throw;
				}
			}
		}

		public Task<SarNakhBusiness> MapAsync(string contextGuid, SarNakh source, SarNakhBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs"))
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
						destination = new SarNakhBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Company = source.Company;
					destination.FullName = source.FullName;
					destination.CashierGuid = source.CashierGuid;
					destination.Status = source.Status;
					destination.Modified = source.Modified;
					destination.StatusId = source.StatusId;
					destination.Description = source.Description;
					destination.CreationDate = source.CreationDate;
					destination.MetaData = source.MetaData.FromJson<SarNakhMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 214);
					destination.ChangeStatus = EntityChangeSt.Edited;
					destination.IntroductionGuid = source.IntroductionGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 223);
					throw;
				}
			}
		}

		public Task<List<SarNakh>> MapListAsync(string contextGuid, IEnumerable<SarNakhBusiness> source, List<SarNakh> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<SarNakhBusiness>> MapListAsync(string contextGuid, IEnumerable<SarNakh> source, List<SarNakhBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private SarNakhLocationPersistenceRepository _sarNakhLocation;

	private SarNakhNumbersPersistenceRepository _sarNakhNumbers;

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemMapper<SarNakh, SarNakhBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<SarNakh, SarNakhBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<SarNakhBusiness, SarNakh> MapToDatabase => Mapper;

	public SarNakhPersistenceRepositoryBase(ContextHolder db)
		: base(db)
	{
		try
		{
			_sarNakhLocation = new SarNakhLocationPersistenceRepository(db);
			_sarNakhNumbers = new SarNakhNumbersPersistenceRepository(db);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 44);
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
				_sarNakhLocation?.Dispose();
				_sarNakhLocation = null;
				_sarNakhNumbers?.Dispose();
				_sarNakhNumbers = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 64);
		}
	}

	public virtual Task<IEnumerable<SarNakhReportBusiness>> GetAllAsync(SarNakhFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<string>> GetStateListAsync(CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetStateListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 69));
		List<string> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<string?> queryable = ((IQueryable<SarNakhLocation>)c.SarNakhLocations).Select((SarNakhLocation q) => q.State);
				object obj;
				if (queryable == null)
				{
					obj = null;
				}
				else
				{
					IQueryable<string?> queryable2 = queryable.Distinct();
					if (queryable2 == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<string> queryable3 = queryable2.Where((string q) => !string.IsNullOrEmpty(q));
						obj = ((queryable3 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<string>(queryable3, token) : null);
					}
				}
				result = await (Task<List<string>>)obj;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetStateListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 78);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetStateListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public async Task<List<string>> GetCityListAsync(CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetCityListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 89));
		List<string> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<string?> queryable = ((IQueryable<SarNakhLocation>)c.SarNakhLocations).Select((SarNakhLocation q) => q.City);
				object obj;
				if (queryable == null)
				{
					obj = null;
				}
				else
				{
					IQueryable<string?> queryable2 = queryable.Distinct();
					if (queryable2 == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<string> queryable3 = queryable2.Where((string q) => !string.IsNullOrEmpty(q));
						obj = ((queryable3 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<string>(queryable3, token) : null);
					}
				}
				result = await (Task<List<string>>)obj;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetCityListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 98);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetCityListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(SarNakhBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 113));
			if (item.Guid == Guid.Empty)
			{
				item.Guid = Guid.NewGuid();
			}
			SarNakh sarNakh = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(sarNakh, sarNakh.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 116);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _sarNakhLocation.RemoveRangeAsync(item.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 118);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _sarNakhNumbers.RemoveRangeAsync(item.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 119);
			if (res.HasError)
			{
				return res;
			}
			if ((item?.LocationList?.Any()).GetValueOrDefault())
			{
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _sarNakhLocation.SaveRangeAsync(item.LocationList, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 122);
			}
			if ((item?.NumbersList?.Any()).GetValueOrDefault())
			{
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _sarNakhNumbers.SaveRangeAsync(item.NumbersList, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 124);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 128);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 129);
		}
		return res;
	}

	public override async Task<SarNakhBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 135));
		SarNakhBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 3;
			try
			{
				SarNakh sarNakh = await c.SarNakhs.FindAsync(new object[1] { guid });
				if (sarNakh == null)
				{
					result = null;
				}
				else
				{
					SarNakhBusiness mappedData = await MapFromDatabase.MapAsync(ContextGuid, sarNakh, null, tranName, token);
					await _sarNakhLocation.GetAllAsync(mappedData, token, tranName);
					await _sarNakhNumbers.GetAllAsync(mappedData, token, tranName);
					result = mappedData;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs", 149);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\Tasks\\SarNakh\\SarNakhPersistenceRepositoryBase.cs");
			}
		}
		return result;
	}
}
