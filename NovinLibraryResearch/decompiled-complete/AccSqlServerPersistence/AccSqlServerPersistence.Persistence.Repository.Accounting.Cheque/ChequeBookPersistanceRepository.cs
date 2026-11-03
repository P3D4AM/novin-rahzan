using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Cheque;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repository.Accounting.Cheque;

internal class ChequeBookPersistanceRepository : PersistenceRepository<ChequeBookBusiness, ChequeBook>, IChequeBookRepository, IRepository<ChequeBookBusiness>, INoGuidRepository<ChequeBookBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ChequeBookBusiness, ChequeBook>, IItemAsyncMapper<ChequeBook, ChequeBookBusiness>
	{
		public Task<ChequeBook> MapAsync(string contextGuid, ChequeBookBusiness source, ChequeBook destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
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
						destination = new ChequeBook();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.SerialNumber = source.SerialNumber;
					destination.BankGuid = source.BankGuid;
					destination.IsSaderShode = source.IsSaderShode;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 233);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<ChequeBookBusiness> MapAsync(string contextGuid, ChequeBook source, ChequeBookBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
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
						destination = new ChequeBookBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.SerialNumber = source.SerialNumber;
					destination.BankGuid = source.BankGuid;
					destination.IsSaderShode = source.IsSaderShode;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 261);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<ChequeBook>> MapListAsync(string contextGuid, IEnumerable<ChequeBookBusiness> source, List<ChequeBook> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ChequeBookBusiness>> MapListAsync(string contextGuid, IEnumerable<ChequeBook> source, List<ChequeBookBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ChequeBook, ChequeBookBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ChequeBookBusiness, ChequeBook> MapToDatabase => Mapper;

	public override IItemMapper<ChequeBook, ChequeBookBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal ChequeBookPersistanceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 41);
		}
	}

	public async Task<List<ChequeBookBusiness>> GetAllAsync(Guid? bankGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 45));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
		{
			List<ChequeBookBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<ChequeBook> queryable = (IQueryable<ChequeBook>)c.ChequeBook;
					if (bankGuid.HasValue && bankGuid != Guid.Empty)
					{
						queryable = queryable.Where((ChequeBook x) => x.BankGuid == bankGuid);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable?.ToList(), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 58);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ChequeBookBusiness> GetAsync(string serialNumber, Guid bankGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 68));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
		{
			ChequeBookBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					ChequeBook source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ChequeBook>((IQueryable<ChequeBook>)c.ChequeBook, (Expression<Func<ChequeBook, bool>>)((ChequeBook p) => p.SerialNumber == serialNumber && p.BankGuid == bankGuid), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 79);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAllAsync(Guid? bankGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 95));
				acc_ModelContext.ChequeBook.RemoveRange((IEnumerable<ChequeBook>)((IQueryable<ChequeBook>)acc_ModelContext.ChequeBook).Where((ChequeBook q) => q.BankGuid == null || q.BankGuid == Guid.Empty || q.BankGuid == bankGuid));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 100);
				ret.AddReturnedValue(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 101);
			}
			return ret;
		}
	}

	public async Task<string> NextSerialAsync(Guid bankGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 107));
		using (new JaegerMonitor("NextSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
		{
			string result;
			try
			{
				string value = ((IQueryable<ChequeBook>)acc_ModelContext.ChequeBook).Where((ChequeBook p) => p.BankGuid == bankGuid && !p.IsSaderShode)?.Min((ChequeBook p) => p.SerialNumber);
				if (value.ParseToInt() == 0)
				{
					result = "1";
				}
				else
				{
					value = value.ParseToInt().ToString();
					result = value;
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
				WebErrorLog.Instance.StartLog(ex2, "", "NextSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 120);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("NextSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ChequeBookBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
		{
			try
			{
				if (items != null && items.Any())
				{
					foreach (ChequeBookBusiness item in items)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 136);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 140);
				res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 141);
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ChequeBookBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 153));
				ChequeBook item2 = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(item2, item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 155);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 159);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 160);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 172));
				ChequeBook chequeBook = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ChequeBook>((IQueryable<ChequeBook>)dbcontext.ChequeBook, (Expression<Func<ChequeBook, bool>>)((ChequeBook q) => q.Guid == guid), token);
				if (chequeBook == null)
				{
					return ret;
				}
				dbcontext.ChequeBook.Remove(chequeBook);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 179);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 180);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(long number1, long number2, Guid bankGuid, bool isSadershode, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 192));
				for (long i = number1; i <= number2; i++)
				{
					ChequeBook chequeBook = new ChequeBook();
					chequeBook.SerialNumber = i.ToString();
					chequeBook.IsSaderShode = false;
					chequeBook.BankGuid = bankGuid;
					if (chequeBook.Guid == Guid.Empty)
					{
						chequeBook.Guid = Guid.NewGuid();
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(chequeBook, chequeBook.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 201);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 206);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Cheque\\ChequeBookPersistanceRepository.cs", 207);
			}
			return ret;
		}
	}
}
