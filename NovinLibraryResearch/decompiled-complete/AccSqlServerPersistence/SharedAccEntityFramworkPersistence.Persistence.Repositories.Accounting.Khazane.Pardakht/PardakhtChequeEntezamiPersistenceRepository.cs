using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Khazane.Pardakht;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Khazane.Pardakht;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Pardakht;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Khazane.Pardakht;

internal class PardakhtChequeEntezamiPersistenceRepository : PersistenceNoGuidRepository<PardakhtChequeEntezamiBusiness, PardakhtChequeEntezami>, IPardakhtChequeEntezamiRepository, INoGuidRepository<PardakhtChequeEntezamiBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PardakhtChequeEntezamiBusiness, PardakhtChequeEntezami>, IItemAsyncMapper<PardakhtChequeEntezami, PardakhtChequeEntezamiBusiness>
	{
		public Task<PardakhtChequeEntezami> MapAsync(string contextGuid, PardakhtChequeEntezamiBusiness source, PardakhtChequeEntezami destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PardakhtChequeEntezami();
				}
				destination.Modified = source.Modified;
				destination.ChequeGuid = source.ChequeGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
				destination.DebitMoeinGuid = source.DebitMoeinGuid;
				destination.DebitTafsilGuid = source.DebitTafsilGuid;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
				destination.SanadNo = source.SanadNo;
				destination.PardakhtDateM = source.PardakhtDateM;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs", 112);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<PardakhtChequeEntezamiBusiness> MapAsync(string contextGuid, PardakhtChequeEntezami source, PardakhtChequeEntezamiBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PardakhtChequeEntezamiBusiness(contextGuid);
				}
				destination.Modified = source.Modified;
				destination.ChequeGuid = source.ChequeGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
				destination.DebitMoeinGuid = source.DebitMoeinGuid;
				destination.DebitTafsilGuid = source.DebitTafsilGuid;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
				destination.SanadNo = source.SanadNo;
				destination.PardakhtDateM = source.PardakhtDateM;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs", 143);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<PardakhtChequeEntezami>> MapListAsync(string contextGuid, IEnumerable<PardakhtChequeEntezamiBusiness> source, List<PardakhtChequeEntezami> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PardakhtChequeEntezamiBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtChequeEntezami> source, List<PardakhtChequeEntezamiBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<PardakhtChequeEntezami, PardakhtChequeEntezamiBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PardakhtChequeEntezamiBusiness, PardakhtChequeEntezami> MapToDatabase => Mapper;

	public PardakhtChequeEntezamiPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs", 39);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(PardakhtChequeEntezamiBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs", 49));
				PardakhtChequeEntezami pardakhtChequeEntezami = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(pardakhtChequeEntezami, pardakhtChequeEntezami.ChequeGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs", 51);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs", 55);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs", 56);
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PardakhtChequeEntezamiBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<PardakhtChequeEntezamiBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs", 63));
		PardakhtChequeEntezamiBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				PardakhtChequeEntezami pardakhtChequeEntezami = await c.PardakhtChequeEntezami.FindAsync(new object[1] { guid });
				result = ((pardakhtChequeEntezami != null) ? (await MapFromDatabase.MapAsync(base.ContextGuid, pardakhtChequeEntezami, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs", 75);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiPersistenceRepository.cs");
			}
		}
		return result;
	}
}
