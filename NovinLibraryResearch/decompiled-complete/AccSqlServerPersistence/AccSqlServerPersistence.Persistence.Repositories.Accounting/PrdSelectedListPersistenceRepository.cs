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
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class PrdSelectedListPersistenceRepository : PersistenceRepository<PrdSelectedBusiness, PrdSelected>, IPrdSelectedListRepository, IRepository<PrdSelectedBusiness>, INoGuidRepository<PrdSelectedBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdSelectedBusiness, PrdSelected>, IItemAsyncMapper<PrdSelected, PrdSelectedBusiness>
	{
		public Task<PrdSelected> MapAsync(string contextGuid, PrdSelectedBusiness source, PrdSelected destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdSelected();
				}
				destination.Guid = source.Guid;
				destination.Code = source.Code;
				destination.Title = source.Title;
				destination.Type = source.Type;
				destination.UserGuid = source.UserGuid;
				destination.Description = source.Description;
				destination.Modified = source.Modified;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 228);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<PrdSelectedBusiness> MapAsync(string contextGuid, PrdSelected source, PrdSelectedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdSelectedBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Code = source.Code;
				destination.Title = source.Title;
				destination.Type = source.Type;
				destination.UserGuid = source.UserGuid;
				destination.Description = source.Description;
				destination.Modified = source.Modified;
				destination.ChangeStatus = EntityChangeSt.Edited;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 258);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<PrdSelected>> MapListAsync(string contextGuid, IEnumerable<PrdSelectedBusiness> source, List<PrdSelected> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdSelectedBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdSelected> source, List<PrdSelectedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private PrdSelectedDetListPersistenceRepository _Prd_DetailsRepo;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<PrdSelected, PrdSelectedBusiness> MapFromDatabase => new mapper();

	public override IItemAsyncMapper<PrdSelectedBusiness, PrdSelected> MapToDatabase => new mapper();

	public override IItemMapper<PrdSelected, PrdSelectedBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public Task<int> DbCountAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public PrdSelectedListPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_Prd_DetailsRepo = new PrdSelectedDetListPersistenceRepository(db);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAllAsync(short tryCount, CancellationToken token, string tranName = null)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 45));
				PrdSelected prdSelected = ((IQueryable<PrdSelected>)acc_ModelContext.ListPrds).FirstOrDefault((PrdSelected w) => w.Guid == item);
				if (prdSelected == null)
				{
					return ret;
				}
				acc_ModelContext.ListPrds.Remove(prdSelected);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await _Prd_DetailsRepo.RemoveAsync(item, tryCount, token, tranName);
				if (returnedSaveFuncInfo.HasError)
				{
					return returnedSaveFuncInfo;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 54);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 55);
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<PrdSelectedBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 70));
			PrdSelectedBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					PrdSelected prdSelected = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PrdSelected>((IQueryable<PrdSelected>)c.ListPrds, (Expression<Func<PrdSelected, bool>>)((PrdSelected x) => x.Guid == guid), token);
					if (prdSelected == null)
					{
						result = null;
					}
					else
					{
						PrdSelectedBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, prdSelected, null, "", token);
						ret?.AddRangeToDetList(await _Prd_DetailsRepo.GetAllAsync(ret.Guid, token, tranName));
						result = ret;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 84);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(PrdSelectedBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				PrdSelected res = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				Acc_ModelContext obj = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 103));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj.AddOrUpdateAsync(res, res.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 104);
				ReturnedSaveFuncInfo result = await _Prd_DetailsRepo.RemoveAsync(item.Guid, tryCount, token, tranName);
				returnedSaveFuncInfo = result;
				returnedSaveFuncInfo.AddReturnedValue(await _Prd_DetailsRepo.SaveAsync(item.DetList?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 106);
				if (result.HasError)
				{
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 111);
				return new ReturnedSaveFuncInfo(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 112);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PrdSelectedBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (PrdSelectedBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 126);
					if (ret.HasError)
					{
						return ret;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 132);
				return new ReturnedSaveFuncInfo(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 133);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount)
	{
		throw new NotImplementedException();
	}

	public Task<List<PrdSelectedBusiness>> GetAllNotSentAsync(CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<PrdSelectedBusiness>> GetAllAsync(string title, string description, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 149));
			IEnumerable<PrdSelectedBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					description = (description ?? "").FixString();
					IQueryable<PrdSelected> queryable = ((IQueryable<PrdSelected>)c.ListPrds).Where((PrdSelected e) => (title == null || e.Title.Contains(title)) && (description == "" || e.Description.Contains(description)));
					List<PrdSelected> source = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<PrdSelected>(queryable, token) : null);
					List<PrdSelectedBusiness> business = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
					if (business != null && business.Any())
					{
						foreach (PrdSelectedBusiness prdSelected in business)
						{
							List<PrdSelectedDetListBusiness> list = await _Prd_DetailsRepo.GetAllAsync(prdSelected.Guid, token, tranName);
							if (list != null && list.Any())
							{
								prdSelected.AddRangeToDetList(list);
							}
						}
					}
					result = business;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 171);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_Prd_DetailsRepo?.Dispose();
				_Prd_DetailsRepo = null;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Product\\PrdSelectedListPersistenceRepository.cs", 193);
		}
	}
}
