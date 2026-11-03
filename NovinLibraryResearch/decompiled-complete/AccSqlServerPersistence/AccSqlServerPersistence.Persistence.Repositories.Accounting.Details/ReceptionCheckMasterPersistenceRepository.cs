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
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Details;

internal abstract class ReceptionCheckMasterPersistenceRepository : PersistenceMasterRepository<ReceptionCheckBusiness, ReceptionCheck>, IReceptionCheckRepository, IPersistenceMasterRepository<ReceptionCheckBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ReceptionCheckBusiness, ReceptionCheck>, IItemAsyncMapper<ReceptionCheck, ReceptionCheckBusiness>
	{
		public Task<ReceptionCheck> MapAsync(string contextGuid, ReceptionCheckBusiness source, ReceptionCheck destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs"))
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
						destination = new ReceptionCheck();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.BankName = source.BankName;
					destination.Date = source.Date;
					destination.Status = source.Status;
					destination.MasterGuid = source.MasterGuid;
					destination.A2re_PardazandeTafsilGuid = source.A2re_PardazandeTafsilGuid;
					destination.A2re_PardazandeMoeinGuid = source.A2re_PardazandeMoeinGuid;
					destination.A2re_ReceptionDate = source.A2re_ReceptionDate;
					destination.Aval2re = source.Aval2re;
					destination.Description = source.Description ?? "";
					destination.HesabNum = source.HesabNum;
					destination.Number = source.Number;
					destination.Personal = source.Personal;
					destination.PoshtNomreh = source.PoshtNomreh;
					destination.Price = source.Price;
					destination.ServerStatus = source.ServerStatus;
					destination.UserGuid = source.UserGuid;
					destination.IsEnteghali = source.IsEnteghali;
					destination.DebitMoeinGuid = source.DebitMoeinGuid;
					destination.DebitTafsilGuid = source.DebitTafsilGuid;
					destination.CreditA2reMoeinGuid = source.CreditA2reMoeinGuid;
					destination.CreditA2reTafsilGuid = source.CreditA2reTafsilGuid;
					destination.SanadNo = source.SanadNo;
					destination.SayadiName = source.SayadiName;
					destination.SayadiIdentity = source.SayadiIdentity;
					destination.SayadiNationalCode = source.SayadiNationalCode;
					destination.MetaData = Json.ToStringJson(source.MetaData);
					destination.SayadiRegistered = source.SayadiRegistered;
					destination.Ordinal = source.Ordinal;
					destination.VisitorGuid = source.VisitorGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 205);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<ReceptionCheckBusiness> MapAsync(string contextGuid, ReceptionCheck source, ReceptionCheckBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs"))
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
						destination = new ReceptionCheckBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.BankName = source.BankName;
					destination.Date = source.Date;
					destination.Status = source.Status;
					destination.MasterGuid = source.MasterGuid;
					destination.UserGuid = source.UserGuid;
					destination.IsEnteghali = source.IsEnteghali;
					destination.A2re_PardazandeTafsilGuid = source.A2re_PardazandeTafsilGuid;
					destination.A2re_PardazandeMoeinGuid = source.A2re_PardazandeMoeinGuid;
					destination.A2re_ReceptionDate = source.A2re_ReceptionDate;
					destination.Aval2re = source.Aval2re;
					destination.Description = source.Description;
					destination.HesabNum = source.HesabNum;
					destination.Number = source.Number;
					destination.Personal = source.Personal;
					destination.PoshtNomreh = source.PoshtNomreh;
					destination.SetPrice(EnSystem.Mapper, source.Price);
					destination.ServerStatus = source.ServerStatus;
					destination.DebitMoeinGuid = source.DebitMoeinGuid;
					destination.DebitTafsilGuid = source.DebitTafsilGuid;
					destination.CreditA2reMoeinGuid = source.CreditA2reMoeinGuid;
					destination.CreditA2reTafsilGuid = source.CreditA2reTafsilGuid;
					destination.SanadNo = source.SanadNo;
					destination.SayadiName = source.SayadiName;
					destination.SayadiIdentity = source.SayadiIdentity;
					destination.SayadiNationalCode = source.SayadiNationalCode;
					destination.MetaData = source.MetaData.FromJson<ReceptionChequeMetaData>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 249);
					destination.SayadiRegistered = source.SayadiRegistered;
					destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
					destination.VisitorGuid = source.VisitorGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 259);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<ReceptionCheck>> MapListAsync(string contextGuid, IEnumerable<ReceptionCheckBusiness> source, List<ReceptionCheck> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ReceptionCheckBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionCheck> source, List<ReceptionCheckBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ReceptionCheck, ReceptionCheckBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ReceptionCheckBusiness, ReceptionCheck> MapToDatabase => Mapper;

	public ReceptionCheckMasterPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public abstract Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllAsync(CancellationToken token, string tranName, Guid? tafsilGuid);

	public override Task<ReturnedSaveFuncInfo> RemoveAsync(Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<string> NextPoshtNomreAsync(int checkCount, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "NextPoshtNomreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 34));
		using (new JaegerMonitor("NextPoshtNomreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs"))
		{
			string result;
			try
			{
				_ = 1;
				try
				{
					result = (((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ReceptionCheck>(((IQueryable<ReceptionCheck>)c.ReceptionCheck).OrderByDescending((ReceptionCheck x) => x.PoshtNomreh).Take(1), token))?.PoshtNomreh ?? 0) + 1).ToString();
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
					WebErrorLog.Instance.StartLog(ex3, "", "NextPoshtNomreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 46);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("NextPoshtNomreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public abstract Task<ReceptionCheckBusiness> GetAsync(Guid guid, CancellationToken token, string tranName);

	public abstract Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName);

	public async Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllAsync(ReceptionChequeFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs"))
		{
			try
			{
				IEnumerable<ReceptionCheckReportBusiness> enumerable = await GetAllAsync(token, tranName, filter.TafsilGuid);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return null;
				}
				if (filter != null)
				{
					if (filter.Status.HasValue)
					{
						enumerable = enumerable.Where((ReceptionCheckReportBusiness q) => q.Status == filter.Status);
					}
					if (!string.IsNullOrEmpty(filter.Number))
					{
						enumerable = enumerable.Where((ReceptionCheckReportBusiness q) => q.Number == filter.Number);
					}
					if (filter.DateFrom.HasValue)
					{
						enumerable = enumerable.Where((ReceptionCheckReportBusiness q) => q.Date >= filter.DateFrom.Value);
					}
					if (filter.DateTo.HasValue)
					{
						enumerable = enumerable.Where((ReceptionCheckReportBusiness q) => q.Date <= filter.DateTo.Value);
					}
					if (filter.PoshtNomre != 0L)
					{
						enumerable = enumerable.Where((ReceptionCheckReportBusiness q) => q.PoshtNomreh == filter.PoshtNomre);
					}
					if (filter.TafsilGuid.HasValue && filter.TafsilGuid != Guid.Empty)
					{
						enumerable = enumerable.Where(delegate(ReceptionCheckReportBusiness q)
						{
							Guid? tafsilGuid = q.TafsilGuid;
							Guid? tafsilGuid2 = filter.TafsilGuid;
							if (tafsilGuid.HasValue != tafsilGuid2.HasValue)
							{
								return false;
							}
							return !tafsilGuid.HasValue || tafsilGuid.GetValueOrDefault() == tafsilGuid2.GetValueOrDefault();
						});
					}
					if (filter.MasterGuid.HasValue && filter.MasterGuid != Guid.Empty)
					{
						enumerable = enumerable.Where(delegate(ReceptionCheckReportBusiness q)
						{
							Guid? masterGuid = q.MasterGuid;
							Guid? masterGuid2 = filter.MasterGuid;
							if (masterGuid.HasValue != masterGuid2.HasValue)
							{
								return false;
							}
							return !masterGuid.HasValue || masterGuid.GetValueOrDefault() == masterGuid2.GetValueOrDefault();
						});
					}
					if (filter.MasterDateFrom.HasValue)
					{
						enumerable = enumerable.Where((ReceptionCheckReportBusiness q) => q.MasterGuid.HasValue && q.ReceptionDate >= filter.MasterDateFrom);
					}
					if (filter.MasterDateTo.HasValue)
					{
						enumerable = enumerable.Where((ReceptionCheckReportBusiness q) => q.MasterGuid.HasValue && q.ReceptionDate <= filter.MasterDateTo);
					}
					if ((filter?.DebitTafsilGuid ?? Guid.Empty) != Guid.Empty)
					{
						enumerable = enumerable?.Where(delegate(ReceptionCheckReportBusiness q)
						{
							if (q.DebitTafsilGuid.HasValue)
							{
								Guid? debitTafsilGuid = q.DebitTafsilGuid;
								Guid? debitTafsilGuid2 = filter.DebitTafsilGuid;
								if (debitTafsilGuid.HasValue != debitTafsilGuid2.HasValue)
								{
									return false;
								}
								if (!debitTafsilGuid.HasValue)
								{
									return true;
								}
								return debitTafsilGuid.GetValueOrDefault() == debitTafsilGuid2.GetValueOrDefault();
							}
							return false;
						});
					}
					if (filter?.IsSayadiRegistered.HasValue ?? false)
					{
						enumerable = enumerable?.Where((ReceptionCheckReportBusiness q) => q.SayadiRegistered == filter.IsSayadiRegistered.Value);
					}
					if ((filter?.PlaceGuid ?? Guid.Empty) != Guid.Empty)
					{
						enumerable = enumerable?.Where((ReceptionCheckReportBusiness q) => q.PlaceGuid == filter.PlaceGuid.Value);
					}
					if (filter.IsPersonal.HasValue)
					{
						enumerable = enumerable?.Where((ReceptionCheckReportBusiness q) => q.Personal == filter.IsPersonal.Value);
					}
				}
				if (IsDisposed || token.IsCancellationRequested)
				{
					return null;
				}
				return enumerable;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 92);
				throw;
			}
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 105);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<ReceptionCheckBusiness> items, Guid masterGuid, short trycount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (items?.Any() ?? false)
			{
				foreach (ReceptionCheckBusiness item in items)
				{
					ReceptionCheckBusiness receptionCheckBusiness = await GetAsync(item.Guid, token, tranName);
					if (receptionCheckBusiness != null && receptionCheckBusiness.MetaData != null && item.MetaData == null)
					{
						throw new Exception("metaData for aghsatM is removed!");
					}
					item.ServerStatus = ServerStatusForClient.None;
					item.Modified = DateTime.Now;
				}
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.SaveAsync(items, masterGuid, trycount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 122);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 126);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 127);
		}
		return res;
	}

	public async Task<ReceptionCheckBusiness> GetAsync(string number, CancellationToken token, ENUMCheck? status, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 134));
		ReceptionCheckBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				ReceptionCheck receptionCheck = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ReceptionCheck>((IQueryable<ReceptionCheck>)c.ReceptionCheck, (Expression<Func<ReceptionCheck, bool>>)((ReceptionCheck q) => q.Number == number && (status == null || (int)q.Status == (int)((ENUMCheck?)status).Value)), token);
				result = ((receptionCheck != null) ? (await MapFromDatabase.MapAsync(base.ContextGuid, receptionCheck, null, tranName, token)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs", 145);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckMasterPersistenceRepository.cs");
			}
		}
		return result;
	}
}
