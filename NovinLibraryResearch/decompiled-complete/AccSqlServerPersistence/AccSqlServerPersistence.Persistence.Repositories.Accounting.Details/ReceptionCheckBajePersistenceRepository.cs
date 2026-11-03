using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Details;

internal class ReceptionCheckBajePersistenceRepository : ReceptionCheckMasterPersistenceRepository
{
	private ViewCheckMPersistenceRepository _viewRepo;

	private bool _isDisposed;

	public ReceptionCheckBajePersistenceRepository(ContextHolder db)
		: base(db)
	{
		_viewRepo = new ViewCheckMPersistenceRepository(db);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckBajePersistenceRepository.cs", 34);
		}
	}

	public override async Task<ReceptionCheckBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckBajePersistenceRepository.cs", 38));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckBajePersistenceRepository.cs"))
		{
			ReceptionCheckBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 4;
				try
				{
					ReceptionCheck receptionCheck = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ReceptionCheck>((IQueryable<ReceptionCheck>)c.ReceptionCheck, (Expression<Func<ReceptionCheck, bool>>)((ReceptionCheck p) => p.Guid == guid), token);
					if (receptionCheck != null)
					{
						result = await MapFromDatabase.MapAsync(base.ContextGuid, receptionCheck, null, tranName, token);
					}
					else
					{
						ViewCheckMBusiness view = await _viewRepo.GetAsync(guid, token, tranName);
						if (view == null)
						{
							result = null;
						}
						else
						{
							AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token);
							ReceptionCheckBusiness receptionCheckBusiness = new ReceptionCheckBusiness(base.ContextGuid)
							{
								ContextGuid = base.ContextGuid
							};
							ReceptionCheckBusiness receptionCheckBusiness2 = receptionCheckBusiness;
							Guid? a2re_PardazandeMoeinGuid = ((!view.Aval2re) ? null : new Guid?(await view.GetPardazandeMoeinGuid(EnSystem.Mapper, token, tranName)));
							receptionCheckBusiness2.A2re_PardazandeMoeinGuid = a2re_PardazandeMoeinGuid;
							receptionCheckBusiness.A2re_PardazandeTafsilGuid = (view.Aval2re ? new Guid?(view.PardazandeTafsilGuid) : null);
							receptionCheckBusiness.A2re_ReceptionDate = (view.Aval2re ? new DateTime?(view.DateSanadM) : null);
							receptionCheckBusiness.Aval2re = view.Aval2re;
							receptionCheckBusiness.BankName = view.BankName;
							receptionCheckBusiness.CreditA2reMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein15;
							receptionCheckBusiness.CreditA2reTafsilGuid = null;
							receptionCheckBusiness.Date = view.DateCheckM;
							receptionCheckBusiness.DebitMoeinGuid = setting.CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception;
							receptionCheckBusiness.DebitTafsilGuid = setting.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception;
							receptionCheckBusiness.Description = view.Description;
							receptionCheckBusiness.Guid = view.Guid;
							receptionCheckBusiness.HesabNum = view.HesabNum;
							receptionCheckBusiness.MasterGuid = view.GuidSanad;
							receptionCheckBusiness.Modified = DateTime.Now;
							receptionCheckBusiness.Number = view.Number;
							receptionCheckBusiness.Personal = view.Personal;
							receptionCheckBusiness.PoshtNomreh = view.PoshtNomreh;
							receptionCheckBusiness.Status = view.Status;
							receptionCheckBusiness.VisitorGuid = view.VisitorGuid;
							receptionCheckBusiness.IsEnteghali = view.IsEnteghali;
							ReceptionCheckBusiness receptionCheckBusiness3 = receptionCheckBusiness;
							receptionCheckBusiness3.SetPrice(EnSystem.Mapper, view.Price);
							result = receptionCheckBusiness3;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckBajePersistenceRepository.cs", 79);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckBajePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllAsync(CancellationToken token, string tranName, Guid? tafsilGuid)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckBajePersistenceRepository.cs", 90));
		IEnumerable<ReceptionCheckReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				IEnumerable<ViewCheckMBusiness> temp = await _viewRepo.GetAllAsync(null, null, null, null, token, tranName);
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token);
				IEnumerable<ReceptionCheckReportBusiness> enumerable = temp.Select((ViewCheckMBusiness q) => new ReceptionCheckReportBusiness
				{
					A2re_PardazandeTafsilGuid = (q.Aval2re ? new Guid?(q.PardazandeTafsilGuid) : null),
					A2re_ReceptionDate = (q.Aval2re ? new DateTime?(q.DateSanadM) : null),
					Aval2re = q.Aval2re,
					BankName = q.BankName,
					ContextGuid = q.ContextGuid,
					CreditA2reMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein15,
					CreditA2reTafsilGuid = null,
					Date = q.DateCheckM,
					DateBargashtOrVosol = q.DateBargashtORVosolM,
					DateToPlace = q.DateToPlace.ShamsiToMiladi(),
					DebitMoeinGuid = setting.CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception,
					DebitTafsilGuid = setting.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception,
					Description = q.Description,
					Guid = q.Guid,
					HesabNum = q.HesabNum,
					MasterGuid = q.GuidSanad,
					Number = q.Number,
					Place = q.Place,
					PoshtNomreh = q.PoshtNomreh,
					Price = q.Price,
					ReceptionDate = q.DateSanadM,
					Status = q.Status,
					TafsilName = q.CustomerFullName,
					VisitorGuid = q.VisitorGuid,
					IsEnteghali = q.IsEnteghali,
					TafsilGuid = q.PardazandeTafsilGuid,
					SayadiNationalCode = q.Sayadi_NationalCode,
					SayadiIdentity = q.Sayadi_Identity,
					SayadiName = q.Sayadi_Name,
					SayadiRegistered = q.SayadiRegistered
				});
				result = enumerable;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckBajePersistenceRepository.cs", 134);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckBajePersistenceRepository.cs");
			}
		}
		return result;
	}

	public override Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
