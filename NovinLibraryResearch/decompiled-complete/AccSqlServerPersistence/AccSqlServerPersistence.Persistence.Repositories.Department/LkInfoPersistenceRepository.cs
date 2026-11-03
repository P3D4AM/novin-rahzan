using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Department;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Department;
using EntityCache.Core;
using EntityCache.Core.Repository.Department;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Department;

internal class LkInfoPersistenceRepository : PersistenceNoGuidRepository<LkInfoBusiness, LkInfo>, ILkInfoRepository, INoGuidRepository<LkInfoBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<LkInfoBusiness, LkInfo>, IItemAsyncMapper<LkInfo, LkInfoBusiness>, IItemMapper<LkInfo, LkInfoBusiness>
	{
		public Task<LkInfo> MapAsync(string contextGuid, LkInfoBusiness source, LkInfo destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new LkInfo();
				}
				destination.Guid = source.Guid;
				destination.AccountingPackage = source.AccountingPackage;
				destination.TypeAccounting = source.TypeAccounting;
				destination.HozoorType = source.HozoorType;
				destination.LKSerial = source.LKSerial;
				destination.TafsilGuid = source.TafsilGuid;
				destination.ExpireDate = source.ExpireDate;
				destination.StartDate = source.StartDate;
				destination.Description = source.Description;
				destination.Tolid = source.Tolid;
				destination.BarCode = source.BarCode;
				destination.Scale = source.Scale;
				destination.SpecialAccountingPackage = source.SpecialAccountingPackage;
				destination.Hozor_Hoghogh = source.Hozor_Hoghogh;
				destination.ScaleCount = source.ScaleCount;
				destination.MaxActivationDays = source.MaxActivationDays;
				destination.Net_Accounting = source.Net_Accounting;
				destination.Net_Hozor = source.Net_Hozor;
				destination.Net_Scales = source.Net_Scales;
				destination.CallerID = source.CallerID;
				destination.FailedDate = source.FailedDate;
				destination.FailedDescription = source.FailedDescription;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DisableActivationCodeDate = source.DisableActivationCodeDate;
				destination.SpecialFacilities = source.SpecialFacilities;
				destination.IsOffline = source.IsOffline;
				destination.MetaData = Json.ToStringJson(source.MetaData);
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 243);
				throw;
			}
		}

		public Task<LkInfoBusiness> MapAsync(string contextGuid, LkInfo source, LkInfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public LkInfoBusiness Map(string contextGuid, LkInfo source, LkInfoBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new LkInfoBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.AccountingPackage = source.AccountingPackage;
				destination.TypeAccounting = source.TypeAccounting;
				destination.HozoorType = source.HozoorType;
				destination.LKSerial = source.LKSerial;
				destination.TafsilGuid = source.TafsilGuid;
				destination.ExpireDate = source.ExpireDate;
				destination.StartDate = source.StartDate;
				destination.Description = source.Description;
				destination.Tolid = source.Tolid;
				destination.BarCode = source.BarCode;
				destination.Scale = source.Scale;
				destination.SpecialAccountingPackage = source.SpecialAccountingPackage;
				destination.Hozor_Hoghogh = source.Hozor_Hoghogh;
				destination.ScaleCount = source.ScaleCount;
				destination.MaxActivationDays = source.MaxActivationDays;
				destination.Net_Accounting = source.Net_Accounting;
				destination.Net_Hozor = source.Net_Hozor;
				destination.Net_Scales = source.Net_Scales;
				destination.CallerID = source.CallerID;
				destination.FailedDate = source.FailedDate;
				destination.FailedDescription = source.FailedDescription;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.ChangeStatus = EntityChangeSt.Edited;
				destination.DisableActivationCodeDate = source.DisableActivationCodeDate;
				destination.SpecialFacilities = source.SpecialFacilities;
				destination.IsOffline = source.IsOffline;
				if (!string.IsNullOrEmpty(source.MetaData))
				{
					destination.MetaData = source.MetaData.FromJson<LkInfoMeta>("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 295);
				}
				return destination;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 300);
				throw;
			}
		}

		public Task<List<LkInfo>> MapListAsync(string contextGuid, IEnumerable<LkInfoBusiness> source, List<LkInfo> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<LkInfoBusiness>> MapListAsync(string contextGuid, IEnumerable<LkInfo> source, List<LkInfoBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public bool HasStatus => true;

	public bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public IItemMapper<LkInfo, LkInfoBusiness> MapFromDatabaseNoneAsync => Mapper;

	public override IItemAsyncMapper<LkInfo, LkInfoBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<LkInfoBusiness, LkInfo> MapToDatabase => Mapper;

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(LkInfoBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Dpt_ModelContext dbcontext = (Dpt_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 33));
				LkInfo lkInfo = await MapToDatabase.MapAsync(base.ContextGuid, item, null, "", token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(lkInfo, lkInfo.LKSerial), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 35);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 39);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 40);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<LkInfoBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs"))
		{
			try
			{
				foreach (LkInfoBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 52);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 56);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 57);
			}
			return ret;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 71);
		}
	}

	public async Task<LkInfoBusiness> GetAsync(long lkSerial, CancellationToken token, string transaction)
	{
		Dpt_ModelContext c = (Dpt_ModelContext)(await dbContext.GetContextAsync(transaction, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 76));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs"))
		{
			LkInfoBusiness result;
			try
			{
				_ = 1;
				try
				{
					LkInfo source = ((IQueryable<LkInfo>)c.LkInfo).FirstOrDefault((LkInfo x) => x.LKSerial == lkSerial);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 85);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(transaction))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<LkInfoBusiness>> GetAllAsync(LkModel filters, CancellationToken token, string tranName)
	{
		Dpt_ModelContext c = (Dpt_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 96));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs"))
		{
			List<LkInfoBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<LkInfo> queryable = ((IQueryable<LkInfo>)c.LkInfo).Where((LkInfo p) => p.Status == true);
					if (filters != null)
					{
						if (filters.BeginExpireDate.HasValue)
						{
							queryable = queryable.Where((LkInfo q) => q.ExpireDate >= filters.BeginExpireDate);
						}
						if (filters.EndExpireDate.HasValue)
						{
							queryable = queryable.Where((LkInfo q) => q.ExpireDate <= filters.EndExpireDate);
						}
						if (filters.IsOffline.HasValue)
						{
							queryable = queryable.Where((LkInfo q) => (bool?)q.IsOffline == filters.IsOffline);
						}
						if (filters.IsAccounting || filters.IsAttendance)
						{
							ENCode[] allowedAccountings = new ENCode[4]
							{
								ENCode.ForoshgahiSade,
								ENCode.ForoshgahiMotavaset,
								ENCode.ForoshgahiPishrafteh,
								ENCode.Jame
							};
							if (filters.IsAccounting)
							{
								queryable = queryable.Where((LkInfo q) => allowedAccountings.Contains(q.TypeAccounting));
								if (filters.LevelLkAccounting != 0)
								{
									queryable = queryable.Where((LkInfo q) => q.TypeAccounting == filters.LevelLkAccounting);
								}
							}
							else
							{
								queryable = queryable.Where((LkInfo q) => !allowedAccountings.Contains(q.TypeAccounting));
							}
							TypeHozor[] allowedAttendances = new TypeHozor[5]
							{
								TypeHozor.Level1,
								TypeHozor.Level2,
								TypeHozor.Level3,
								TypeHozor.Level4,
								TypeHozor.Level5
							};
							if (filters.IsAttendance)
							{
								queryable = queryable.Where((LkInfo q) => allowedAttendances.Contains(q.Hozor_Hoghogh));
								if (filters.LevelLkAttebdance != 0)
								{
									queryable = queryable.Where((LkInfo q) => q.Hozor_Hoghogh == filters.LevelLkAttebdance);
								}
							}
							else
							{
								queryable = queryable.Where((LkInfo q) => !allowedAttendances.Contains(q.Hozor_Hoghogh));
							}
						}
						if (filters.StatusLock != 0)
						{
							queryable = ((filters.StatusLock != StatusLock.Blocked) ? queryable.Where((LkInfo q) => q.FailedDate == null) : queryable.Where((LkInfo q) => q.FailedDate != null));
						}
						if ((filters?.TafsilGuid?.Any()).GetValueOrDefault())
						{
							Guid tafsilGuid = filters.TafsilGuid.FirstOrDefault();
							queryable = queryable.Where((LkInfo p) => p.TafsilGuid == tafsilGuid);
						}
						if (!string.IsNullOrEmpty(filters?.LkSearial))
						{
							queryable = (from p in queryable.AsEnumerable()
								where p.LKSerial.ToString().Contains(filters.LkSearial)
								select p).AsQueryable();
						}
					}
					token.ThrowIfCancellationRequested();
					if (!queryable.Any())
					{
						result = null;
					}
					else
					{
						List<LkInfo> source = await EntityFrameworkQueryableExtensions.ToListAsync<LkInfo>(queryable, token);
						token.ThrowIfCancellationRequested();
						result = await MapFromDatabase.MapListAsync(base.ContextGuid, source, new List<LkInfoBusiness>(), tranName, token);
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 168);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<int> GetAllGuidsDontLock()
	{
		throw new NotImplementedException();
	}

	public Task<int> GetAllGuidsHasLock()
	{
		throw new NotImplementedException();
	}

	public LkInfoPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<LkInfoBusiness> GetAsync(Guid guid, string tranName)
	{
		throw new NotImplementedException();
	}

	public LkInfoBusiness Get(Guid guid, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<LkInfoBusiness> GetFromDatabaseAsync(Guid guid, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<LkInfoBusiness>> GetAllAsync(bool? status, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<List<long>> GetAllNotSentAsync(string tranName)
	{
		Dpt_ModelContext c = (Dpt_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 357));
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs"))
		{
			List<long> result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.ToListAsync<long>(from p in (IQueryable<LkInfo>)c.LkInfo
						where p.ServerStatus == ServerStatusForClient.None
						select p.LKSerial, default(CancellationToken));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs", 365);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccDepartmentSqlServerPersistene\\Department\\LkInfoPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
