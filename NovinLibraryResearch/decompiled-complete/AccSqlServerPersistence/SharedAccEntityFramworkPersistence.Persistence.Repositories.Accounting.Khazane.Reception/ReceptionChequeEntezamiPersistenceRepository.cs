using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Khazane.Reception;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Khazane.Reception;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Khazane.Reception;

internal class ReceptionChequeEntezamiPersistenceRepository : PersistenceNoGuidRepository<ReceptionChequeEntezamiBusiness, ReceptionChequeEntezami>, IReceptionChequeEntezamiRepository, INoGuidRepository<ReceptionChequeEntezamiBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ReceptionChequeEntezamiBusiness, ReceptionChequeEntezami>, IItemAsyncMapper<ReceptionChequeEntezami, ReceptionChequeEntezamiBusiness>
	{
		public Task<ReceptionChequeEntezami> MapAsync(string contextGuid, ReceptionChequeEntezamiBusiness source, ReceptionChequeEntezami destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceptionChequeEntezami();
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
				destination.ReceptionDateM = source.ReceptionDateM;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs", 111);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ReceptionChequeEntezamiBusiness> MapAsync(string contextGuid, ReceptionChequeEntezami source, ReceptionChequeEntezamiBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceptionChequeEntezamiBusiness(contextGuid);
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
				destination.ReceptionDateM = source.ReceptionDateM;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs", 142);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ReceptionChequeEntezami>> MapListAsync(string contextGuid, IEnumerable<ReceptionChequeEntezamiBusiness> source, List<ReceptionChequeEntezami> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReceptionChequeEntezamiBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionChequeEntezami> source, List<ReceptionChequeEntezamiBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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


	public override IItemAsyncMapper<ReceptionChequeEntezami, ReceptionChequeEntezamiBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReceptionChequeEntezamiBusiness, ReceptionChequeEntezami> MapToDatabase => Mapper;

	public ReceptionChequeEntezamiPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs", 39);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ReceptionChequeEntezamiBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs", 49));
				ReceptionChequeEntezami receptionChequeEntezami = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(receptionChequeEntezami, receptionChequeEntezami.ChequeGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs", 51);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs", 55);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs", 56);
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ReceptionChequeEntezamiBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReceptionChequeEntezamiBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs", 63));
		ReceptionChequeEntezamiBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				ReceptionChequeEntezami receptionChequeEntezami = await c.ReceptionChequeEntezami.FindAsync(new object[1] { guid });
				result = ((receptionChequeEntezami != null) ? (await MapFromDatabase.MapAsync(base.ContextGuid, receptionChequeEntezami, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs", 75);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiPersistenceRepository.cs");
			}
		}
		return result;
	}
}
