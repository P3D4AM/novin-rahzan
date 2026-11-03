using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Details;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class PardakhtBankPersistenceRepository : PersistenceMasterRepository<PardakhtBankBusiness, PardakhtBank>, IPardakhtBankRepository, IPersistenceMasterRepository<PardakhtBankBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PardakhtBankBusiness, PardakhtBank>, IItemAsyncMapper<PardakhtBank, PardakhtBankBusiness>
	{
		public async Task<PardakhtBank> MapAsync(string contextGuid, PardakhtBankBusiness source, PardakhtBank destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtBankPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new PardakhtBank();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DateM = source.DateM;
					destination.Description = source.RowDesc ?? "";
					destination.Number = source.Number;
					destination.Price = source.Price;
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.DecimalGuid = source.DecimalGuid;
					destination.MasterGuid = source.MasterGuid;
					PardakhtBank pardakhtBank = destination;
					pardakhtBank.ShortDescription = await source.GetAndroidPrintTextAsync(EnSystem.Mapper, token, tranName);
					destination.MetaData = Json.ToStringJson(source.MetaData);
					destination.Ordinal = source.Ordinal;
					destination.BankRealAccount = source.BankRealAccount;
					destination.WageGuid = source.WageGuid;
					destination.WageTafsilGuid = source.WageTafsilGuid;
					destination.WageMoeinGuid = source.WageMoeinGuid;
					destination.WagePrice = source.WagePrice;
					destination.DateUtc = destination.DateM.ToUniversalTime();
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtBankPersistenceRepository.cs", 102);
					throw;
				}
				return destination;
			}
		}

		public Task<PardakhtBankBusiness> MapAsync(string contextGuid, PardakhtBank source, PardakhtBankBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtBankPersistenceRepository.cs"))
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
						destination = new PardakhtBankBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DateM = source.DateM;
					destination.RowDesc = source.Description;
					destination.Number = source.Number;
					destination.SetPrice(EnSystem.Mapper, source.Price);
					destination.TafsilGuid = source.TafsilGuid;
					destination.MoeinGuid = source.MoeinGuid;
					destination.DecimalGuid = source.DecimalGuid;
					destination.BankRealAccount = source.BankRealAccount;
					destination.MasterGuid = source.MasterGuid;
					destination.MetaData = source.MetaData.FromJson<PardakhtHavaleMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtBankPersistenceRepository.cs", 130);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtBankPersistenceRepository.cs", 142);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<PardakhtBank>> MapListAsync(string contextGuid, IEnumerable<PardakhtBankBusiness> source, List<PardakhtBank> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PardakhtBankBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtBank> source, List<PardakhtBankBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<PardakhtBank, PardakhtBankBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PardakhtBankBusiness, PardakhtBank> MapToDatabase => Mapper;

	public PardakhtBankPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtBankPersistenceRepository.cs", 36);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtBankBusiness> item, Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtBankPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				List<PardakhtBankBusiness> list = await GetAllAsync(masterGuid, token, tranName);
				if (list != null && list.Any())
				{
					foreach (PardakhtBankBusiness exist in list)
					{
						item.FirstOrDefault((PardakhtBankBusiness q) => q.Guid == exist.Guid);
					}
				}
				return await base.SaveAsync(item, masterGuid, tryCount, token, tranName);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtBankPersistenceRepository.cs", 60);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtBankPersistenceRepository.cs", 61);
			}
			return ret;
		}
	}
}
