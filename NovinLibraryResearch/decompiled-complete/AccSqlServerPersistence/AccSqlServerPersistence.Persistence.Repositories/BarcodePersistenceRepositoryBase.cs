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
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal abstract class BarcodePersistenceRepositoryBase : PersistenceNoGuidRepository<BarcodeBusiness, Barcode>, IBarcodeRepository, INoGuidRepository<BarcodeBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BarcodeBusiness, Barcode>, IItemAsyncMapper<Barcode, BarcodeBusiness>
	{
		public Task<Barcode> MapAsync(string contextGuid, BarcodeBusiness source, Barcode destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs"))
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
						destination = new Barcode();
					}
					destination.BarCode = source.BarCode;
					destination.PrdGuid = source.PrdGuid;
					destination.CreateDate = source.CreateDate;
					destination.Ordinal = source.Ordinal;
					destination.DepotGuid = source.DepotGuid;
					destination.Description = source.Description ?? "";
					destination.Name = source.Name ?? "";
					destination.Price = source.Price;
					destination.PriceTypeGuid = source.PriceTypeGuid;
					destination.Quantity = source.Quantity;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 230);
					throw;
				}
			}
		}

		public Task<BarcodeBusiness> MapAsync(string contextGuid, Barcode source, BarcodeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs"))
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
						destination = new BarcodeBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.BarCode = source.BarCode;
					destination.PrdGuid = source.PrdGuid;
					destination.CreateDate = source.CreateDate;
					destination.Ordinal = source.Ordinal;
					destination.DepotGuid = source.DepotGuid;
					destination.Description = source.Description ?? "";
					destination.Name = source.Name ?? "";
					destination.Price = source.Price;
					destination.PriceTypeGuid = source.PriceTypeGuid;
					destination.Quantity = source.Quantity;
					destination.Type = source.Type;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 264);
					throw;
				}
			}
		}

		public Task<List<Barcode>> MapListAsync(string contextGuid, IEnumerable<BarcodeBusiness> source, List<Barcode> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BarcodeBusiness>> MapListAsync(string contextGuid, IEnumerable<Barcode> source, List<BarcodeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Barcode, BarcodeBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BarcodeBusiness, Barcode> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public BarcodePersistenceRepositoryBase(ContextHolder context)
		: base(context)
	{
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsyncByPrdGuid(Guid prdGuid, short tryCount, string tranName, CancellationToken token)
	{
		return RemoveAsync(prdGuid, tryCount, tranName, token);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(string barCode, short tryCount, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 40));
				foreach (Barcode item in ((IQueryable<Barcode>)acc_ModelContext.Barcodes)?.Where((Barcode x) => x.BarCode == barCode).ToList())
				{
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 44);
					}
					if (item != null)
					{
						acc_ModelContext.Barcodes.Remove(item);
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 52);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 53);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid prdGuid, short tryCount, string tranName, CancellationToken token)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 66));
				IQueryable<Barcode> queryable = ((IQueryable<Barcode>)acc_ModelContext.Barcodes)?.Where((Barcode x) => x.PrdGuid == prdGuid);
				if (queryable != null && queryable.Any())
				{
					foreach (Barcode item in queryable)
					{
						if (item != null)
						{
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 72);
							}
							acc_ModelContext.Barcodes.Remove(item);
						}
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 79);
				ret.AddReturnedValue(ex2, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 80);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(List<Guid> prdGuid, short tryCount, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 91);
				}
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 92));
				IQueryable<Barcode> queryable = ((IQueryable<Barcode>)dbcontext.Barcodes).Where((Barcode x) => prdGuid.Contains(x.PrdGuid));
				List<Barcode> list = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<Barcode>(queryable, token) : null);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 94);
				}
				if (list == null || !list.Any())
				{
					return ret;
				}
				dbcontext.Barcodes.RemoveRange((IEnumerable<Barcode>)list);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 99);
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 102);
				ret.AddReturnedValue(ex3, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 103);
			}
			return ret;
		}
	}

	public async Task<List<BarcodeBusiness>> GetByPrdGuidAsync(List<Guid> prdGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 110));
			List<BarcodeBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<Barcode> source = ((IQueryable<Barcode>)c.Barcodes)?.Where((Barcode x) => prdGuid.Contains(x.PrdGuid));
					result = ((!token.IsCancellationRequested) ? (await MapFromDatabase.MapListAsync(base.ContextGuid, source, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 121);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs");
				}
			}
			return result;
		}
	}

	public async Task<BarcodeBusiness> GetAsync(string barcode, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 132));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs"))
		{
			BarcodeBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Barcode source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Barcode>((IQueryable<Barcode>)c.Barcodes, (Expression<Func<Barcode, bool>>)((Barcode p) => p.BarCode == barcode), token);
					result = ((!token.IsCancellationRequested) ? (await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 144);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs");
				}
			}
			return result;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 162);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(BarcodeBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 170));
				Barcode barcode = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(barcode, barcode.BarCode), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 172);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 176);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 177);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<BarcodeBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs"))
		{
			try
			{
				foreach (BarcodeBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 188);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 192);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\BarcodePersistenceRepositoryBase.cs", 193);
			}
			return ret;
		}
	}

	public virtual Task<ReturnedSaveFuncInfo> FillBarcodesAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public virtual Task<IEnumerable<BarcodeModel>> GetAllConflictsAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
