using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using Core.Repository;
using EntityCache.Business;
using EntityCache.Core;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal abstract class PardakhtCheckShMasterPersistanceRepository : PersistenceMasterRepository<PardakhtCheckShBusiness, PardakhtCheckSh>, IPardakhtCheckShRepository, IPersistenceMasterRepository<PardakhtCheckShBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PardakhtCheckShBusiness, PardakhtCheckSh>, IItemAsyncMapper<PardakhtCheckSh, PardakhtCheckShBusiness>
	{
		public async Task<PardakhtCheckSh> MapAsync(string contextGuid, PardakhtCheckShBusiness source, PardakhtCheckSh destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMasterPersistanceRepository.cs"))
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
						destination = new PardakhtCheckSh();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DateSaresid = source.DateSaresid;
					destination.Description = source.RowDesc ?? "";
					destination.Number = source.Number;
					destination.Price = source.Price;
					destination.Status = source.Status;
					destination.BankTafsilGuid = source.BankTafsilGuid;
					destination.CreditMoeinGuid = source.CreditMoeinGuid;
					destination.CreditTafsilGuid = source.CreditTafsilGuid;
					destination.MasterGuid = source.MasterGuid;
					destination.UserGuid = source.UserGuid;
					destination.IsEnteghali = source.IsEnteghali;
					destination.PardakhtDate = source.PardakhtDate;
					destination.Aval2re = source.Aval2re;
					destination.A2re_TafsilGuid = source.A2re_TafsilGuid;
					destination.A2re_MoeinGuid = source.A2re_MoeinGuid;
					PardakhtCheckSh pardakhtCheckSh = destination;
					pardakhtCheckSh.ShortDescription = await source.GetShortDescriptionAsync(EnSystem.Mapper, token, tranName);
					destination.ServerStatus = source.ServerStatus;
					destination.DebitA2reMoeinGuid = source.DebitA2reMoeinGuid;
					destination.DebitA2reTafsiGuid = source.DebitA2reTafsiGuid;
					destination.SanadNo = source.SanadNo;
					destination.SayadiName = source.SayadiName;
					destination.SayadiIdentity = source.SayadiIdentity;
					destination.SayadiNationalCode = source.SayadiNationalCode;
					destination.MetaData = source.MetaData?.ToString() ?? "";
					destination.SayadiRegistered = source.SayadiRegistered;
					destination.Ordinal = source.Ordinal;
					destination.DateUtc = destination.DateSaresid.ToUniversalTime();
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMasterPersistanceRepository.cs", 114);
					throw;
				}
				return destination;
			}
		}

		public Task<PardakhtCheckShBusiness> MapAsync(string contextGuid, PardakhtCheckSh source, PardakhtCheckShBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMasterPersistanceRepository.cs"))
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
						destination = new PardakhtCheckShBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.DateSaresid = source.DateSaresid;
					destination.RowDesc = source.Description;
					destination.Number = source.Number;
					destination.SetPrice(EnSystem.Mapper, source.Price);
					destination.Status = source.Status;
					destination.BankTafsilGuid = source.BankTafsilGuid;
					destination.CreditMoeinGuid = source.CreditMoeinGuid;
					destination.CreditTafsilGuid = source.CreditTafsilGuid;
					destination.MasterGuid = source.MasterGuid;
					destination.UserGuid = source.UserGuid;
					destination.IsEnteghali = source.IsEnteghali;
					destination.PardakhtDate = source.PardakhtDate;
					destination.Aval2re = source.Aval2re;
					destination.A2re_TafsilGuid = source.A2re_TafsilGuid;
					destination.A2re_MoeinGuid = source.A2re_MoeinGuid;
					destination.ServerStatus = source.ServerStatus;
					destination.DebitA2reMoeinGuid = source.DebitA2reMoeinGuid;
					destination.DebitA2reTafsiGuid = source.DebitA2reTafsiGuid;
					destination.SanadNo = source.SanadNo;
					destination.SayadiName = source.SayadiName;
					destination.SayadiIdentity = source.SayadiIdentity;
					destination.SayadiNationalCode = source.SayadiNationalCode;
					destination.MetaData = source.MetaData.FromJson<PardakhtChequeShMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMasterPersistanceRepository.cs", 155);
					destination.SayadiRegistered = source.SayadiRegistered;
					destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
					destination.ChangeStatus = EntityChangeSt.Edited;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMasterPersistanceRepository.cs", 165);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<PardakhtCheckSh>> MapListAsync(string contextGuid, IEnumerable<PardakhtCheckShBusiness> source, List<PardakhtCheckSh> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PardakhtCheckShBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtCheckSh> source, List<PardakhtCheckShBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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


	public override IItemAsyncMapper<PardakhtCheckSh, PardakhtCheckShBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PardakhtCheckShBusiness, PardakhtCheckSh> MapToDatabase => Mapper;

	public PardakhtCheckShMasterPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public abstract Task<PardakhtCheckShBusiness> GetAsync(Guid guid, CancellationToken token, string tranName);

	public abstract Task<IEnumerable<PardakhtCheckShReportBusiness>> GetAllAsync(PardakhtChequeFilter filter, CancellationToken token, string tranName);

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMasterPersistanceRepository.cs", 40);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtCheckShBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (items?.Any() ?? false)
			{
				foreach (PardakhtCheckShBusiness item in items)
				{
					PardakhtCheckShBusiness pardakhtCheckShBusiness = await GetAsync(item.Guid, token, tranName);
					if (pardakhtCheckShBusiness != null && pardakhtCheckShBusiness.MetaData != null && item.MetaData == null)
					{
						throw new Exception("metaData for aghsatM is removed!");
					}
					item.ServerStatus = ServerStatusForClient.None;
					item.Modified = DateTime.Now;
				}
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.SaveAsync(items, masterGuid, trycount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMasterPersistanceRepository.cs", 55);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMasterPersistanceRepository.cs", 59);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShMasterPersistanceRepository.cs", 60);
		}
		return res;
	}

	public abstract Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName);
}
