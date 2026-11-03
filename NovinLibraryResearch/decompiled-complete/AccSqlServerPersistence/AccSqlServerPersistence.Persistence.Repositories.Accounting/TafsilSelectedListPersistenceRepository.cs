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
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Customer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class TafsilSelectedListPersistenceRepository : PersistenceRepository<TafsilSelectedBusiness, TafsilSelected>, ITafsilSelectedListRepository, IRepository<TafsilSelectedBusiness>, INoGuidRepository<TafsilSelectedBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TafsilSelectedBusiness, TafsilSelected>, IItemAsyncMapper<TafsilSelected, TafsilSelectedBusiness>
	{
		public Task<TafsilSelected> MapAsync(string contextGuid, TafsilSelectedBusiness source, TafsilSelected destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TafsilSelected();
				}
				destination.Guid = source.Guid;
				destination.Code = source.Code;
				destination.Title = source.Title;
				destination.Type = source.Type;
				destination.UGuid = source.UGuid;
				destination.Description = source.Description;
				destination.Modified = source.Modified;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 227);
				throw;
			}
		}

		public Task<TafsilSelectedBusiness> MapAsync(string contextGuid, TafsilSelected source, TafsilSelectedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TafsilSelectedBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Code = source.Code;
				destination.Title = source.Title;
				destination.Type = source.Type;
				destination.UGuid = source.UGuid;
				destination.Description = source.Description;
				destination.Modified = source.Modified;
				destination.ChangeStatus = EntityChangeSt.Edited;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 256);
				throw;
			}
		}

		public Task<List<TafsilSelected>> MapListAsync(string contextGuid, IEnumerable<TafsilSelectedBusiness> source, List<TafsilSelected> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TafsilSelectedBusiness>> MapListAsync(string contextGuid, IEnumerable<TafsilSelected> source, List<TafsilSelectedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private TafsilSelectedDetListPersistenceRepository _Customer_DetailsRepo;

	public override IItemAsyncMapper<TafsilSelected, TafsilSelectedBusiness> MapFromDatabase => new mapper();

	public override IItemAsyncMapper<TafsilSelectedBusiness, TafsilSelected> MapToDatabase => new mapper();

	public override IItemMapper<TafsilSelected, TafsilSelectedBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => false;

	public TafsilSelectedListPersistenceRepository(ContextHolder db)
		: base(db)
	{
		try
		{
			dbContext = db;
			_Customer_DetailsRepo = new TafsilSelectedDetListPersistenceRepository(db);
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
			WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 45);
			throw;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 55));
			try
			{
				TafsilSelected tafsilSelected = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TafsilSelected>((IQueryable<TafsilSelected>)c.ListCustomers, (Expression<Func<TafsilSelected, bool>>)((TafsilSelected w) => w.Guid == item), token);
				if (tafsilSelected == null)
				{
					return ret;
				}
				c.ListCustomers.Remove(tafsilSelected);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _Customer_DetailsRepo.RemoveAsync(item, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 62);
				if (ret.HasError)
				{
					return ret;
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
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 69);
				return new ReturnedSaveFuncInfo(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 70);
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

	public override async Task<TafsilSelectedBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 80));
			TafsilSelectedBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					TafsilSelected tafsilSelected = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TafsilSelected>((IQueryable<TafsilSelected>)c.ListCustomers, (Expression<Func<TafsilSelected, bool>>)((TafsilSelected x) => x.Guid == guid), token);
					if (tafsilSelected == null)
					{
						result = null;
					}
					else
					{
						TafsilSelectedBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, tafsilSelected, null, tranName, token);
						if (ret != null)
						{
							TafsilSelectedBusiness tafsilSelectedBusiness = ret;
							await tafsilSelectedBusiness.AddRangeToDetListAsync(await _Customer_DetailsRepo.GetAllAsync(ret.Guid, token, tranName));
						}
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 95);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TafsilSelectedBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (item.Guid == Guid.Empty)
				{
					item.Guid = Guid.NewGuid();
				}
				if (string.IsNullOrEmpty(item.Description))
				{
					item.Description = "";
				}
				TafsilSelected res = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				Acc_ModelContext obj = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 114));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj.AddOrUpdateAsync(res, res.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 115);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _Customer_DetailsRepo.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 116);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _Customer_DetailsRepo.SaveAsync(item.DetList.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 117);
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 123);
				return new ReturnedSaveFuncInfo(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 124);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<TafsilSelectedBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (TafsilSelectedBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 138);
					if (ret.HasError)
					{
						return ret;
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
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 146);
				return new ReturnedSaveFuncInfo(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 147);
			}
			return ret;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_Customer_DetailsRepo?.Dispose();
				_Customer_DetailsRepo = null;
				dbContext?.Dispose();
				dbContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 163);
		}
	}

	public async Task<IEnumerable<TafsilSelectedBusiness>> GetAllAsync(TafsilSelectedFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 168));
			IEnumerable<TafsilSelectedBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					IQueryable<TafsilSelected> queryable = (IQueryable<TafsilSelected>)c.ListCustomers;
					if (filter != null && (filter?.TafsilGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((TafsilSelected q) => q.TafsilSelectedDetails.Select((TafsilSelectedDetails q) => q.HGuid).Contains(filter.TafsilGuid.Value));
					}
					List<TafsilSelectedBusiness> businessList = await MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token);
					if (businessList?.Any() ?? false)
					{
						foreach (TafsilSelectedBusiness cusSelected in businessList)
						{
							if (cusSelected != null)
							{
								List<TafsilSelectedDetListBusiness> list = await _Customer_DetailsRepo.GetAllAsync(cusSelected.Guid, token, tranName);
								if (list != null && list.Any())
								{
									await cusSelected.AddRangeToDetListAsync(list);
								}
							}
						}
					}
					result = businessList;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs", 192);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Selection\\Tafsil\\TafsilSelectedListPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
