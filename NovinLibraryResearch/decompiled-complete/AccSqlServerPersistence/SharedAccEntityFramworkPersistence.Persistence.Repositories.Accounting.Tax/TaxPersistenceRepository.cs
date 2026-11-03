using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Tax;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Tax;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Tax;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Tax;

internal class TaxPersistenceRepository : PersistenceSyncRepository<TaxBusiness, TaxClass>, ITaxRepository, ISyncRepository<TaxBusiness>, IEditableDelivery, IRepository<TaxBusiness>, INoGuidRepository<TaxBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TaxBusiness, TaxClass>, IItemAsyncMapper<TaxClass, TaxBusiness>, IItemAsyncMapper<TaxClass, TaxReportBusiness>
	{
		public Task<TaxClass> MapAsync(string contextGuid, TaxBusiness source, TaxClass destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TaxClass();
				}
				destination.Guid = source.Guid;
				destination.Name = source.Name;
				destination.Slug = source.Slug;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 193);
				throw;
			}
		}

		public Task<TaxBusiness> MapAsync(string contextGuid, TaxClass source, TaxBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TaxBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Name = source.Name;
				destination.Slug = source.Slug;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 226);
				throw;
			}
		}

		public Task<TaxReportBusiness> MapAsync(string contextGuid, TaxClass source, TaxReportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new TaxReportBusiness();
				}
				destination.Guid = source.Guid;
				destination.Name = source.Name;
				destination.Slug = source.Slug;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 251);
				throw;
			}
		}

		public Task<List<TaxClass>> MapListAsync(string contextGuid, IEnumerable<TaxBusiness> source, List<TaxClass> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TaxBusiness>> MapListAsync(string contextGuid, IEnumerable<TaxClass> source, List<TaxBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TaxReportBusiness>> MapListAsync(string contextGuid, IEnumerable<TaxClass> source, List<TaxReportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private TaxDetailPersistenceRepository _detRep;

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<TaxClass, TaxBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<TaxClass, TaxBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TaxBusiness, TaxClass> MapToDatabase => Mapper;

	public IItemAsyncMapper<TaxClass, TaxReportBusiness> MapToReport => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public TaxPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRep = new TaxDetailPersistenceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				_detRep?.Dispose();
				_detRep = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 47);
		}
	}

	public async Task<IEnumerable<TaxReportBusiness>> GetAllReportAsync(bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 51));
		IEnumerable<TaxReportBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<TaxClass> queryable = (IQueryable<TaxClass>)c.Tax;
				if (status.HasValue)
				{
					queryable = queryable?.Where((TaxClass q) => q.Status == ((bool?)status).Value);
				}
				result = await MapToReport.MapListAsync(ContextGuid, queryable, null, tranName, token);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 62);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(TaxBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 77));
			TaxClass taxClass = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(taxClass, taxClass.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 79);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await _detRep.RemoveAsync(item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 81);
			if (res.HasError)
			{
				return res;
			}
			if ((item?.DetailList?.Any()).GetValueOrDefault())
			{
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRep.SaveAsync(item.DetailList, item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 84);
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
			WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 90);
			res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 91);
		}
		return res;
	}

	public override async Task<TaxBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 97));
		TaxBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				TaxClass taxClass = await c.Tax.FindAsync(new object[1] { guid });
				if (taxClass == null)
				{
					result = null;
				}
				else
				{
					TaxBusiness mappedData = await MapFromDatabase.MapAsync(ContextGuid, taxClass, null, tranName, token);
					await _detRep.GetAllAsync(mappedData, token, tranName);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 110);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<TaxReportBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 121));
		TaxReportBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				TaxClass taxClass = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TaxClass>((IQueryable<TaxClass>)c.Tax, (Expression<Func<TaxClass, bool>>)((TaxClass q) => q.Name == name.FixString()), default(CancellationToken));
				result = ((taxClass != null) ? (await MapToReport.MapAsync(ContextGuid, taxClass, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 132);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<TaxReportBusiness> GetBySlugAsync(string slug, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetBySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 143));
		TaxReportBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				TaxClass taxClass = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TaxClass>((IQueryable<TaxClass>)c.Tax, (Expression<Func<TaxClass, bool>>)((TaxClass q) => q.Slug == slug.FixString()), default(CancellationToken));
				result = ((taxClass != null) ? (await MapToReport.MapAsync(ContextGuid, taxClass, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetBySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs", 154);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetBySlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Tax\\TaxPersistenceRepository.cs");
			}
		}
		return result;
	}
}
