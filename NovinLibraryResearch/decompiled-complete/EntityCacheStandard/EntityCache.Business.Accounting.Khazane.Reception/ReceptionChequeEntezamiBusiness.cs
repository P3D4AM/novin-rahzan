using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Khazane.Reception;
using PacketParser.Filters;

namespace EntityCache.Business.Accounting.Khazane.Reception;

public class ReceptionChequeEntezamiBusiness : IReceptionChequeEntezami, IHasModified, IHasContext, IHasChangeStatus
{
	private ReceptionCheckBusiness _cheque;

	public Guid ChequeGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public Guid CashierGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public int SanadNo { get; set; }

	public string Description { get; set; }

	public DateTime ReceptionDateM { get; set; } = DateTime.Now;


	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; } = EntityChangeSt.New;


	public ReceptionChequeEntezamiBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<ReceptionChequeEntezamiBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ReceptionChequeEntezami.GetAsync(guid, token, tranName);
	}

	public async Task<ReceptionCheckBusiness> GetReceptionChequeAsync(ENUMCheck defStatus, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_cheque == null)
			{
				_cheque = await ReceptionCheckBusiness.GetAsync(ContextGuid, ChequeGuid, token, tranName);
			}
			if (_cheque == null)
			{
				_cheque = new ReceptionCheckBusiness(ContextGuid)
				{
					Status = defStatus
				};
			}
			return _cheque;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionChequeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 47);
			throw;
		}
	}

	public void SetReception(ReceptionCheckBusiness value)
	{
		_cheque = value;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 6;
			try
			{
				if (_cheque == null)
				{
					ret.AddError("چک معتبر نمی باشد", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 60);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 61), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 61);
				}
				transactionHasBeenCreated = true;
				if (!ret.HasError && !_cheque.Aval2re && system == EnSystem.UserInterface)
				{
					if (SanadNo <= 0)
					{
						SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
					}
					SanadBusiness sanad = await GenerateSanadAsync(system, tranName, token);
					List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, token, tranName);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.CheckSanad, ChequeGuid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 68);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_0731;
				}
				_cheque.SanadNo = SanadNo;
				_cheque.UserGuid = CashierGuid;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IReceptionCheckRepository.SaveAsync(new List<ReceptionCheckBusiness> { _cheque }, _cheque.MasterGuid ?? Guid.Empty, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 73);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0731;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ReceptionChequeEntezami.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 75);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 79);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 80);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 85), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 85);
			}
		}
		return ret;
		IL_0731:
		return result;
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, string tranName, CancellationToken token)
	{
		_ = 6;
		try
		{
			ENSource source = ClsCache.GetCache(ContextGuid).Source;
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, DebitTafsilGuid.Value, tranName, token, "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs");
			SanadBusiness Sanad = new SanadBusiness(ContextGuid, 0)
			{
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = ContextGuid,
				DateM = DateTime.Now,
				Description = "چک دریافتی انتظامی (" + _cheque.BankName + " - " + _cheque.Number + ") ** طرف حساب " + tafsilBusiness.Name,
				Guid = Guid.NewGuid(),
				IsTemp = true,
				Modified = DateTime.Now,
				SanadNo = SanadNo,
				SanadSt = SanadSt.Movaghat,
				SanadType = SanadType.Automatic,
				Source = source,
				UserGuid = CashierGuid,
				Status = true
			};
			Sanad_DetBusiness debit = new Sanad_DetBusiness(ContextGuid, system, ChequeGuid, Document.CheckSanad, Sanad.NextOrdinal())
			{
				Credit = 0m,
				Debit = _cheque.Price,
				DateSarresid = _cheque.Date,
				RowDesc = Sanad.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = Sanad.Guid,
				Modified = DateTime.Now,
				Serial = _cheque.Number,
				Status = true
			};
			await debit.SetMoeinGuidAsync(DebitMoeinGuid);
			await debit.SetTafsilGuidAsync(DebitTafsilGuid, "", token);
			await Sanad.AddToListSanadAsync(debit, system, token);
			Sanad_DetBusiness credit = new Sanad_DetBusiness(ContextGuid, system, ChequeGuid, Document.CheckSanad, Sanad.NextOrdinal())
			{
				Credit = _cheque.Price,
				Debit = 0m,
				DateSarresid = _cheque.Date,
				RowDesc = Sanad.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = Sanad.Guid,
				Modified = DateTime.Now,
				Serial = _cheque.Number,
				Status = true
			};
			await credit.SetMoeinGuidAsync(CreditMoeinGuid);
			await credit.SetTafsilGuidAsync(CreditTafsilGuid, "", token);
			await Sanad.AddToListSanadAsync(credit, system, token);
			return Sanad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 148);
			throw;
		}
	}

	public async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, CancellationToken token, string tranName)
	{
		try
		{
			if (system == EnSystem.Transfer)
			{
				return null;
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, DebitTafsilGuid.Value, tranName, token, "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs");
			List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
			list.Add(new ViewSanadDetailCoverBusiness(ContextGuid)
			{
				DebitMoeinGuid = DebitMoeinGuid,
				DebitTafsilGuid = DebitTafsilGuid,
				Date = ReceptionDateM,
				AsGuid = ChequeGuid,
				AsType = DocumentDet.ChequeSanad,
				AsTypeDocument = Document.CheckSanad,
				Debit = 0m,
				Credit = _cheque.Price,
				CashierGuid = CashierGuid,
				SanadNo = SanadNo,
				SanadSt = SanadSt.Movaghat,
				Id2 = 0,
				CreditMoeinGuid = CreditMoeinGuid,
				CreditTafsilGuid = CreditTafsilGuid,
				SanadDesc = "چک دریافتی انتظامی (" + _cheque.BankName + " - " + _cheque.Number + ") ** طرف حساب " + tafsilBusiness.Name,
				RDesc = Description,
				DateMoaser = ReceptionDateM,
				DetailGuid = ChequeGuid,
				Guid = Guid.NewGuid(),
				DecimalGuid = null,
				DecimalPrice = 0m,
				DecimalQt = 0m,
				DepotGuid = null,
				Modified = DateTime.Now,
				Price = 0m,
				ProductGuid = null,
				Qt = 0m,
				Qt2 = 0m,
				RBazaryab = 0m,
				RDiscount = 0m,
				Serial = _cheque.Number,
				Status = true,
				VahedGuid = null
			});
			list.Add(new ViewSanadDetailCoverBusiness(ContextGuid)
			{
				DebitMoeinGuid = DebitMoeinGuid,
				DebitTafsilGuid = DebitTafsilGuid,
				Date = ReceptionDateM,
				AsGuid = ChequeGuid,
				AsType = DocumentDet.ChequeSanad,
				AsTypeDocument = Document.CheckSanad,
				Debit = _cheque.Price,
				Credit = 0m,
				CashierGuid = CashierGuid,
				SanadNo = SanadNo,
				SanadSt = SanadSt.Movaghat,
				Id2 = 0,
				CreditMoeinGuid = CreditMoeinGuid,
				CreditTafsilGuid = CreditTafsilGuid,
				SanadDesc = "چک دریافتی انتظامی (" + _cheque.BankName + " - " + _cheque.Number + ") ** طرف حساب " + tafsilBusiness.Name,
				RDesc = Description,
				DateMoaser = ReceptionDateM,
				DetailGuid = ChequeGuid,
				Guid = Guid.NewGuid(),
				DecimalGuid = null,
				DecimalPrice = 0m,
				DecimalQt = 0m,
				DepotGuid = null,
				Modified = DateTime.Now,
				Price = 0m,
				ProductGuid = null,
				Qt = 0m,
				Qt2 = 0m,
				RBazaryab = 0m,
				RDiscount = 0m,
				Serial = _cheque.Number,
				Status = true,
				VahedGuid = null
			});
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 237);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			List<ReceptionCheckReportBusiness> list = new List<ReceptionCheckReportBusiness>();
			ReceptionChequeFilter filter = new ReceptionChequeFilter
			{
				Status = ENUMCheck.Entezami_Mojod
			};
			IEnumerable<ReceptionCheckReportBusiness> enumerable = await ReceptionCheckBusiness.GetAllAsync(contextGuid, filter, default(CancellationToken));
			if (enumerable != null && enumerable.Any())
			{
				list.AddRange(enumerable);
			}
			filter = new ReceptionChequeFilter
			{
				Status = ENUMCheck.Entezami_Odat
			};
			IEnumerable<ReceptionCheckReportBusiness> enumerable2 = await ReceptionCheckBusiness.GetAllAsync(contextGuid, filter, default(CancellationToken));
			if (enumerable2 != null && enumerable2.Any())
			{
				list.AddRange(enumerable2);
			}
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			foreach (ReceptionCheckReportBusiness item in list)
			{
				if ((item?.SanadNo).GetValueOrDefault() <= 0 || await SanadBusiness.GetAsync(contextGuid, item.SanadNo.Value) != null)
				{
					continue;
				}
				ReceptionChequeEntezamiBusiness chequeEntezami = await GetAsync(contextGuid, item.Guid);
				if (chequeEntezami != null)
				{
					ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(contextGuid, item.Guid);
					if (receptionCheckBusiness != null)
					{
						chequeEntezami.SetReception(receptionCheckBusiness);
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await chequeEntezami.SaveAsync(EnSystem.UserInterface), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 268);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 273);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiBusiness.cs", 274);
		}
		return res;
	}
}
