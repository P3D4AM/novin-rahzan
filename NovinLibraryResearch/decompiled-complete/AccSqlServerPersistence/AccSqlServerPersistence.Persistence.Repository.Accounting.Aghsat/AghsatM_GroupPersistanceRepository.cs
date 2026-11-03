using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Aghsat;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Aghsat;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repository.Accounting.Aghsat;

internal class AghsatM_GroupPersistanceRepository : PersistenceSyncRepository<AghsatM_GroupBusiness, AghsatM_Group>, IAghsatM_GroupRepository, ISyncRepository<AghsatM_GroupBusiness>, IEditableDelivery, IRepository<AghsatM_GroupBusiness>, INoGuidRepository<AghsatM_GroupBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AghsatM_GroupBusiness, AghsatM_Group>, IItemAsyncMapper<AghsatM_Group, AghsatM_GroupBusiness>
	{
		public async Task<AghsatM_Group> MapAsync(string contextGuid, AghsatM_GroupBusiness source, AghsatM_Group destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			_ = 2;
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				if (destination == null)
				{
					destination = new AghsatM_Group();
				}
				destination.Guid = source.Guid;
				destination.Sum = source.Sum;
				destination.Count = source.Count;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.TafsilGuid = source.TafsilGuid;
				AghsatM_Group aghsatM_Group = destination;
				aghsatM_Group.MoeinGuid = await source.GetMoeinGuidAsync(EnSystem.Mapper, token, tranName);
				destination.CashierGuid = source.CashierGuid;
				destination.SanadNo = source.SanadNo;
				destination.ProfitSum = source.ProfitSum;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				aghsatM_Group = destination;
				aghsatM_Group.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync(token, tranName);
				aghsatM_Group = destination;
				aghsatM_Group.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync(token, tranName);
				return destination;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 147);
				throw;
			}
		}

		public Task<AghsatM_GroupBusiness> MapAsync(string contextGuid, AghsatM_Group source, AghsatM_GroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AghsatM_GroupBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Sum = source.Sum;
				destination.Count = source.Count;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.TafsilGuid = source.TafsilGuid;
				destination.MoeinGuid = source.MoeinGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.SanadNo = source.SanadNo;
				destination.ProfitSum = source.ProfitSum;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.CreditMoeinGuid = source.CreditMoeinGuid;
				destination.CreditTafsilGuid = source.CreditTafsilGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 186);
				throw;
			}
		}

		public Task<List<AghsatM_Group>> MapListAsync(string contextGuid, IEnumerable<AghsatM_GroupBusiness> source, List<AghsatM_Group> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AghsatM_GroupBusiness>> MapListAsync(string contextGuid, IEnumerable<AghsatM_Group> source, List<AghsatM_GroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AghsatM_Group, AghsatM_GroupBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AghsatM_GroupBusiness, AghsatM_Group> MapToDatabase => Mapper;

	public override IItemMapper<AghsatM_Group, AghsatM_GroupBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public AghsatM_GroupPersistanceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 40);
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateSumAsync(Guid guid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs"))
		{
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 49));
				AghsatM_Group item = await c.AghsatM_Group.FindAsync(new object[1] { guid });
				if (item != null)
				{
					List<AghsatM> list = await EntityFrameworkQueryableExtensions.ToListAsync<AghsatM>(((IQueryable<AghsatM>)c.AghsatM).Where((AghsatM q) => q.GuidGroup == guid), token);
					item.Sum = Math.Round(list?.Sum((AghsatM q) => q.Price) ?? 0m);
					item.Count = list?.Count() ?? 0;
					if (list != null && !list.Any())
					{
						item.Status = false;
					}
					((DbContext)c).Update<AghsatM_Group>(item);
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
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 64);
				ret.AddReturnedValue(ex2, "", "UpdateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 65);
			}
			return ret;
		}
	}

	public override async Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 71));
		List<Guid> result;
		try
		{
			_ = 1;
			try
			{
				List<AghsatM_Group> list = await EntityFrameworkQueryableExtensions.ToListAsync<AghsatM_Group>(((IQueryable<AghsatM_Group>)c.AghsatM_Group).Where((AghsatM_Group x) => x.ServerStatus == ServerStatusForClient.None || x.ServerStatus == ServerStatusForClient.SendError), token);
				result = ((list.Count <= 0) ? null : list.Select((AghsatM_Group q) => q.Guid).ToList());
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 81);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 97));
				AghsatM_Group aghsatM_Group = await dbcontext.AghsatM_Group.FindAsync(new object[1] { guid });
				aghsatM_Group.SanadNo = sanadNo;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(aghsatM_Group, guid), ReturnedState.None, "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 100);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 105);
				ret.AddReturnedValue(ex2, "", "SetSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Aghsat\\AghsatM_GroupPersistanceRepository.cs", 106);
			}
			return ret;
		}
	}
}
