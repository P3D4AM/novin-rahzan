using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence.Repositories.Accounting;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class PardakhtCheckShBajePersistanceRepository : PardakhtCheckShMasterPersistanceRepository
{
	private ViewCheckShPersistenceRepository _viewRepo;

	private bool _isDisposed;

	public PardakhtCheckShBajePersistanceRepository(ContextHolder db)
		: base(db)
	{
		_viewRepo = new ViewCheckShPersistenceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_viewRepo?.Dispose();
				_viewRepo = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBajePersistanceRepository.cs", 35);
		}
	}

	public override async Task<IEnumerable<PardakhtCheckShReportBusiness>> GetAllAsync(PardakhtChequeFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBajePersistanceRepository.cs", 39));
		IEnumerable<PardakhtCheckShReportBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IEnumerable<PardakhtCheckShReportBusiness> enumerable = (await _viewRepo.GetAllAsync(null, token, tranName))?.Select((ViewCheckShBusiness q) => new PardakhtCheckShReportBusiness
				{
					A2re_MoeinGuid = q.A2reMoeinGuid,
					A2re_TafsilGuid = q.A2reTafsilGuid,
					Aval2re = q.Aval2re,
					Status = q.ChequeStatus,
					BankTafsilGuid = q.BankGuid,
					ContextGuid = base.ContextGuid,
					CreditMoeinGuid = q.CreditMoeinGuid,
					CreditTafsilGuid = q.CreditTafsilGuid,
					DateSaresid = q.DateSarresidM,
					DebitA2reMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein15,
					DebitA2reTafsiGuid = null,
					Description = q.AutoDescription,
					Guid = q.Guid,
					Modified = q.Modified,
					MasterGuid = q.GuidSanad,
					Number = q.Number,
					PardakhtDate = q.PardakhtDate.ShamsiToMiladi(),
					Price = q.Price,
					ServerStatus = q.ServerStatus,
					BankName = q.BankName,
					TafsilName = q.CustomerFullName,
					IsEnteghali = q.IsEnteghali,
					TafsilGuid = Guid.Empty
				});
				if (filter != null)
				{
					if (filter.ChequeGuid.HasValue && filter.ChequeGuid != Guid.Empty)
					{
						enumerable = enumerable?.Where(delegate(PardakhtCheckShReportBusiness x)
						{
							Guid guid = x.Guid;
							Guid? chequeGuid = filter.ChequeGuid;
							return guid == chequeGuid;
						});
					}
					if (!string.IsNullOrEmpty(filter.Number))
					{
						enumerable = enumerable?.Where((PardakhtCheckShReportBusiness x) => x.Number == filter.Number);
					}
					if (filter.Status.HasValue)
					{
						enumerable = enumerable?.Where((PardakhtCheckShReportBusiness x) => x.Status == filter.Status);
					}
					if (filter.DateFrom.HasValue)
					{
						enumerable = enumerable?.Where(delegate(PardakhtCheckShReportBusiness x)
						{
							DateTime dateSaresid2 = x.DateSaresid;
							DateTime? dateFrom = filter.DateFrom;
							return dateSaresid2 >= dateFrom;
						});
					}
					if (filter.DateTo.HasValue)
					{
						enumerable = enumerable?.Where(delegate(PardakhtCheckShReportBusiness x)
						{
							DateTime dateSaresid = x.DateSaresid;
							DateTime? dateTo = filter.DateTo;
							return dateSaresid <= dateTo;
						});
					}
				}
				result = ((!token.IsCancellationRequested) ? enumerable : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBajePersistanceRepository.cs", 90);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBajePersistanceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<PardakhtCheckShBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBajePersistanceRepository.cs", 101));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBajePersistanceRepository.cs"))
		{
			PardakhtCheckShBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					DbSet<PardakhtCheckSh> pardakhtCheckSh = c.PardakhtCheckSh;
					PardakhtCheckSh pardakhtCheckSh2 = await ((pardakhtCheckSh != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<PardakhtCheckSh>((IQueryable<PardakhtCheckSh>)pardakhtCheckSh, (Expression<Func<PardakhtCheckSh, bool>>)((PardakhtCheckSh x) => x.Guid == guid), token) : null);
					if (pardakhtCheckSh2 != null)
					{
						result = await MapFromDatabase.MapAsync(base.ContextGuid, pardakhtCheckSh2, null, tranName, token);
					}
					else
					{
						ViewCheckShBusiness viewCheckShBusiness = await _viewRepo.GetAsync(guid, token, tranName);
						if (viewCheckShBusiness == null)
						{
							result = null;
						}
						else
						{
							PardakhtCheckShBusiness pardakhtCheckShBusiness = new PardakhtCheckShBusiness(base.ContextGuid)
							{
								ContextGuid = base.ContextGuid,
								A2re_MoeinGuid = viewCheckShBusiness.A2reMoeinGuid,
								A2re_TafsilGuid = viewCheckShBusiness.A2reTafsilGuid,
								Aval2re = viewCheckShBusiness.Aval2re,
								BankName = viewCheckShBusiness.BankName,
								BankTafsilGuid = viewCheckShBusiness.BankGuid,
								CreditMoeinGuid = viewCheckShBusiness.CreditMoeinGuid,
								CreditTafsilGuid = viewCheckShBusiness.CreditTafsilGuid,
								DateSaresid = viewCheckShBusiness.DateSarresidM,
								DebitA2reMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein15,
								DebitA2reTafsiGuid = null,
								RowDesc = viewCheckShBusiness.AutoDescription,
								Guid = viewCheckShBusiness.Guid,
								MasterGuid = viewCheckShBusiness.GuidSanad,
								Modified = DateTime.Now,
								Number = viewCheckShBusiness.Number,
								PardakhtDate = viewCheckShBusiness.PardakhtDate.ShamsiToMiladi(),
								ServerStatus = viewCheckShBusiness.ServerStatus,
								Status = viewCheckShBusiness.ChequeStatus,
								IsEnteghali = viewCheckShBusiness.IsEnteghali,
								TafsilName = viewCheckShBusiness.CustomerFullName,
								ChangeStatus = EntityChangeSt.Edited
							};
							pardakhtCheckShBusiness.SetPrice(EnSystem.Mapper, viewCheckShBusiness.Price);
							result = pardakhtCheckShBusiness;
						}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBajePersistanceRepository.cs", 141);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBajePersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
