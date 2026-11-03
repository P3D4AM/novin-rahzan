using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories;
using EntityCache.Business;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repository;

internal class PardakhtCheckShAccountingPersistanceRepository : PardakhtCheckShMasterPersistanceRepository
{
	private bool _isDisposed;

	public PardakhtCheckShAccountingPersistanceRepository(ContextHolder db)
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
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs", 33);
		}
	}

	public override async Task<IEnumerable<PardakhtCheckShReportBusiness>> GetAllAsync(PardakhtChequeFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs", 38));
		IEnumerable<PardakhtCheckShReportBusiness> result;
		try
		{
			IQueryable<PardakhtCheckShReportBusiness> queryable = ((IQueryable<PardakhtCheckSh>)acc_ModelContext.PardakhtCheckSh)?.Select((PardakhtCheckSh q) => new PardakhtCheckShReportBusiness
			{
				A2re_MoeinGuid = q.A2re_MoeinGuid,
				A2re_TafsilGuid = q.A2re_TafsilGuid,
				Aval2re = q.Aval2re,
				Status = q.Status,
				BankTafsilGuid = q.BankTafsilGuid,
				ContextGuid = ContextGuid,
				CreditMoeinGuid = q.CreditMoeinGuid,
				CreditTafsilGuid = q.CreditTafsilGuid,
				DateSaresid = q.DateSaresid,
				DebitA2reMoeinGuid = q.DebitA2reMoeinGuid,
				DebitA2reTafsiGuid = q.DebitA2reTafsiGuid,
				UserGuid = q.UserGuid,
				Description = q.Description,
				Guid = q.Guid,
				Modified = q.Modified,
				MasterGuid = q.MasterGuid,
				IsEnteghali = q.IsEnteghali,
				Number = q.Number,
				PardakhtDate = q.PardakhtDate,
				Price = q.Price,
				ServerStatus = q.ServerStatus,
				BankName = q.BankTafsilItem.Name,
				TafsilName = ((q.Status == ENCheckSHStatus.Entezami_Pardakhtani || q.Status == ENCheckSHStatus.Entezami_Odat) ? q.PardakhtChequeEntezami.DebitTafsilItem.Name : (q.Aval2re ? q.TafsilItem.Name : q.PardakhtItem.TafsilItem.Name)),
				SanadNo = ((q.Status == ENCheckSHStatus.Entezami_Pardakhtani || q.Status == ENCheckSHStatus.Entezami_Odat) ? ((int?)q.PardakhtChequeEntezami.SanadNo) : q.SanadNo),
				TafsilGuid = (q.Aval2re ? q.TafsilItem.Guid : q.PardakhtItem.TafsilItem.Guid),
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiRegistered = q.SayadiRegistered,
				MetaData = q.MetaData.FromJson<PardakhtChequeShMetaData>("", "", 0)
			});
			if (filter != null)
			{
				if (filter.ChequeGuid.HasValue && filter.ChequeGuid != Guid.Empty)
				{
					queryable = queryable?.Where((PardakhtCheckShReportBusiness x) => x.Guid == filter.ChequeGuid);
				}
				if (!string.IsNullOrEmpty(filter.Number))
				{
					queryable = queryable?.Where((PardakhtCheckShReportBusiness x) => x.Number == filter.Number);
				}
				if (filter.Status.HasValue)
				{
					queryable = queryable?.Where((PardakhtCheckShReportBusiness x) => (int?)x.Status == (int?)filter.Status);
				}
				if (filter.DateFrom.HasValue)
				{
					queryable = queryable?.Where((PardakhtCheckShReportBusiness x) => x.DateSaresid >= filter.DateFrom);
				}
				if (filter.DateTo.HasValue)
				{
					queryable = queryable?.Where((PardakhtCheckShReportBusiness x) => x.DateSaresid <= filter.DateTo);
				}
				if (filter.MasterDateFrom.HasValue)
				{
					queryable = queryable.Where((PardakhtCheckShReportBusiness q) => q.MasterGuid != null && q.PardakhtDate >= filter.MasterDateFrom);
				}
				if (filter.MasterDateTo.HasValue)
				{
					queryable = queryable.Where((PardakhtCheckShReportBusiness q) => q.MasterGuid != null && q.PardakhtDate <= filter.MasterDateTo);
				}
				if (filter.IsSayadiRegistered.HasValue)
				{
					queryable = queryable?.Where((PardakhtCheckShReportBusiness q) => q.SayadiRegistered == filter.IsSayadiRegistered.Value);
				}
			}
			result = ((!token.IsCancellationRequested) ? queryable?.ToList() : null);
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs", 101);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await acc_ModelContext.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs", 112));
		IEnumerable<BedehiTalabModel> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<PardakhtCheckSh> queryable = (IQueryable<PardakhtCheckSh>)c.PardakhtCheckSh;
				if (filter != null)
				{
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable?.Where((PardakhtCheckSh q) => q.DateSaresid >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable?.Where((PardakhtCheckSh q) => q.DateSaresid <= filter.DateTo);
					}
					if (filter.Count > 0)
					{
						queryable = queryable?.Take(filter.Count);
					}
				}
				result = ((queryable != null && queryable.Any()) ? (from q in await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token)
					group q by q.DateSarresidSh)?.Select((IGrouping<string, PardakhtCheckShBusiness> q) => new BedehiTalabModel
				{
					Count = q.Count(),
					DateSh = q.Key,
					Sum = q.Sum((PardakhtCheckShBusiness p) => p.Price)
				}) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs", 135);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<PardakhtCheckShBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs", 146));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs"))
		{
			PardakhtCheckShBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					PardakhtCheckSh obj = (await EntityFrameworkQueryableExtensions.ToListAsync<PardakhtCheckSh>(RelationalQueryableExtensions.FromSqlRaw<PardakhtCheckSh>(c.PardakhtCheckSh, $"select * from Acc_PardakhtCheckSh with (updlock, holdlock) where Guid='{guid}'", Array.Empty<object>()), token)).SingleOrDefault();
					if (!string.IsNullOrEmpty(tranName))
					{
						await c.ExecuteNoneQueryAsync($"update Acc_PardakhtCheckSh set modified=getdate() where Guid='{guid}'", null, tranName, token, "", CommandType.Text, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs");
					}
					PardakhtCheckShBusiness pardakhtCheckShBusiness = ((obj == null) ? null : (await MapFromDatabase.MapAsync(base.ContextGuid, obj, null, tranName, token)));
					result = pardakhtCheckShBusiness;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs", 160);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShAccountingPersistanceRepository.cs");
				}
			}
			return result;
		}
	}
}
