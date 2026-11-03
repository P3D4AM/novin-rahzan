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
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

public abstract class ViewCheckShBasePersistenceRepository : PersistenceSyncRepository<ViewCheckShBusiness, ViewCheckSh>, IViewCheckShRepository, ISyncRepository<ViewCheckShBusiness>, IEditableDelivery, IRepository<ViewCheckShBusiness>, INoGuidRepository<ViewCheckShBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ViewCheckShBusiness, ViewCheckSh>, IItemAsyncMapper<ViewCheckSh, ViewCheckShBusiness>
	{
		public Task<ViewCheckSh> MapAsync(string contextGuid, ViewCheckShBusiness source, ViewCheckSh destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ViewCheckSh();
				}
				destination.Guid = source.Guid;
				destination.Number = source.Number ?? "";
				destination.Price = source.Price;
				destination.ChequeStatus = source.ChequeStatus;
				destination.BankName = source.BankName ?? "";
				destination.CustomerFullName = source.CustomerFullName ?? "";
				destination.PardakhtDate = source.PardakhtDate;
				destination.GuidSanad = source.GuidSanad;
				destination.Aval2re = source.Aval2re;
				destination.DateSarresidM = source.DateSarresidM;
				destination.PardakhtDesc = source.PardakhtDesc ?? "";
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.IsEnteghali = source.IsEnteghali;
				destination.SayadiRegistered = source.SayadiRegistered;
				destination.DateUtc = destination.DateSarresidM.ToUniversalTime();
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckShBasePersistenceRepository.cs", 87);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ViewCheckShBusiness> MapAsync(string contextGuid, ViewCheckSh source, ViewCheckShBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ViewCheckShBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Number = source.Number;
				destination.Price = source.Price;
				destination.ChequeStatus = source.ChequeStatus;
				destination.BankName = source.BankName;
				destination.CustomerFullName = source.CustomerFullName;
				destination.PardakhtDate = source.PardakhtDate;
				destination.GuidSanad = source.GuidSanad;
				destination.Aval2re = source.Aval2re;
				destination.DateSarresidM = source.DateSarresidM;
				destination.PardakhtDesc = source.PardakhtDesc;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.IsEnteghali = source.IsEnteghali;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckShBasePersistenceRepository.cs", 130);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ViewCheckSh>> MapListAsync(string contextGuid, IEnumerable<ViewCheckShBusiness> source, List<ViewCheckSh> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ViewCheckShBusiness>> MapListAsync(string contextGuid, IEnumerable<ViewCheckSh> source, List<ViewCheckShBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ViewCheckSh, ViewCheckShBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ViewCheckShBusiness, ViewCheckSh> MapToDatabase => Mapper;

	public override IItemMapper<ViewCheckSh, ViewCheckShBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ViewCheckShBasePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckShBasePersistenceRepository.cs", 37);
		}
	}

	public Task<bool> CheckSHA2re_ISEnteghaliAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> ElamPardakhtAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> EbtalAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> BargashtAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> PardakhtShode2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Bargashty2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, DateTime dateM, int sanadNo, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Ebtal2PardakhtaniAsync(List<ViewCheckShBusiness> checkList, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(Guid chequeGuid, ENCheckSHStatus status, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckShBasePersistenceRepository.cs", 183));
			ViewCheckSh viewCheckSh = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ViewCheckSh>((IQueryable<ViewCheckSh>)c.ViewCheckSh, (Expression<Func<ViewCheckSh, bool>>)((ViewCheckSh q) => q.Guid == chequeGuid), token);
			if (viewCheckSh == null)
			{
				return res;
			}
			viewCheckSh.ChequeStatus = status;
			viewCheckSh.ServerStatus = ServerStatusForClient.None;
			((EntityEntry)((DbContext)c).Entry<ViewCheckSh>(viewCheckSh)).State = (EntityState)3;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckShBasePersistenceRepository.cs", 192);
			res.AddReturnedValue(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ViewCheckShBasePersistenceRepository.cs", 193);
		}
		return res;
	}

	public abstract Task<IEnumerable<ChequeSHSummaryViewModel>> GetSummaryAsync(DateTime? date1, DateTime? date2, CancellationToken token, string tranName);

	public Task<IEnumerable<ViewCheckShBusiness>> GetAllAsync(ENCheckSHStatus? status, DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
