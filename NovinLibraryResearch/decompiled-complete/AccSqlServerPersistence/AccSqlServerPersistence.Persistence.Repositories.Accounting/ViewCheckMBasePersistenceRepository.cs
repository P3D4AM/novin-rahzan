using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters.Configs;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

public abstract class ViewCheckMBasePersistenceRepository : PersistenceSyncRepository<ViewCheckMBusiness, ViewCheckM>, IViewCheckMRepository, ISyncRepository<ViewCheckMBusiness>, IEditableDelivery, IRepository<ViewCheckMBusiness>, INoGuidRepository<ViewCheckMBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ViewCheckMBusiness, ViewCheckM>, IItemAsyncMapper<ViewCheckM, ViewCheckMBusiness>
	{
		public Task<ViewCheckM> MapAsync(string contextGuid, ViewCheckMBusiness source, ViewCheckM destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ViewCheckM();
				}
				destination.Guid = source.Guid;
				destination.BankName = source.BankName ?? "";
				destination.Price = source.Price;
				destination.Status = source.Status;
				destination.DateSanadM = source.DateSanadM;
				destination.DateCheckM = source.DateCheckM;
				destination.DateBargashtORVosolM = source.DateBargashtORVosolM;
				destination.DateToPlace = source.DateToPlace ?? "";
				destination.Place = source.Place ?? "";
				destination.Description = source.Description ?? "";
				destination.Number = source.Number ?? "";
				destination.IsEnteghali = source.IsEnteghali;
				destination.GuidSanad = source.GuidSanad;
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.Personal = source.Personal;
				destination.HesabNum = source.HesabNum ?? "";
				destination.PardazandeTafsilGuid = source.PardazandeTafsilGuid;
				destination.Aval2re = source.Aval2re;
				destination.PoshtNomreh = source.PoshtNomreh;
				destination.VisitorGuid = source.VisitorGuid;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.Sayadi_Name = source.Sayadi_Name ?? "";
				destination.Sayadi_Identity = source.Sayadi_Identity ?? "";
				destination.Sayadi_NationalCode = source.Sayadi_NationalCode ?? "";
				destination.SayadiRegistered = source.SayadiRegistered;
				destination.DateUtc = destination.DateCheckM.ToUniversalTime();
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs", 267);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ViewCheckMBusiness> MapAsync(string contextGuid, ViewCheckM source, ViewCheckMBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ViewCheckMBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.BankName = source.BankName;
				destination.Price = source.Price;
				destination.Status = source.Status;
				destination.DateSanadM = source.DateSanadM;
				destination.DateCheckM = source.DateCheckM;
				destination.DateBargashtORVosolM = source.DateBargashtORVosolM;
				destination.DateToPlace = source.DateToPlace;
				destination.Place = source.Place;
				destination.Description = source.Description;
				destination.Number = source.Number;
				destination.IsEnteghali = source.IsEnteghali;
				destination.GuidSanad = source.GuidSanad;
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.Personal = source.Personal;
				destination.HesabNum = source.HesabNum;
				destination.Aval2re = source.Aval2re;
				destination.PoshtNomreh = source.PoshtNomreh;
				destination.VisitorGuid = source.VisitorGuid;
				destination.Modified = source.Modified;
				destination.PardazandeTafsilGuid = source.PardazandeTafsilGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.Sayadi_Name = source.Sayadi_Name ?? "";
				destination.Sayadi_Identity = source.Sayadi_Identity ?? "";
				destination.Sayadi_NationalCode = source.Sayadi_NationalCode;
				destination.SayadiRegistered = source.SayadiRegistered;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs", 322);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ViewCheckM>> MapListAsync(string contextGuid, IEnumerable<ViewCheckMBusiness> source, List<ViewCheckM> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ViewCheckMBusiness>> MapListAsync(string contextGuid, IEnumerable<ViewCheckM> source, List<ViewCheckMBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ViewCheckM, ViewCheckMBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ViewCheckMBusiness, ViewCheckM> MapToDatabase => Mapper;

	public override IItemMapper<ViewCheckM, ViewCheckMBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ViewCheckMBasePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs", 42);
		}
	}

	public async Task<IEnumerable<ViewCheckMBusiness>> GetAllAsync(ENUMCheck? status, Guid? pardazandeGuid, DateTime? dateSarresid1, DateTime? dateSarresid2, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs", 48));
			IEnumerable<ViewCheckMBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<ViewCheckM> source = (IQueryable<ViewCheckM>)c.ViewCheckM;
					if (status.HasValue)
					{
						source = source.Where((ViewCheckM q) => (int?)q.Status == (int?)status);
					}
					if (pardazandeGuid.HasValue)
					{
						source = source.Where((ViewCheckM q) => q.PardazandeTafsilGuid == pardazandeGuid);
					}
					if (dateSarresid1.HasValue)
					{
						source = source.Where((ViewCheckM q) => q.DateCheckM >= dateSarresid1);
					}
					if (dateSarresid2.HasValue)
					{
						source = source.Where((ViewCheckM q) => q.DateCheckM <= dateSarresid2);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs", 63);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<bool> CheckMA2re_ISEnteghaliAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> VagozarKardanAsync(List<ViewCheckMBusiness> checkList, Guid guid, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> BargashtZadanAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> NaghdKardanAsync(List<ViewCheckMBusiness> checkList, Guid guid, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> EbtalAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> VosolKardanAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> OdatBargashtiAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> Bargashty2GhablAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> Vagozar2MojoodAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> Naghd2MojoodAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Batel2MojoodAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> Vosol2VagozarAsync(List<ViewCheckMBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> ElamTahvilAsync(List<ViewCheckMBusiness> checkList, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public new Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public new Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public new Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<Guid> GetBankVagozarGuidAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(ViewChequeMForceUpdateConfig config, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs", 158));
			ViewCheckM viewCheckM = await EntityFrameworkQueryableExtensions.SingleAsync<ViewCheckM>((IQueryable<ViewCheckM>)c.ViewCheckM, (Expression<Func<ViewCheckM, bool>>)((ViewCheckM q) => q.Guid == config.ChequeGuid), token);
			viewCheckM.Status = config.Status;
			viewCheckM.Modified = DateTime.Now;
			if (!string.IsNullOrEmpty(config.Place))
			{
				viewCheckM.Place = config.Place;
			}
			if (config.DateToPlace.HasValue)
			{
				viewCheckM.DateToPlace = config.DateToPlace.MiladiToCalture();
			}
			if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
			{
				viewCheckM.ServerStatus = ServerStatusForClient.None;
			}
			((EntityEntry)((DbContext)c).Entry<ViewCheckM>(viewCheckM)).State = (EntityState)3;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs", 170);
			res.AddReturnedValue(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckMBasePersistenceRepository.cs", 171);
		}
		return res;
	}

	public abstract Task<IEnumerable<ChequeMSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName);
}
