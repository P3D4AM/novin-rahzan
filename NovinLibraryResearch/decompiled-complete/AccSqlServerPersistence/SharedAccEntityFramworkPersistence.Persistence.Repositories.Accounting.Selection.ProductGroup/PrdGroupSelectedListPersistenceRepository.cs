using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Selection.ProductGroup;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Selection.ProductGroup;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Selection.ProductGroup;

internal class PrdGroupSelectedListPersistenceRepository : PersistenceRepository<PrdGroupSelectedListBusiness, ProductGroupSelected>, IPrdGroupSelectedListRepository, IRepository<PrdGroupSelectedListBusiness>, INoGuidRepository<PrdGroupSelectedListBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdGroupSelectedListBusiness, ProductGroupSelected>, IItemAsyncMapper<ProductGroupSelected, PrdGroupSelectedListBusiness>
	{
		public Task<ProductGroupSelected> MapAsync(string contextGuid, PrdGroupSelectedListBusiness source, ProductGroupSelected destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductGroupSelected();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Code = source.Code ?? "";
				destination.Title = source.Title;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description ?? "";
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 195);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<PrdGroupSelectedListBusiness> MapAsync(string contextGuid, ProductGroupSelected source, PrdGroupSelectedListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PrdGroupSelectedListBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Code = source.Code;
				destination.Title = source.Title;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 223);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ProductGroupSelected>> MapListAsync(string contextGuid, IEnumerable<PrdGroupSelectedListBusiness> source, List<ProductGroupSelected> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdGroupSelectedListBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductGroupSelected> source, List<PrdGroupSelectedListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private PrdGroupSelectedDetListPersistenceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<ProductGroupSelected, PrdGroupSelectedListBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<ProductGroupSelected, PrdGroupSelectedListBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdGroupSelectedListBusiness, ProductGroupSelected> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public PrdGroupSelectedListPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new PrdGroupSelectedDetListPersistenceRepository(db);
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 41));
				ProductGroupSelected productGroupSelected = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ProductGroupSelected>((IQueryable<ProductGroupSelected>)c.ProductGroupSelected, (Expression<Func<ProductGroupSelected, bool>>)((ProductGroupSelected w) => w.Guid == item), token);
				if (productGroupSelected == null)
				{
					return ret;
				}
				c.ProductGroupSelected.Remove(productGroupSelected);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await _detRepo.RemoveAsync(item, tryCount, token, tranName);
				if (returnedSaveFuncInfo.HasError)
				{
					return returnedSaveFuncInfo;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 50);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 51);
			}
			return ret;
		}
	}

	public override async Task<PrdGroupSelectedListBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 58));
			PrdGroupSelectedListBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					ProductGroupSelected productGroupSelected = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ProductGroupSelected>((IQueryable<ProductGroupSelected>)c.ProductGroupSelected, (Expression<Func<ProductGroupSelected, bool>>)((ProductGroupSelected x) => x.Guid == guid), token);
					if (productGroupSelected == null)
					{
						result = null;
					}
					else
					{
						PrdGroupSelectedListBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, productGroupSelected, null, tranName, token);
						ret?.AddRangeToDetList(await _detRepo.GetAllAsync(ret.Guid, token, tranName));
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 72);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(PrdGroupSelectedListBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				ProductGroupSelected res = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				Acc_ModelContext obj = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 91));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj.AddOrUpdateAsync(res, res.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 92);
				ReturnedSaveFuncInfo result = await _detRepo.RemoveAsync(item.Guid, tryCount, token, tranName);
				returnedSaveFuncInfo = result;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetList?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 94);
				if (result.HasError)
				{
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 99);
				return new ReturnedSaveFuncInfo(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 100);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PrdGroupSelectedListBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (PrdGroupSelectedListBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 114);
					if (ret.HasError)
					{
						return ret;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 120);
				return new ReturnedSaveFuncInfo(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 121);
			}
			return ret;
		}
	}

	public override async Task<IEnumerable<PrdGroupSelectedListBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 129));
			IEnumerable<PrdGroupSelectedListBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<ProductGroupSelected> productGroupSelected = (IQueryable<ProductGroupSelected>)c.ProductGroupSelected;
					List<PrdGroupSelectedListBusiness> business = await MapFromDatabase.MapListAsync(ContextGuid, productGroupSelected, null, tranName, token);
					if (business != null && business.Any())
					{
						foreach (PrdGroupSelectedListBusiness prdSelected in business)
						{
							List<PrdGroupSelectedDetListBusiness> list = await _detRepo.GetAllAsync(prdSelected.Guid, token, tranName);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 148);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs");
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
				_detRepo?.Dispose();
				_detRepo = null;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedListPersistenceRepository.cs", 168);
		}
	}
}
