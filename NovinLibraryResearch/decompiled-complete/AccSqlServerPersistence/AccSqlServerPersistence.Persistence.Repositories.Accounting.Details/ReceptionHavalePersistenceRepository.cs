using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Details;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Details;

internal class ReceptionHavalePersistenceRepository : PersistenceMasterRepository<ReceptionHavaleBusiness, ReceptionHavale>, IReceptionHavaleRepository, IPersistenceMasterRepository<ReceptionHavaleBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ReceptionHavaleBusiness, ReceptionHavale>, IItemAsyncMapper<ReceptionHavale, ReceptionHavaleBusiness>
	{
		public Task<ReceptionHavale> MapAsync(string contextGuid, ReceptionHavaleBusiness source, ReceptionHavale destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceptionHavale();
				}
				destination.Guid = source.Guid;
				destination.Date = source.Date;
				destination.MasterGuid = source.MasterGuid;
				destination.Description = source.Description ?? "";
				destination.TafsilGuid = source.TafsilGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.Number = source.Number ?? "";
				destination.Price = source.Price;
				destination.Type = source.HavaleType;
				destination.TerminalId = source.TerminalId;
				destination.PAN = source.PAN;
				destination.STAN = source.STAN;
				destination.RRN = source.RRN;
				destination.MerchantId = source.MerchantId;
				destination.MessageId = source.MessageId;
				destination.DateTasviye = source.DateTasviye;
				destination.Qt = source.Qt;
				destination.DecimalGuid = source.DecimalGuid;
				destination.BankRealAccount = source.BankRealAccount;
				destination.Modified = DateTime.Now;
				if (destination.DateTasviye < DateTime.Now.AddYears(-20))
				{
					destination.DateTasviye = DateTime.Now;
				}
				destination.MetaData = Json.ToStringJson(source.MetaData);
				destination.Ordinal = source.Ordinal;
				destination.WageGuid = source.WageGuid;
				destination.WageTafsilGuid = source.WageTafsilGuid;
				destination.WageMoeinGuid = source.WageMoeinGuid;
				destination.WagePrice = source.WagePrice;
				destination.DateUtc = destination.Date.ToUniversalTime();
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 156);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ReceptionHavaleBusiness> MapAsync(string contextGuid, ReceptionHavale source, ReceptionHavaleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ReceptionHavaleBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Date = source.Date;
				destination.MasterGuid = source.MasterGuid;
				destination.Description = source.Description;
				destination.TafsilGuid = source.TafsilGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.Number = source.Number;
				destination.SetPrice(EnSystem.Mapper, source.Price);
				destination.HavaleType = source.Type;
				destination.TerminalId = source.TerminalId;
				destination.PAN = source.PAN;
				destination.STAN = source.STAN;
				destination.RRN = source.RRN;
				destination.MerchantId = source.MerchantId;
				destination.MessageId = source.MessageId;
				destination.DateTasviye = source.DateTasviye;
				destination.Qt = source.Qt;
				destination.BankRealAccount = source.BankRealAccount;
				destination.DecimalGuid = source.DecimalGuid;
				destination.MetaData = source.MetaData.FromJson<ReceptionHavaleMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 192);
				destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
				destination.WageGuid = source.WageGuid;
				destination.WageTafsilGuid = source.WageTafsilGuid;
				destination.WageMoeinGuid = source.WageMoeinGuid;
				destination.WagePrice = source.WagePrice;
				destination.DateUtc = source.DateUtc;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 204);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ReceptionHavale>> MapListAsync(string contextGuid, IEnumerable<ReceptionHavaleBusiness> source, List<ReceptionHavale> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReceptionHavaleBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionHavale> source, List<ReceptionHavaleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<ReceptionHavale, ReceptionHavaleBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReceptionHavaleBusiness, ReceptionHavale> MapToDatabase => Mapper;

	public ReceptionHavalePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<bool> IsPeygiriNumberTekrariAsync(string number, Guid guid, Guid bankGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "IsPeygiriNumberTekrariAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 30));
		bool result;
		try
		{
			_ = 1;
			try
			{
				result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ReceptionHavale>((IQueryable<ReceptionHavale>)c.ReceptionHavale, (Expression<Func<ReceptionHavale, bool>>)((ReceptionHavale p) => p.STAN == number && p.TafsilGuid == bankGuid && p.Guid != guid), token) != null;
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
				WebErrorLog.Instance.StartLog(ex2, "", "IsPeygiriNumberTekrariAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 40);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("IsPeygiriNumberTekrariAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs");
			}
		}
		return result;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 58);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<ReceptionHavaleBusiness> item, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				List<ReceptionHavaleBusiness> list = await GetAllAsync(masterGuid, token, tranName);
				if (list != null && list.Any())
				{
					foreach (ReceptionHavaleBusiness exist in list)
					{
						item.FirstOrDefault((ReceptionHavaleBusiness q) => q.Guid == exist.Guid);
					}
				}
				return await base.SaveAsync(item, masterGuid, tryCount, token, tranName);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 83);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 84);
			}
			return ret;
		}
	}

	public async Task<List<Guid>> GetAllMasterGuidsForFixWageAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllMasterGuidsForFixWageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 90));
		List<Guid> result;
		try
		{
			_ = 1;
			try
			{
				List<ReceptionHavale> list = await EntityFrameworkQueryableExtensions.ToListAsync<ReceptionHavale>(((IQueryable<ReceptionHavale>)c.ReceptionHavale).Where((ReceptionHavale q) => (q.WageGuid ?? Guid.Empty) != Guid.Empty || q.WagePrice > 0m), token);
				result = ((list != null && list.Any()) ? list.Select((ReceptionHavale q) => q.MasterGuid).ToList() : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllMasterGuidsForFixWageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs", 101);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllMasterGuidsForFixWageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionHavalePersistenceRepository.cs");
			}
		}
		return result;
	}
}
